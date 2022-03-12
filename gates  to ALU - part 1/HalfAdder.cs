using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class implements a HalfAdder, taking as input 2 bits - 2 numbers and computing the result in the output, and the carry out.

    class HalfAdder : TwoInputGate
    {
        public Wire CarryOutput { get; private set; }

        //your code here
        private AndGate and;
        private XorGate xor;

        public HalfAdder()
        {
            //your code here
            and = new AndGate();
            xor = new XorGate();
            Input1 = new Wire();
            Input2 = new Wire();
            Output = new Wire();
            CarryOutput = new Wire();
            and.ConnectInput1(Input1);
            and.ConnectInput2(Input2);
            xor.ConnectInput1(Input1);
            xor.ConnectInput2(Input2);
            CarryOutput.ConnectInput(and.Output);
            Output.ConnectInput(xor.Output);
        }


        public override string ToString()
        {
            return "HA " + Input1.Value + "," + Input2.Value + " -> " + Output.Value + " (C" + CarryOutput + ")";
        }

        public override bool TestGate()
        {
            Input1.Value = 0;
            Input2.Value = 0;
            if(CarryOutput.Value !=0 | Output.Value != 0)
            {
                //Console.WriteLine(this.ToString());
                return false;
            }

            Input1.Value = 0;
            Input2.Value = 1;
            if (CarryOutput.Value != 0 | Output.Value != 1)
            {
                //Console.WriteLine(this.ToString());
                return false;
            }

            Input1.Value = 1;
            Input2.Value = 0;
            if (CarryOutput.Value != 0 | Output.Value != 1)
            {
                //Console.WriteLine(this.ToString());
                return false;
            }

            Input1.Value = 1;
            Input2.Value = 1;
            if (CarryOutput.Value != 1 | Output.Value != 0)
            {
                //Console.WriteLine(this.ToString());
                return false;
            }
            return true;
        }
    }
}
