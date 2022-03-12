using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembler a = new Assembler();
            //to run tests, call the "TranslateAssemblyFile" function like this:
            string sourceFileLocation = @"Test.asm";
            string destFileLocation = @"Test.hack";
            a.TranslateAssemblyFile(sourceFileLocation, destFileLocation);
            //a.TranslateAssemblyFile(@"Add.asm", @"Add.hack");
            //You need to be able to run two translations one after the other
            //a.TranslateAssemblyFile(@"Max.asm", @"Max.hack");
        }
    }
}
