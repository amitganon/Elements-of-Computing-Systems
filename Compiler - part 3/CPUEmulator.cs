using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    class CPUEmulator
    {
        public int A { get; private set; }
        public int D { get; private set; }
        public int PC { get; private set; }
        public Dictionary<string, int> Registers { get; private set; }

        public Dictionary<string, int> Labels { get; private set; }
        int VarBase, VarCount;
        public int[] M { get; private set; }

        public List<string> Code;



        public CPUEmulator()
        {
            M = new int[10000];

            Registers = new Dictionary<string, int>();
            Registers["SP"] = 0;
            Registers["LCL"] = 1;
            Registers["LOCAL"] = 1;
            Registers["ARG"] = 2;
            Registers["GLOBAL"] = 3;
            Registers["RESULT"] = 4;
            Registers["OPERAND1"] = 5;
            Registers["OPERAND2"] = 6;
            Registers["ADDRESS"] = 7;
            Registers["RETURN"] = 15;
            for (int i = 0; i < 16; i++)
                Registers["R" + i] = i;

            VarBase = 16;
            VarCount = 0;
        }

        public void LoadAssembly(string sFileName)
        {
            try
            {
                StreamReader sr = new StreamReader(sFileName);
                Code = new List<string>();
                while (!sr.EndOfStream)
                {
                    string sLine = sr.ReadLine().Trim();
                    if (sLine != "")
                        Code.Add(sLine);
                }
                sr.Close();
            }
            catch (Exception e)
            {

            }
        }

        public void Run(int cSteps, bool bBreakInfiniteLoops)
        {
            List<int> lLines = new List<int>();
            Labels = new Dictionary<string, int>();
            for (int i = 0; i < Code.Count; i++)
            {
                string sLine = Code[i].Trim().Replace(" ", "");
                if (sLine.StartsWith("("))
                    Labels[sLine.Substring(1, sLine.Length - 2)] = i + 1;
            }
            PC = 0;
            for (int i = 0; i < cSteps && PC < Code.Count; i++)
            {
                lLines.Add(PC);
                if(lLines.Count > 3 && bBreakInfiniteLoops)
                {
                    if (lLines[i] == lLines[i - 2] && lLines[i - 1] == lLines[i - 3])
                        break;
                }


                int iPreviousA = A;

                string sOriginalLine = Code[PC].Trim();
                string sLine = sOriginalLine.Replace(" ", "");
                if (sLine.Contains("//"))
                    sLine = sLine.Substring(0, sLine.IndexOf("/")).Trim();

                if (sLine == "")
                {
                    PC++;
                    continue;
                }

                string sMA = "N/A";
                if (A >= 0 && A < M.Length)
                    sMA = M[A] + "";
                Console.WriteLine(i + ") A=" + A + ", D=" + D + ", M=" + sMA + ", PC=" + PC + ", line=" + sOriginalLine);
                if (sLine == "")
                    continue;
                if (sLine.StartsWith("@"))
                {
                    string sValue = sLine.Substring(1);
                    int iValue = 0;
                    if (int.TryParse(sValue, out iValue))
                        A = iValue;
                    else
                    {
                        if (Registers.ContainsKey(sValue))
                            A = Registers[sValue];
                        else if (Labels.ContainsKey(sValue))
                            A = Labels[sValue];
                        else
                        {
                            Labels[sValue] = VarBase + VarCount;
                            VarCount++;
                            A = Labels[sValue];
                        }
                        //throw new Exception("Not spporting symbol " + sValue);

                    }
                    PC++;
                }
                else if (sLine.StartsWith("(") || sLine.StartsWith("//"))
                {
                    PC++;
                }
                else
                {

                    string sDest = "", sCompute = "", sJMP = "";
                    if (sLine.Contains("="))
                    {
                        sDest = sLine.Split('=')[0];
                        sLine = sLine.Substring(sDest.Length + 1);
                    }
                    if (sLine.Contains(";"))
                    {
                        sJMP = sLine.Split(';')[1];
                        sLine = sLine.Replace(";" + sJMP, "");
                    }
                    sCompute = sLine;
                    
                    int iCompute = Compute(sCompute);
                    if (sDest.Contains("D"))
                        D = iCompute;
                    if (sDest.Contains("M"))
                        M[iPreviousA] = iCompute;
                    if (sDest.Contains("A"))
                        A = iCompute;

                    if (sJMP != "" && Jump(sJMP, iCompute))
                        PC = iPreviousA;
                    else
                        PC++;
                }
            }
        }

        private int[] ToArray(string s)
        {
            int[] a = new int[s.Length];
            for (int i = 0; i < a.Length; i++)
            {
                if (s[i] == '0')
                    a[i] = 0;
                else
                    a[i] = 1;
            }
            return a;
        }

        private int ToNumber(string s)
        {
            int n = 0;
            for(int i = 0; i < s.Length; i++)
            {
                n = n * 2;
                if (s[i] == '1')
                    n = n  + 1;
            }
            return n;
        }

        private string GetCommand(Dictionary<string,int[]> d, int[] a)
        {
            foreach(KeyValuePair<string,int[]> p in d)
            {
                if (CompareArray(p.Value, a))
                    return p.Key;
            }
            return null;
        }
        private string GetDest(int[] a)
        {
            string sDest = "";
            if (a[2] == 1)
                sDest += "M";
            if (a[1] == 1)
                sDest += "D";
            if (a[0] == 1)
                sDest += "A";
            return sDest;
        }
        private bool CompareArray(int[] a1, int[] a2)
        {
            if (a1.Length != a2.Length)
                return false;
            for (int i = 0; i < a1.Length; i++)
                if (a1[i] != a2[i])
                    return false;
            return true;
        }

        private bool ValidLine(string sLine)
        {
            if (sLine.Length != 16)
                return false;
            foreach (char c in sLine)
                if (c != '0' && c != '1')
                    return false;
            return true;
        }

        public void Reset()
        {
            A = 0;
            D = 0;
            PC = 0;
            M = new int[10000];
        }

        private bool Jump(string sJMP, int iCompute)
        {
            if (sJMP == "JEQ" && iCompute == 0)
                return true;
            if (sJMP == "JGT" && iCompute > 0)
                return true;
            if (sJMP == "JLT" && iCompute < 0)
                return true;
            if (sJMP == "JGE" && iCompute >= 0)
                return true;
            if (sJMP == "JLE" && iCompute <= 0)
                return true;
            if (sJMP == "JNE" && iCompute != 0)
                return true;
            if (sJMP == "JMP")
                return true;
            return false;
        }

        private int Compute(string sCompute)
        {
            string sOperand1 = sCompute[0] + "";
            string sOperand2 = "";
            string sOperator = "";

            if (sCompute.Length == 2)
            {
                sOperand1 = sCompute[1] + "";
            }
            if (sCompute.Length > 2)
            {
                sOperand2 = sCompute[2] + "";
                sOperator = sCompute[1] + "";
            }
            int iOperand1 = 0;
            if (sOperand1 == "A")
                iOperand1 = A;
            if (sOperand1 == "D")
                iOperand1 = D;
            if (sOperand1 == "M")
                iOperand1 = M[A];
            if (sOperand1 == "1")
                iOperand1 = 1;
            if (sOperand1 == "0")
                iOperand1 = 0;

            if (sCompute.Length == 1)
                return iOperand1;

            int iOperand2 = 0;
            if (sOperand2 == "A")
                iOperand2 = A;
            if (sOperand2 == "D")
                iOperand2 = D;
            if (sOperand2 == "M")
                iOperand2 = M[A];
            if (sOperand2 == "1")
                iOperand2 = 1;
            if (sOperand2 == "0")
                iOperand2 = 0;


            if (sCompute.Length == 2 && sCompute[0] == '-')
                return -1 * iOperand1;

            if (sOperator == "+")
                return iOperand1 + iOperand2;
            if (sOperator == "-")
                return iOperand1 - iOperand2;
            if (sOperator == "&")
                return iOperand1 & iOperand2;
            if (sOperator == "|")
                return iOperand1 | iOperand2;

            return 0;
        }

        public void Set(string sRegister, int iValue)
        {
            M[Registers[sRegister]] = iValue;
        }



        public void Compute(List<LetStatement> lStatements, Dictionary<string, int> dValues)
        {
            foreach (LetStatement ls in lStatements)
            {
                int iValue = ComputeExpression(ls.Value, dValues);
                dValues[ls.Variable] = iValue;
            }

        }

        private int ComputeExpression(Expression exp, Dictionary<string, int> dValues)
        {
            if (exp is NumericExpression)
            {
                return ((NumericExpression)exp).Value;
            }
            if (exp is VariableExpression)
            {
                return dValues[((VariableExpression)exp).Name];
            }
            if (exp is BinaryOperationExpression)
            {
                BinaryOperationExpression be = (BinaryOperationExpression)exp;
                int iOperand1 = ComputeExpression(be.Operand1, dValues);
                int iOperand2 = ComputeExpression(be.Operand2, dValues);
                if (be.Operator == "+")
                    return iOperand1 + iOperand2;
                if (be.Operator == "-")
                    return iOperand1 - iOperand2;
                if (be.Operator == "&")
                    return iOperand1 & iOperand2;
                if (be.Operator == "|")
                    return iOperand1 | iOperand2;
                if (be.Operator == "*")
                    return iOperand1 * iOperand2;
                if (be.Operator == "/")
                    return iOperand1 / iOperand2;
            }
            return 0;
        }


    }
}
