using System;
using System.Collections.Generic;

namespace SimpleCompiler
{
    public class FunctionCallExpression : Expression
    {
        public string FunctionName { get; private set; }
        public List<Expression> Args { get; private set; }

        public FunctionCallExpression()
        {
            Args = new List<Expression>();
        }

        public override void Parse(TokensStack sTokens) // ID (Expression LIst)
        {
            //ID
            Token t = sTokens.Pop();
            if (!(t is Identifier))
                throw new SyntaxErrorException("Excpect ID got: ",t);
            FunctionName = ((Identifier)t).Name;

            //(
            t = sTokens.Pop();
            if (!(t is Parentheses) || ((Parentheses)t).Name != '(')
                throw new SyntaxErrorException("Excpect ID got: ", t);

            //Expression LIst
            int count = 0;
            while (sTokens.Count>0 && !((sTokens.Peek() is Parentheses) && (((Parentheses)sTokens.Peek()).Name ==')')))
            {
                if (count > 0 && sTokens.Peek() is Separator && ((Separator)sTokens.Peek()).Name==',')
                {
                    sTokens.Pop();
                }
                else if(count > 0)
                    throw new SyntaxErrorException("Expected , got: ", t);
                Expression e = Expression.Create(sTokens);
                e.Parse(sTokens);
                Args.Add(e);
                count++;
            }

            //)
            t = sTokens.Pop();
            if (!(t is Parentheses) || ((Parentheses)t).Name != ')')
                throw new SyntaxErrorException("Expected ) got: ",t);

        }

        public override string ToString()
        {
            string sFunction = FunctionName + "(";
            for (int i = 0; i < Args.Count - 1; i++)
                sFunction += Args[i] + ",";
            if (Args.Count > 0)
                sFunction += Args[Args.Count - 1];
            sFunction += ")";
            return sFunction;
        }
    }
}