using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class IfStatement : StatetmentBase
    {
        public Expression Term { get; private set; }
        public List<StatetmentBase> DoIfTrue { get; private set; }
        public List<StatetmentBase> DoIfFalse { get; private set; }

        public IfStatement()
        {
            DoIfTrue = new List<StatetmentBase>();
            DoIfFalse = new List<StatetmentBase>();
        }

        public override void Parse(TokensStack sTokens) // if (Expression){ statment* }
        {
            // if
            Token t = sTokens.Pop();
            if (!(t is Statement) || ((Statement)t).Name != "if")
                throw new SyntaxErrorException("Expected if Statement got: ", t);
            
            // (
            t = sTokens.Pop();
            if(!(t is Parentheses) || ((Parentheses)t).Name != '(')
                throw new SyntaxErrorException("Expected ( got: ", t);

            // Expression
            Expression e = Expression.Create(sTokens);
            e.Parse(sTokens);
            Term = e;

            // )
            t = sTokens.Pop();
            if (!(t is Parentheses) || ((Parentheses)t).Name != ')')
                throw new SyntaxErrorException("Expected ) got: ", t);
            //{
            t = sTokens.Pop();
            if (!(t is Parentheses) || ((Parentheses)t).Name != '{')
                throw new SyntaxErrorException("Expected { got: ", t);

            // statment*
            while (sTokens.Count > 0 && !(sTokens.Peek() is Parentheses))
            {
                StatetmentBase s = StatetmentBase.Create(sTokens.Peek());
                s.Parse(sTokens);
                DoIfTrue.Add(s);
            }

            //}
            t = sTokens.Pop();
            if (!(t is Parentheses) || ((Parentheses)t).Name != '}')
                throw new SyntaxErrorException("Expected } got: ", t);

            // else
            if ((sTokens.Peek() is Statement) && (((Statement)sTokens.Peek()).Name == "else"))
            {
                t = sTokens.Pop();

                //{
                t = sTokens.Pop();
                if (!(t is Parentheses) || ((Parentheses)t).Name != '{')
                    throw new SyntaxErrorException("Expected { got: ", t);

                // statment*
                while (sTokens.Count > 0 && !(sTokens.Peek() is Parentheses))
                {
                    StatetmentBase s = StatetmentBase.Create(sTokens.Peek());
                    s.Parse(sTokens);
                    DoIfFalse.Add(s);
                }

                //}
                t = sTokens.Pop();
                if (!(t is Parentheses) || ((Parentheses)t).Name != '}')
                    throw new SyntaxErrorException("Expected } got: ", t);
            }
        }

        public override string ToString()
        {
            string sIf = "if(" + Term + "){\n";
            foreach (StatetmentBase s in DoIfTrue)
                sIf += "\t\t\t" + s + "\n";
            sIf += "\t\t}";
            if (DoIfFalse.Count > 0)
            {
                sIf += "else{";
                foreach (StatetmentBase s in DoIfFalse)
                    sIf += "\t\t\t" + s + "\n";
                sIf += "\t\t}";
            }
            return sIf;
        }

    }
}
