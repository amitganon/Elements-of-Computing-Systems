using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class Compiler
    {


        public Compiler()
        {

        }


        public List<VarDeclaration> ParseVarDeclarations(List<string> lVarLines)
        {
            List<VarDeclaration> lVars = new List<VarDeclaration>();
            for(int i = 0; i < lVarLines.Count; i++)
            {
                List<Token> lTokens = Tokenize(lVarLines[i], i);
                TokensStack stack = new TokensStack(lTokens);
                VarDeclaration var = new VarDeclaration();
                var.Parse(stack);
                lVars.Add(var);
            }
            return lVars;
        }


        public List<LetStatement> ParseAssignments(List<string> lLines)
        {
            List<LetStatement> lParsed = new List<LetStatement>();
            List<Token> lTokens = Tokenize(lLines);
            TokensStack sTokens = new TokensStack();
            for (int i = lTokens.Count - 1; i >= 0; i--)
                sTokens.Push(lTokens[i]);
            while(sTokens.Count > 0)
            {
                LetStatement ls = new LetStatement();
                ls.Parse(sTokens);
                lParsed.Add(ls);

            }
            return lParsed;
        }

 

        public List<string> GenerateCode(LetStatement aSimple, Dictionary<string, int> dSymbolTable)
        {
            List<string> lAssembly = new List<string>();
            String name = aSimple.Variable;
            Expression exp = aSimple.Value;
            if (!dSymbolTable.ContainsKey(name))//todo this throw exception, mabey it will solve the test problem
                throw new SyntaxErrorException("The given key" + name + "was not present in the dictionary", new Token());
            int index = dSymbolTable[name];
            if(index == 5)
            {
                int x = 3;
            }
            lAssembly.Add("@" + index);
            lAssembly.Add("D=A");
            lAssembly.Add("@LCL");
            lAssembly.Add("A=M");
            lAssembly.Add("D=A+D");
            lAssembly.Add("@ADDRESS");
            lAssembly.Add("M=D");
            if (exp is BinaryOperationExpression)
            {
                BinaryOperationExpression bExp = (BinaryOperationExpression)exp;
                if (bExp.Operand1 is NumericExpression)
                {
                    lAssembly.Add("@" + ((NumericExpression)bExp.Operand1).Value);
                    lAssembly.Add("D=A");
                    lAssembly.Add("@OPERAND1");
                    lAssembly.Add("M=D");
                }
                else
                {
                    int variableIndex = dSymbolTable[((VariableExpression)bExp.Operand1).Name];
                    lAssembly.Add("@" + variableIndex);
                    lAssembly.Add("D=A");
                    lAssembly.Add("@LCL");
                    lAssembly.Add("D=M+D");
                    lAssembly.Add("A=D");
                    lAssembly.Add("D=M");
                    lAssembly.Add("@OPERAND1");
                    lAssembly.Add("M=D");
                }

                if (bExp.Operand2 is NumericExpression)
                {
                    lAssembly.Add("@" + ((NumericExpression)bExp.Operand2).Value);
                    lAssembly.Add("D=A");
                    lAssembly.Add("@OPERAND2");
                    lAssembly.Add("M=D");
                }
                else
                {
                    int variableIndex = dSymbolTable[((VariableExpression)bExp.Operand2).Name];
                    lAssembly.Add("@" + variableIndex);
                    lAssembly.Add("D=A");
                    lAssembly.Add("@LCL");
                    lAssembly.Add("D=M+D");
                    lAssembly.Add("A=D");
                    lAssembly.Add("D=M");
                    lAssembly.Add("@OPERAND2");
                    lAssembly.Add("M=D");
                }
                lAssembly.Add("@OPERAND1");
                lAssembly.Add("D=M");
                lAssembly.Add("@OPERAND2");
                if (bExp.Operator == "-")
                    lAssembly.Add("D=D-M");
                else
                    lAssembly.Add("D=M+D");
                lAssembly.Add("@ADDRESS");
                lAssembly.Add("A=M");
                lAssembly.Add("M=D");

            }
            else if (exp is NumericExpression)
            {
                lAssembly.Add("@" +((NumericExpression)exp).Value);
                lAssembly.Add("D=A");
                lAssembly.Add("@ADDRESS");
                lAssembly.Add("A=M");
                lAssembly.Add("M=D");
            }
            else if (exp is VariableExpression)
            {
                int variableIndex = dSymbolTable[((VariableExpression)exp).Name];
                lAssembly.Add("@" + variableIndex);
                lAssembly.Add("D=A");
                lAssembly.Add("@LCL");                
                lAssembly.Add("D=M+D");
                lAssembly.Add("A=D");
                lAssembly.Add("D=M");
                lAssembly.Add("@ADDRESS");
                lAssembly.Add("A=M");
                lAssembly.Add("M=D");
                
            }
            else
                throw new ArgumentException("Expected expression got:"+exp);

            return lAssembly;
        }

        public Dictionary<string, int> ComputeSymbolTable(List<VarDeclaration> lDeclerations)
        {
            Dictionary<string, int> dTable = new Dictionary<string, int>();
            //add here code to comptue a symbol table for the given var declarations
            //real vars should come before (lower indexes) than artificial vars (starting with _), and their indexes must be by order of appearance.
            //for example, given the declarations:
            //var int x;
            //var int _1;
            //var int y;
            //the resulting table should be x=0,y=1,_1=2
            //throw an exception if a var with the same name is defined more than once
            List<VarDeclaration> artificialVars = new List<VarDeclaration>();
            int counter = 0;
            foreach (VarDeclaration var in lDeclerations){
                if (var.Name[0] == '_')
                    artificialVars.Add(var);
                else
                {
                    if (dTable.ContainsKey(var.Name))
                        throw new SyntaxErrorException("the token-" + var.Name + " has been defined more that once", new Token());
                    dTable.Add(var.Name,counter);
                    counter++;
                }
            }
            foreach (VarDeclaration var in artificialVars)
            {
                if (dTable.ContainsKey(var.Name))
                    throw new SyntaxErrorException("the token-" + var.Name + " has been defined more that once", new Token());
                dTable.Add(var.Name, counter);
                counter++;
            }
            return dTable;
        }


        public List<string> GenerateCode(List<LetStatement> lSimpleAssignments, List<VarDeclaration> lVars)
        {
            List<string> lAssembly = new List<string>();
            Dictionary<string, int> dSymbolTable = ComputeSymbolTable(lVars);
            foreach (LetStatement aSimple in lSimpleAssignments)
                lAssembly.AddRange(GenerateCode(aSimple, dSymbolTable));
            return lAssembly;
        }

        public List<LetStatement> SimplifyExpressions(LetStatement s, List<VarDeclaration> lVars)
        {
            //add here code to simply expressins in a statement. 
            //add var declarations for artificial variables.
            List<LetStatement> list = new List<LetStatement>();
            int counter = lVars.Count+1;
            if((s.Value is BinaryOperationExpression))
            {
                BinaryOperationExpression bExp = (BinaryOperationExpression)s.Value;
                if (!(bExp.Operand1 is BinaryOperationExpression) && !(bExp.Operand2 is BinaryOperationExpression))
                {
                    list.Add(s);
                }
                else if ((bExp.Operand1 is BinaryOperationExpression) && !(bExp.Operand2 is BinaryOperationExpression))
                {
                    lVars.Add(new VarDeclaration("int", "_" + counter));
                    LetStatement let = new LetStatement();
                    let.Variable = "_" + (counter);
                    let.Value = bExp.Operand1;
                    list.AddRange(SimplifyExpressions(let, lVars));

                    BinaryOperationExpression exp = new BinaryOperationExpression();
                    VariableExpression var = new VariableExpression();
                    var.Name = let.Variable;                    
                    exp.Operand2 = bExp.Operand2;
                    exp.Operand1 = var;
                    exp.Operator = bExp.Operator;
                    s.Value = exp;
                    list.Add(s);
                }
                else if(!(bExp.Operand1 is BinaryOperationExpression) && (bExp.Operand2 is BinaryOperationExpression))
                {
                    lVars.Add(new VarDeclaration("int", "_" + counter));
                    LetStatement let = new LetStatement();
                    let.Variable = "_" + (counter);
                    let.Value = bExp.Operand2;
                    list.AddRange(SimplifyExpressions(let, lVars));

                    BinaryOperationExpression exp = new BinaryOperationExpression();
                    VariableExpression var = new VariableExpression();
                    var.Name = let.Variable;
                    exp.Operand1 = bExp.Operand1;
                    exp.Operand2 = var;
                    exp.Operator = bExp.Operator;
                    s.Value = exp;
                    list.Add(s);
                }
                else
                {
                    lVars.Add(new VarDeclaration("int", "_" + counter));
                    LetStatement let = new LetStatement();
                    let.Variable = "_" + (counter);
                    let.Value = bExp.Operand1;
                    list.AddRange(SimplifyExpressions(let, lVars));

                    counter = lVars.Count + 1;

                    lVars.Add(new VarDeclaration("int", "_" + counter));
                    LetStatement let2 = new LetStatement();
                    let2.Variable = "_" + (counter);
                    let2.Value = bExp.Operand2;
                    list.AddRange(SimplifyExpressions(let2, lVars));

                    //counter = lVars.Count + 1;

                    BinaryOperationExpression exp = new BinaryOperationExpression();
                    VariableExpression var = new VariableExpression();
                    var.Name = let.Variable;
                    exp.Operand1 = var;
                    exp.Operator = bExp.Operator;
                    VariableExpression var2 = new VariableExpression();
                    var2.Name = let2.Variable;
                    exp.Operand2 = var2;
                    s.Value = exp;
                    //s.Variable = "_" + (counter+1);
                    list.Add(s);
                }

            }
            else
                list.Add(s);
            return list;

        }

        public List<LetStatement> SimplifyExpressions(List<LetStatement> ls, List<VarDeclaration> lVars)
        {
            List<LetStatement> lSimplified = new List<LetStatement>();
            foreach (LetStatement s in ls)
                lSimplified.AddRange(SimplifyExpressions(s, lVars));
            return lSimplified;
        }

 
        public LetStatement ParseStatement(List<Token> lTokens)
        {
            TokensStack sTokens = new TokensStack();
            for (int i = lTokens.Count - 1; i >= 0; i--)
                sTokens.Push(lTokens[i]);
            LetStatement s = new LetStatement();
            s.Parse(sTokens);
            return s;
        }

 
        public List<Token> Tokenize(string sLine, int iLine)
        {
            List<Token> lTokens = new List<Token>();
            char[] symbolsChar = new char[Token.Parentheses.Length + Token.Separators.Length + Token.Operators.Length + 2];
            Array.Copy(Token.Parentheses, symbolsChar, Token.Parentheses.Length);
            Array.Copy(Token.Separators, 0, symbolsChar, Token.Parentheses.Length, Token.Separators.Length);
            Array.Copy(Token.Operators, 0, symbolsChar, Token.Parentheses.Length + Token.Separators.Length, Token.Operators.Length);
            symbolsChar[symbolsChar.Length - 2] = ' ';
            symbolsChar[symbolsChar.Length - 1] = '\t';

            List<String> splitLine;
            if (!sLine.Contains("//"))
            {
                splitLine = Split(sLine, symbolsChar);
                int inLineIndex = 0;
                for (int j = 0; j < splitLine.Count; j++)
                {
                    Token t = GetInstance(splitLine[j]);
                    if (t != null)
                    {
                        t.Position = inLineIndex;
                        t.Line = iLine;
                        lTokens.Add(t);
                    }
                    inLineIndex += splitLine[j].Length;
                }
            }
            return lTokens;
        }

        public List<Token> Tokenize(List<string> lCodeLines)
        {
            List<Token> lTokens = new List<Token>();
            for (int i = 0; i < lCodeLines.Count; i++)
            {
                string sLine = lCodeLines[i];
                List<Token> lLineTokens = Tokenize(sLine, i);
                lTokens.AddRange(lLineTokens);
            }
            return lTokens;
        }

        private Token GetInstance(String str)
        {
            if (str.Length == 1)
            {
                if (Token.Operators.Contains(str[0]))
                    return new Operator(str[0], 0, 0);
                else if (Token.Parentheses.Contains(str[0]))
                    return new Parentheses(str[0], 0, 0);
                else if (Token.Separators.Contains(str[0]))
                    return new Separator(str[0], 0, 0);
            }
            else
            {
                if (Token.Statements.Contains(str))
                    return new Statement(str, 0, 0);
                else if (Token.VarTypes.Contains(str))
                    return new VarType(str, 0, 0);
                else if (Token.Constants.Contains(str))
                    return new Constant(str, 0, 0);
            }

            try
            {
                int.Parse(str);
                return new Number(str, 0, 0);
            }
            catch
            {
                if (char.IsDigit(str[0]))
                    throw new SyntaxErrorException("Illegal indentifier", new Token());
                else if (str == " " || str == "" || str == "\t")
                    return null;
                else if (char.IsLetter(str[0]))
                    return new Identifier(str, 0, 0);
            }
            throw new SyntaxErrorException("Ilegal syntax", new Token());
        }

        //Splits a string into a list of tokens, separated by delimiters
        private List<string> Split(string s, char[] aDelimiters)
        {
            List<string> lTokens = new List<string>();
            while (s.Length > 0)
            {
                string sToken = "";
                int i = 0;
                for (i = 0; i < s.Length; i++)
                {
                    if (aDelimiters.Contains(s[i]))
                    {
                        if (sToken.Length > 0)
                            lTokens.Add(sToken);
                        lTokens.Add(s[i] + "");
                        break;
                    }
                    else
                        sToken += s[i];
                }
                if (i == s.Length)
                {
                    lTokens.Add(sToken);
                    s = "";
                }
                else
                    s = s.Substring(i + 1);
            }
            return lTokens;
        }

    }
}
