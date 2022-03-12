using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //Multibit gates take as input k bits, and compute a function over all bits - z=f(x_0,x_1,...,x_k)
    class MultiBitAndGate : MultiBitGate
    {
        //your code here
        private AndGate [] arrAnd1;

        public MultiBitAndGate(int iInputCount)
            : base(iInputCount)
        {
            //your code here
            arrAnd1 = new AndGate [iInputCount-1];
            Output = new Wire();
            arrAnd1[0] = new AndGate();
            arrAnd1[0].ConnectInput1(m_wsInput[0]);
            arrAnd1[0].ConnectInput2(m_wsInput[1]);
            for (int i = 2; i<iInputCount; i++)
            {
                arrAnd1[i-1]=new AndGate();
                arrAnd1[i - 1].ConnectInput1(arrAnd1[i - 2].Output);
                arrAnd1[i - 1].ConnectInput2(m_wsInput[i]);
            }
            Output.ConnectInput(arrAnd1[iInputCount - 2].Output);
        }


        public override bool TestGate()
        {
            for (int i = 0; i < m_wsInput.Size; i++)
            {
                m_wsInput[0].Value = 0;
            }

            for (int i = 0; i < m_wsInput.Size; i++)
            {
                m_wsInput[i].Value = 1;
                if (Output.Value != 0)
                {
                    //Console.WriteLine(ToString());
                    return false;
                }
                m_wsInput[i].Value = 0;
            }

            if (m_wsInput.Size > 2)
            {
                m_wsInput[0].Value = 1;
                m_wsInput[m_wsInput.Size - 1].Value = 1;
                if (Output.Value != 0)
                {
                    //Console.WriteLine(ToString());
                    return false;
                }
                m_wsInput[0].Value = 0;
                m_wsInput[m_wsInput.Size - 1].Value = 0;

                m_wsInput[1].Value = 1;
                m_wsInput[m_wsInput.Size - 1].Value = 1;
                if (Output.Value != 0)
                {
                    //Console.WriteLine(ToString());
                    return false;
                }
            }

            for (int i = 0; i < m_wsInput.Size; i++)
            {
                m_wsInput[i].Value = 1;
            }
            if (Output.Value != 1)
            {
                //Console.WriteLine(ToString());
                return false;
            }

            return true;
        }
    }
}
