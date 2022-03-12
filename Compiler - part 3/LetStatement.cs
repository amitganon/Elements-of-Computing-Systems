using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class LetStatement : StatetmentBase
    {
        public string Variable { get; set; }
        public Expression Value { get; set; }

        public override string ToString()
        {
            return "let " + Variable + " = " + Value + ";";
        }

        public override void Parse(TokensStack sTokens) // let ID = Expression;
        {
            //let
            Token t = sTokens.Pop();            
            if (!(t is Statement) || ((Statement)t).Name != "let")
                throw new SyntaxErrorException("Expected let got: ",t);

            //ID
            t = sTokens.Pop();
            if(!(t is Identifier))
                throw new SyntaxErrorException("Expected Identifier got: ", t);
            Variable = ((Identifier)t).Name;

            // = 
            t = sTokens.Pop();
            if (!(t is Operator) || ((Operator)t).Name != '=')
                throw new SyntaxErrorException("Expected = got: ", t);

            // Expression
            Expression e = Expression.Create(sTokens);
            e.Parse(sTokens);
            Value = e;

            // ;
            t = sTokens.Pop();
            if (!(t is Separator) || ((Separator)t).Name != ';')
                throw new SyntaxErrorException("Expected ; got: ", t);

        }

    }
}
