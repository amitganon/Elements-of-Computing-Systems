using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assembler
{
    public class AssemblerException : Exception
    {
        public int LineNumber { get; private set; }
        public string Line { get; private set; }
        public string Info { get; private set; }
        public AssemblerException(int iLineNumber, string sLine, string sInfo)
        {
            LineNumber = iLineNumber;
            Line = sLine;
            Info = sInfo;
        }

        public override string ToString()
        {
            return "Assmbely error detected at line: " + LineNumber + ", " + Line + ", " + Info;
        }
    }
}
