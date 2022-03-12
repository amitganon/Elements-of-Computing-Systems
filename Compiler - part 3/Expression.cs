using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public abstract class Expression : JackProgramElement
    {
        public static Expression Create(TokensStack sTokens)
        {
            if(sTokens.Count < 2)
                throw new SyntaxErrorException("Expected expression", null);

            Token tFirst = sTokens.Peek(0);
            Token tSecond = sTokens.Peek(1);
            Token tThird = null;
            if( sTokens.Count > 2)
                tThird = sTokens.Peek(2);

            if (tFirst is Identifier)
                return new VariableExpression();
            if (tFirst is Number)
                return new NumericExpression();
            if ((tFirst is Parentheses) && (((Parentheses)tFirst).Name  == '('))
                return new BinaryOperationExpression();
            if ((tFirst is Identifier) && (tSecond is Parentheses) && (((Parentheses)tSecond).Name == '(') && !(tThird is Operator))
                return new FunctionCallExpression();
            //if (tFirst.Type == Token.TokenType.ID && tSecond.Name == "[")
            //    return new ArrayExpression();
            throw new SyntaxErrorException("Expected expression", tFirst);
            return null;
        }
    }
}
