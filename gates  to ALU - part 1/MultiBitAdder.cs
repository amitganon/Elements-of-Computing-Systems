using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class implements an adder, receving as input two n bit numbers, and outputing the sum of the two numbers
    class MultiBitAdder : Gate
    {
        //Word size - number of bits in each input
        public int Size { get; private set; }

        public WireSet Input1 { get; private set; }
        public WireSet Input2 { get; private set; }
        public WireSet Output { get; private set; }
        //An overflow bit for the summation computation
        public Wire Overflow { get; private set; }


        public MultiBitAdder(int iSize)
        {
            Size = iSize;
            Input1 = new WireSet(Size);
            Input2 = new WireSet(Size);
            Output = new WireSet(Size);
            Overflow = new Wire();
            //your code here
            HalfAdder halfAdder = new HalfAdder();
            halfAdder.ConnectInput1(Input1[0]);
            halfAdder.ConnectInput2(Input2[0]);
            Output[0].ConnectInput(halfAdder.Output);
            Wire carryInput = halfAdder.CarryOutput;
            for (int i=1; i<Size; i++)
            {
                FullAdder fullfAdder = new FullAdder();
                fullfAdder.ConnectInput1(Input1[i]);
                fullfAdder.ConnectInput2(Input2[i]);
                fullfAdder.CarryInput.ConnectInput(carryInput);
                Output[i].ConnectInput(fullfAdder.Output);
                carryInput=fullfAdder.CarryOutput;
                if(i==Size-1)
                    Overflow.ConnectInput(fullfAdder.CarryOutput);
            }
        }

        public override string ToString()
        {
            return Input1 + "(" + Input1.Get2sComplement() + ")" + " + " + Input2 + "(" + Input2.Get2sComplement() + ")" + " = " + Output + "(" + Output.Get2sComplement() + ")";
        }

        public void ConnectInput1(WireSet wInput)
        {
            Input1.ConnectInput(wInput);
        }
        public void ConnectInput2(WireSet wInput)
        {
            Input2.ConnectInput(wInput);
        }


        public override bool TestGate()
        {
            WireSet check = new WireSet(Size);
            for (int i=0; i<Math.Pow(2,Size); i++)
            {
                Input1.SetValue(i);
                for(int j=0; j< Math.Pow(2, Size); j++)
                {
                    Input2.SetValue(j);
                    check.SetValue(i + j);
                    if (check.GetValue() != Output.GetValue())
                        return false;
                    if (i + j >= Math.Pow(2, Size) & Overflow.Value != 1)
                        return false;
                    else if (i + j < Math.Pow(2, Size) & Overflow.Value != 0)
                        return false;
                }
            }
            return true;            
        }
    }
}
