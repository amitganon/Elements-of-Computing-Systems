using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public abstract class JackProgramElement
    {
        public abstract void Parse(TokensStack sTokens);
    }
}
