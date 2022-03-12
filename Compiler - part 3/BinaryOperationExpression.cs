using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class BinaryOperationExpression : Expression
    {
        public string Operator { get;  set; }
        public Expression Operand1 { get;  set; }
        public Expression Operand2 { get;  set; }

        public override string ToString()
        {
            return "(" + Operand1 + " " + Operator + " " + Operand2 + ")";
        }

        public override void Parse(TokensStack sTokens) // (exp operator exp)
        {
            // (
            Token t = sTokens.Pop();
            if (t == null || t.ToString() != "(")
                throw new SyntaxErrorException("Expected ( got: ", t);
            // exp
            Expression e = Expression.Create(sTokens);
            e.Parse(sTokens);
            Operand1 = e;

            // operator
            Token op = sTokens.Pop();
            if (!(op is Operator))
                throw new SyntaxErrorException("Expected expression got: ", op);
            Operator = op.ToString();

            //exp
            e = Expression.Create(sTokens);
            e.Parse(sTokens);
            Operand2 = e;

            // )
            t = sTokens.Pop();
            if (t == null || t.ToString() != ")")
                throw new SyntaxErrorException("Expected ) got: ", t);
        }
    }
}
