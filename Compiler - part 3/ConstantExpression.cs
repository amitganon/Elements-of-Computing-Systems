using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    class ConstantExpression : Expression
    {
        public string Constant;

        public override void Parse(TokensStack sTokens)
        {
            Token tConst = sTokens.Pop();
            if (tConst is Keyword)
                Constant = ((Keyword)tConst).Name;
            else
            {
                throw new SyntaxErrorException("Expected Keyword got: ", tConst);
            }
        }

        public override string ToString()
        {
            return Constant;
        }
    }
}
