using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class represents an n bit register that can maintain an n bit number
    class MultiBitRegister : Gate
    {
        public WireSet Input { get; private set; }
        public WireSet Output { get; private set; }
        //A bit setting the register operation to read or write
        public Wire Load { get; private set; }

        //Word size - number of bits in the register
        public int Size { get; private set; }


        public MultiBitRegister(int iSize)
        {
            Size = iSize;
            Input = new WireSet(Size);
            Output = new WireSet(Size);
            Load = new Wire();
            //your code here
            SingleBitRegister[] arrReg = new SingleBitRegister[Size];
            for (int i=0; i<Size; i++)
            {
                SingleBitRegister reg = new SingleBitRegister();
                reg.ConnectInput(Input[i]);
                reg.ConnectLoad(Load);
                Output[i].ConnectInput(reg.Output);
            }
        }

        public void ConnectInput(WireSet wsInput)
        {
            Input.ConnectInput(wsInput);
        }

        //public void ConnectLoad(Wire wLoad)
        //{
        //    Load.ConnectInput(wLoad);
        //}


        public override string ToString()
        {
            return Output.ToString();
        }


        public override bool TestGate()
        {
            for (int j = 0; j < Size; j++)
            {
                Load.Value = 1;
                Input.SetValue(j);
                Clock.ClockDown();
                Clock.ClockUp();
                Load.Value = 0;
                for (int i = 0; i < Size; i++)
                {
                    Input.SetValue(i);
                    Clock.ClockDown();
                    Clock.ClockUp();
                    if (Output.GetValue() != j)
                        return false;
                }
            }
            Load.Value = 1;
            for (int i = 0; i < Size; i++)
            {
                Input.SetValue(i);
                Clock.ClockDown();
                Clock.ClockUp();
                if (i!=0 & Output.GetValue() != i)
                    return false;
            }
            return true;
        }
    }
}
