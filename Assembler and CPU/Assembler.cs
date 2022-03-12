using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{
    public class Assembler
    {
        private const int WORD_SIZE = 16;

        private Dictionary<string, int[]> m_dControl, m_dJmp, m_dDest; //these dictionaries map command mnemonics to machine code - they are initialized at the bottom of the class

        //more data structures here (symbol map, ...)
        private Dictionary <String,int> symbolMap;
        int symbolIndex;
        public Assembler()
        {
            InitCommandDictionaries();
        }

        //this method is called from the outside to run the assembler translation
        public void TranslateAssemblyFile(string sInputAssemblyFile, string sOutputMachineCodeFile)
        {
            symbolMap = new Dictionary<string, int>();
            symbolIndex = 16;
            //read the raw input, including comments, errors, ...
            StreamReader sr = new StreamReader(sInputAssemblyFile);
            List<string> lLines = new List<string>();
            while (!sr.EndOfStream)
            {
                lLines.Add(sr.ReadLine());
            }
            sr.Close();
            //translate to machine code
            List<string> lTranslated = TranslateAssemblyFile(lLines);
            //write the output to the machine code file
            StreamWriter sw = new StreamWriter(sOutputMachineCodeFile);
            foreach (string sLine in lTranslated)
                sw.WriteLine(sLine);
            sw.Close();
        }

        //translate assembly into machine code
        private List<string> TranslateAssemblyFile(List<string> lLines)
        {
            //implementation order:
            //first, implement "TranslateAssemblyToMachineCode", and check if the examples "Add", "MaxL" are translated correctly.
            //next, implement "CreateSymbolTable", and modify the method "TranslateAssemblyToMachineCode" so it will support symbols (translating symbols to numbers). check this on the examples that don't contain macros
            //the last thing you need to do, is to implement "ExpendMacro", and test it on the example: "SquareMacro.asm".
            //init data structures here 

            //expand the macros
            List<string> lAfterMacroExpansion = ExpendMacros(lLines);

            //first pass - create symbol table and remove lable lines
            CreateSymbolTable(lAfterMacroExpansion);

            //second pass - replace symbols with numbers, and translate to machine code
            List<string> lAfterTranslation = TranslateAssemblyToMachineCode(lAfterMacroExpansion);
            return lAfterTranslation;
        }

        
        //first pass - replace all macros with real assembly
        private List<string> ExpendMacros(List<string> lLines)
        {
            //You do not need to change this function, you only need to implement the "ExapndMacro" method (that gets a single line == string)
            List<string> lAfterExpansion = new List<string>();
            for (int i = 0; i < lLines.Count; i++)
            {
                //remove all redudant characters
                string sLine = CleanWhiteSpacesAndComments(lLines[i]);
                if (sLine == "")
                    continue;
                //if the line contains a macro, expand it, otherwise the line remains the same
                List<string> lExpanded = ExapndMacro(sLine);
                //we may get multiple lines from a macro expansion
                foreach (string sExpanded in lExpanded)
                {
                    lAfterExpansion.Add(sExpanded);
                }
            }
            return lAfterExpansion;
        }

        //expand a single macro line
        private List<string> ExapndMacro(string sLine)
        {
            List<string> lExpanded = new List<string>();

            if (IsCCommand(sLine))
            {
                string sDest, sCompute, sJmp;
                GetCommandParts(sLine, out sDest, out sCompute, out sJmp);
                //your code here - check for indirect addessing and for jmp shortcuts
                Char[] sDestChars = sDest.ToCharArray();
                Char[] sComputeChars = sCompute.ToCharArray();
                Char[] sJmpChars = sJmp.ToCharArray();
                if (sDest.Length != 0)
                {
                    if(sDest=="A" | sDest == "M" | sDest == "D")
                    {
                        if(sCompute.Length != 0)
                        {
                            if(sCompute.Substring(0,1) != "A" & sCompute.Substring(0, 1) != "M" & sCompute.Substring(0, 1) != "D")
                            {
                                if (!(sComputeChars[0] == '1' | sComputeChars[0] == '0'| sComputeChars[0] == '-'))
                                {
                                    lExpanded.Add("@" + sCompute);
                                    lExpanded.Add(sDest + "=" + "M");
                                }
                            }
                        }
                        else
                        {
                            if ((sDestChars[1] == '+' & sDestChars[2]== '+') | (sDestChars[1] == '-' & sDestChars[2] == '-'))
                            {
                                lExpanded.Add(sDest+"="+ sDest+"+"+"1");
                            }
                        }
                    }
                    else
                    {
                        if (sCompute.Length != 0)
                        {
                            if(isNumber(sCompute))
                            {
                                lExpanded.Add("@" + sCompute);
                                lExpanded.Add("D" + "=" + "A");
                                lExpanded.Add("@" + sDest);
                                lExpanded.Add("M" + "=" + "D");
                            }
                            else if (sCompute.Substring(0, 1) != "A" & sCompute.Substring(0, 1) != "M" & sCompute.Substring(0, 1) != "D")
                            {
                                lExpanded.Add("@" + sCompute);
                                lExpanded.Add("D" + "="+"M");
                                lExpanded.Add("@" + sDest);
                                lExpanded.Add("M" + "=" + "D");
                            }
                            else
                            {
                                if (sComputeChars[0] == 'A')
                                {
                                    lExpanded.Add("D" + "=" + "M");
                                    lExpanded.Add("@" + sDest);
                                    lExpanded.Add("M" + "=" + "D");
                                }
                                else if (sComputeChars[0] == 'D')
                                {
                                    lExpanded.Add("@" + sDest);
                                    lExpanded.Add("M" + "=" + "D");
                                }
                                else
                                {
                                    //do nothing
                                }
                            }
                        }
                    }
                }
                else
                {
                    if ((sComputeChars[0] == '0' | sComputeChars[0] == 'D') && m_dJmp.ContainsKey(sJmp.Substring(0,3)) && sJmp.Contains(':'))
                    {
                        lExpanded.Add("@" + sJmp.Substring(4));
                        if (sComputeChars[0] == 'D' | sComputeChars[0] == 'A' | sComputeChars[0] == 'M')
                            lExpanded.Add(sComputeChars[0] + ";" + sJmp.Substring(0, 3));
                        else if (isNumber(sComputeChars[0].ToString()))
                            lExpanded.Add(sComputeChars[0] + ";" + sJmp.Substring(0, 3));
                        else
                            throw new AssemblerException(-1,sLine,"illegal argument in comp");
                    }
                    else if (countChar(sCompute, '-') == 2 | countChar(sCompute, '+') == 2)
                    {
                        lExpanded.Add("@" + sCompute.Substring(0, sCompute.Length - 2));
                        lExpanded.Add("M" + "=" + "M" + sComputeChars[sComputeChars.Length - 1] + "1");
                    }
                }

                //read the word file to see all the macros you need to support
            }
            if (lExpanded.Count == 0)
                lExpanded.Add(sLine);
            return lExpanded;
        }

        private int countChar(string sDest, char v)
        {
            Char[] chars = sDest.ToCharArray();
            int count = 0;
            foreach(char c in chars)
            {
                if (c==v)
                {
                    count ++;
                }
            }
            return count;
        }

        //second pass - record all symbols - labels and variables
        private void CreateSymbolTable(List<string> lLines)
        {
            int codeLineIndex = 0;
            string sLine = "";
            for (int i = 0; i < lLines.Count; i++)
            {
                sLine = lLines[i];
                char[] charArr = sLine.ToCharArray();
                if (IsLabelLine(sLine))
                {
                    //Console.WriteLine(sLine);
                    sLine = sLine.Substring(1, sLine.Length - 2);
                    //record label in symbol table
                    //do not add the label line to the result
                    //Console.WriteLine(sLine);
                    if (symbolMap.ContainsKey(sLine) && symbolMap[sLine] != -1)
                        throw new AssemblerException(i, sLine, "symbol is alredy exist in line:"+ symbolMap[sLine]);
                    else if (symbolMap.ContainsKey(sLine))
                        symbolMap[sLine] = codeLineIndex;
                    else{
                        checkSymbole(sLine,i);
                        symbolMap.Add(sLine, codeLineIndex);                        
                    }

                }
                else if (IsACommand(sLine))
                {
                    //may contain a variable - if so, record it to the symbol table (if it doesn't exist there yet...)
                    sLine = sLine.Substring(1);
                    try
                    {
                        int.Parse(sLine.Substring(0));
                    }
                    catch 
                    {
                        checkSymbole(sLine,i);
                        if (!symbolMap.ContainsKey(sLine))                     
                            symbolMap.Add(sLine, -1);
                    }
                    codeLineIndex++;
                }
                else if (IsCCommand(sLine))
                {
                    //do nothing here
                    codeLineIndex++;
                }
                else
                    throw new FormatException("Cannot parse line " + i + ": " + lLines[i]);
            }
            int counterForVar = 16;
            ICollection <String> keys = symbolMap.Keys;
            for(int i =0; i< keys.Count; i++)
            {
                if (symbolMap[keys.ElementAt(i)] == -1)
                {
                    symbolMap[keys.ElementAt(i)] = counterForVar;
                    counterForVar++;
                }
            }
        }

        private void checkSymbole(string sLine, int lineNumber)
        {
            Char[] charArr = sLine.ToCharArray();
            if (Char.IsDigit(charArr[0]))
                throw new AssemblerException(lineNumber, sLine, "illegal symbol");
            for (int j = 1; j < charArr.Length; j++)
            {
                if (!(Char.IsLetter(charArr[j]) | Char.IsDigit(charArr[j]) | charArr[j] == ' ' | charArr[j] == '_'))
                    throw new AssemblerException(lineNumber, sLine, "illegal symbol");
            }
        }

        //third pass - translate lines into machine code, replacing symbols with numbers
        private List<string> TranslateAssemblyToMachineCode(List<string> lLines)
        {
            string sLine = "";
            List<string> lAfterPass = new List<string>();
            for (int i = 0; i < lLines.Count; i++)
            {
                sLine = lLines[i];
                if (IsLabelLine(sLine)) { }
                else if (IsACommand(sLine))
                {
                    if (!isNumber(sLine.Substring(1)))
                    {
                        int address = symbolMap[sLine.Substring(1)];
                        sLine = ToBinary(address);
                    }
                    else
                    {
                        sLine = ToBinary(int.Parse(sLine.Substring(1)));
                    }
                    if (sLine.Length != 16)
                    {
                        throw new AssemblerException(i, sLine, "binary address not in length - 16");
                    }
                    lAfterPass.Add(sLine);
                }
                else if (IsCCommand(sLine))
                {
                    string sDest, sControl, sJmp;
                    GetCommandParts(sLine, out sDest, out sControl, out sJmp);
                    //translate an C command into a sequence of bits
                    //take a look at the dictionaries m_dControl, m_dJmp, and where they are initialized (InitCommandDictionaries), to understand how to you them here
                    if (!m_dDest.ContainsKey(sDest))
                        throw new AssemblerException(i, sLine, "problem in C comand at dest");
                    else if (!m_dControl.ContainsKey(sControl))
                        throw new AssemblerException(i, sLine, "problem in C comand at control");
                    else if (!m_dJmp.ContainsKey(sJmp))
                        throw new AssemblerException(i, sLine, "problem in C comand at jmp");
                    else if(sDest.Length == 0) 
                    {
                        if(sControl.Length == 0 | sJmp.Length == 0)
                                throw new AssemblerException(i, sLine, "problem in C comand at jmp or control");
                    }
                    else 
                    {
                        if (sControl.Length == 0 | sJmp.Length != 0)
                            throw new AssemblerException(i, sLine, "problem in C comand at jmp or control");
                    }

                    lAfterPass.Add("1000"+ ToString(m_dControl[sControl]) + ToString(m_dDest[sDest]) + ToString(m_dJmp[sJmp]));
                }
                else
                    throw new FormatException("Cannot parse line " + i + ": " + lLines[i]);
            }
            return lAfterPass;
        }

        private bool isNumber(String str)
        {
            bool isNumber = true;
            try
            {
                int.Parse(str);
            }
            catch
            {
                isNumber = false;
            }
            return isNumber;
        }

        //helper functions for translating numbers or bits into strings
        private string ToString(int[] aBits)
        {
            string sBinary = "";
            for (int i = (aBits.Length-1); i >= 0; i--)
                sBinary += aBits[i];
            return sBinary;
        }

        private string ToBinary(int x)
        {
            string sBinary = "";
            for (int i = 0; i < WORD_SIZE; i++)
            {
                sBinary = (x % 2) + sBinary;
                x = x / 2;
            }
            return sBinary;
        }


        //helper function for splitting the various fields of a C command
        private void GetCommandParts(string sLine, out string sDest, out string sControl, out string sJmp)
        {
            if (sLine.Contains('='))
            {
                int idx = sLine.IndexOf('=');
                sDest = sLine.Substring(0, idx);
                sLine = sLine.Substring(idx + 1);
            }
            else
                sDest = "";
            if (sLine.Contains(';'))
            {
                int idx = sLine.IndexOf(';');
                sControl = sLine.Substring(0, idx);
                sJmp = sLine.Substring(idx + 1);

            }
            else
            {
                sControl = sLine;
                sJmp = "";
            }
        }

        private bool IsCCommand(string sLine)
        {
            return !IsLabelLine(sLine) && sLine[0] != '@';
        }

        private bool IsACommand(string sLine)
        {
            return sLine[0] == '@';
        }

        private bool IsLabelLine(string sLine)
        {
            if (sLine.StartsWith("(") && sLine.EndsWith(")"))
                return true;
            return false;
        }

        private string CleanWhiteSpacesAndComments(string sDirty)
        {
            string sClean = "";
            for (int i = 0 ; i < sDirty.Length ; i++)
            {
                char c = sDirty[i];
                if (c == '/' && i < sDirty.Length - 1 && sDirty[i + 1] == '/') // this is a comment
                    return sClean;
                if (c > ' ' && c <= '~')//ignore white spaces
                    sClean += c;
            }
            return sClean;
        }

        public int[] GetArray(params int[] l)
        {
            int[] a = new int[l.Length];
            for (int i = 0; i < l.Length; i++)
                a[l.Length - i - 1] = l[i];
            return a;
        }
        private void InitCommandDictionaries()
        {
            m_dControl = new Dictionary<string, int[]>();


            m_dControl["0"] = GetArray(0, 0, 0, 0, 0, 0);
            m_dControl["1"] = GetArray(0, 0, 0, 0, 0, 1);
            m_dControl["D"] = GetArray(0, 0, 0, 0, 1, 0);
            m_dControl["A"] = GetArray(0, 0, 0, 0, 1, 1);
            m_dControl["!D"] = GetArray(0, 0, 0, 1, 0, 0);
            m_dControl["!A"] = GetArray(0, 0, 0, 1, 0, 1);
            m_dControl["-D"] = GetArray(0, 0, 0, 1, 1, 0);
            m_dControl["-A"] = GetArray(0, 0, 0, 1, 1, 1);
            m_dControl["D+1"] = GetArray(0, 0, 1, 0, 0, 0);
            m_dControl["A+1"] = GetArray(0, 0, 1, 0, 0, 1);
            m_dControl["D-1"] = GetArray(0, 0, 1, 0, 1, 0);
            m_dControl["A-1"] = GetArray(0, 0, 1, 0, 1, 1);
            m_dControl["A+D"] = GetArray(0, 0, 1, 1, 0, 0);
            m_dControl["D+A"] = GetArray(0, 0, 1, 1, 0, 0);
            m_dControl["D-A"] = GetArray(0, 0, 1, 1, 0, 1);
            m_dControl["A-D"] = GetArray(0, 0, 1, 1, 1, 0);
            m_dControl["A^D"] = GetArray(0, 0, 1, 1, 1, 1);
            m_dControl["A&D"] = GetArray(0, 1, 0, 0, 0, 0);
            m_dControl["AvD"] = GetArray(0, 1, 0, 0, 0, 1);
            m_dControl["A|D"] = GetArray(0, 1, 0, 0, 1, 0);

            m_dControl["M"] = GetArray(1, 0, 0, 0, 1, 1);
            m_dControl["!M"] = GetArray(1, 0, 0, 1, 0, 1);
            m_dControl["-M"] = GetArray(1, 0, 0, 1, 1, 1);
            m_dControl["M+1"] = GetArray(1, 0, 1, 0, 0, 1);
            m_dControl["M-1"] = GetArray(1, 0, 1, 0, 1, 1);
            m_dControl["M+D"] = GetArray(1, 0, 1, 1, 0, 0);
            m_dControl["D+M"] = GetArray(1, 0, 1, 1, 0, 0);
            m_dControl["D-M"] = GetArray(1, 0, 1, 1, 0, 1);
            m_dControl["M-D"] = GetArray(1, 0, 1, 1, 1, 0);
            m_dControl["M^D"] = GetArray(1, 0, 1, 1, 1, 1);
            m_dControl["M&D"] = GetArray(1, 1, 0, 0, 0, 0);
            m_dControl["MvD"] = GetArray(1, 1, 0, 0, 0, 1);
            m_dControl["M|D"] = GetArray(1, 1, 0, 0, 1, 0);



            m_dDest = new Dictionary<string, int[]>();
            m_dDest[""] = GetArray(0, 0, 0);
            m_dDest["M"] = GetArray(0, 0, 1);
            m_dDest["D"] = GetArray(0, 1, 0);
            m_dDest["A"] = GetArray(1, 0, 0);
            m_dDest["DM"] = GetArray(0, 1, 1);
            m_dDest["AM"] = GetArray(1, 0, 1);
            m_dDest["AD"] = GetArray(1, 1, 0);
            m_dDest["ADM"] = GetArray(1, 1, 1);


            m_dJmp = new Dictionary<string, int[]>();

            m_dJmp[""] = GetArray(0, 0, 0);
            m_dJmp["JGT"] = GetArray(0, 0, 1);
            m_dJmp["JEQ"] = GetArray(0, 1, 0);
            m_dJmp["JGE"] = GetArray(0, 1, 1);
            m_dJmp["JLT"] = GetArray(1, 0, 0);
            m_dJmp["JNE"] = GetArray(1, 0, 1);
            m_dJmp["JLE"] = GetArray(1, 1, 0);
            m_dJmp["JMP"] = GetArray(1, 1, 1);
        }
    }
}
