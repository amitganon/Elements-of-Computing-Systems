using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class Function : JackProgramElement
    {
        //The various elements of the grammar are maintained in the fields of the object
        public VarDeclaration.VarTypeEnum ReturnType { get; private set; }
        public string Name { get; private set; }
        public List<VarDeclaration> Args { get; private set; }
        public List<VarDeclaration> Locals { get; private set; }
        public List<StatetmentBase> Body { get; private set; }
        public ReturnStatement Return { get; private set; }

        public Function()
        {
            Args = new List<VarDeclaration>();
            Locals = new List<VarDeclaration>();
            Body = new List<StatetmentBase>();
        }

        //This is an example of the implementation of the Parse method 
        public override void Parse(TokensStack sTokens)
        {
            //We check that the first token is "function"
            Token tFunc = sTokens.Pop();
            if (!(tFunc is Statement) || ((Statement)tFunc).Name != "function")
                throw new SyntaxErrorException("Expected function received: " + tFunc, tFunc);
            //Now there should be the return type. We pop it from the stack, check for errors, and then set the field
            Token tType = sTokens.Pop();
            if(!(tType is VarType))
                 throw new SyntaxErrorException("Expected var type, received " + tType, tType);
            ReturnType = VarDeclaration.GetVarType(tType);
            //Next is the function name
            Token tName = sTokens.Pop();
            if(!(tName is Identifier))
                throw new SyntaxErrorException("Expected function name, received " + tType, tType);
            Name = ((Identifier)tName).Name;

            //After the name there should be opening paranthesis for the arguments
            Token t = sTokens.Pop(); //(
            //Now we extract the arguments from the stack until we see a closing parathesis
            while(sTokens.Count > 0 && !(sTokens.Peek() is Parentheses))//)
            {
                //For each argument there should be a type, and a name
                if (sTokens.Count < 3)
                    throw new SyntaxErrorException("Early termination ", t);
                Token tArgType = sTokens.Pop();
                Token tArgName = sTokens.Pop();
                VarDeclaration vc = new VarDeclaration(tArgType, tArgName);
                Args.Add(vc);
                //If there is a comma, then there is another argument
                if (sTokens.Count > 0 && sTokens.Peek() is Separator)//,
                    sTokens.Pop(); 
            }
            //Now we pop out the ) and the {. Note that you need to check that the stack contains the correct symbols here.
            t = sTokens.Pop();//)
            t = sTokens.Pop();//{

            //Now we parse the list of local variable declarations
            while (sTokens.Count > 0 && (sTokens.Peek() is Statement) && (((Statement)sTokens.Peek()).Name == "var"))
            {
                VarDeclaration local = new VarDeclaration();
                //We call the Parse method of the VarDeclaration, which is responsible to parsing the elements of the variable declaration
                local.Parse(sTokens);
                Locals.Add(local);
            }

            //Now we parse the list of statements
            while (sTokens.Count > 0 && !(sTokens.Peek() is Parentheses))
            {
                //We create the correct Statement type (if, while, return, let) based on the top token in the stack
                StatetmentBase s = StatetmentBase.Create(sTokens.Peek());
                //And call the Parse method of the statement to parse the different parts of the statement 
                s.Parse(sTokens);
                Body.Add(s);
            }
            //Need to check here that the last statement is a return statement
//            if (!(sTokens.Peek() is Statement) || ((Statement)sTokens.Peek()).Name != "Return")
//                throw new SyntaxErrorException("can't find return statement",new Token());
            //Finally, the function should end with }
            Token tEnd = sTokens.Pop();//}
        }

        public override string ToString()
        {
            string sFunction = "function " + ReturnType + " " + Name + "(";
            for (int i = 0; i < Args.Count - 1; i++)
                sFunction += Args[i].Type + " " + Args[i].Name + ",";
            if(Args.Count > 0)
                sFunction += Args[Args.Count - 1].Type + " " + Args[Args.Count - 1].Name;
            sFunction += "){\n";
            foreach (VarDeclaration v in Locals)
                sFunction += "\t\t" + v + "\n";
            foreach (StatetmentBase s in Body)
                sFunction += "\t\t" + s + "\n";
            sFunction += "\t}";
            return sFunction;
        }
    }
}
