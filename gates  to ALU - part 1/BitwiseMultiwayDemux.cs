using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    class BitwiseMultiwayDemux : Gate, Component
    {
        public int Size { get; private set; }
        public int ControlBits { get; private set; }
        public WireSet Input { get; private set; }
        public WireSet Control { get; private set; }
        public WireSet[] Outputs { get; private set; }

        public BitwiseMultiwayDemux(int iSize, int cControlBits)
        {
            Size = iSize;
            Input = new WireSet(Size);
            Control = new WireSet(cControlBits);
            Outputs = new WireSet[(int)Math.Pow(2, cControlBits)];

            for (int i = 0; i < Outputs.Length; i++)
            {
                Outputs[i] = new WireSet(Size);
            }
            for (int i = 0; i < Size; i++)
                Input[i].ConnectOutput(this);
            for (int i = 0; i < cControlBits; i++)
                Control[i].ConnectOutput(this);
        }


        public void ConnectInput(WireSet wsInput)
        {
            Input.ConnectInput(wsInput);
        }
        public void ConnectControl(WireSet wsControl)
        {
            Control.ConnectInput(wsControl);
        }

        public override bool TestGate()
        {
            Random rnd = new Random();
            int iValue = rnd.Next((int)Math.Pow(2, Size));
            Input.SetValue(iValue);

            for (int i = 0; i < Outputs.Length; i++)
            {
                Control.SetValue(i);

                for (int j = 0; j < Outputs.Length; j++)
                {
                    if (j == i)
                    {
                        if (Outputs[j].GetValue() != iValue)
                            return false;
                    }
                    else if (Outputs[j].GetValue() != 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        #region Component Members

        public void Compute()
        {
            for (int i = 0; i < Outputs.Length; i++)
            {
                if (i == Control.GetValue())
                    Outputs[i].SetValue(Input.GetValue());
                else
                    Outputs[i].SetValue(0);
            }
        }

        #endregion
    }
}
