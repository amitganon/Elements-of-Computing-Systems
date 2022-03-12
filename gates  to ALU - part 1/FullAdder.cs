using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class implements a FullAdder, taking as input 3 bits - 2 numbers and a carry, and computing the result in the output, and the carry out.
    class FullAdder : TwoInputGate
    {
        public Wire CarryInput { get; private set; }
        public Wire CarryOutput { get; private set; }

        //your code here
        HalfAdder halfAdder1;
        HalfAdder halfAdder2;
        OrGate or;

        public FullAdder()
        {
            CarryInput = new Wire();
            //your code here
            CarryOutput = new Wire();
            halfAdder1 = new HalfAdder();
            halfAdder2 = new HalfAdder();
            Input1 = new Wire();
            Input2 = new Wire();
            or = new OrGate();
            halfAdder1.ConnectInput1(Input1);
            halfAdder1.ConnectInput2(Input2);
            or.ConnectInput1(halfAdder1.CarryOutput);
            or.ConnectInput2(halfAdder2.CarryOutput);
            CarryOutput.ConnectInput(or.Output);
            halfAdder2.ConnectInput1(halfAdder1.Output);
            halfAdder2.ConnectInput2(CarryInput);
            Output.ConnectInput(halfAdder2.Output);

        }


        public override string ToString()
        {
            return Input1.Value + "+" + Input2.Value + " (C" + CarryInput.Value + ") = " + Output.Value + " (C" + CarryOutput.Value + ")";
        }

        public override bool TestGate()
        {
            Input1.Value = 0;
            Input2.Value = 0;
            CarryInput.Value = 1;
            if (CarryOutput.Value != 0 | Output.Value != 1)
            {
                //Console.WriteLine(this.ToString());
                return false;
            }

            Input1.Value = 0;
            Input2.Value = 1;
            if (CarryOutput.Value != 1 | Output.Value != 0)
            {
                //Console.WriteLine(this.ToString());
                return false;
            }

            Input1.Value = 1;
            Input2.Value = 0;
            if (CarryOutput.Value != 1 | Output.Value != 0)
            {
                //Console.WriteLine(this.ToString());
                return false;
            }

            Input1.Value = 1;
            Input2.Value = 1;
            if (CarryOutput.Value != 1 | Output.Value != 1)
            {
                //Console.WriteLine(this.ToString());
                return false;
            }





            Input1.Value = 0;
            Input2.Value = 0;
            CarryInput.Value = 0;
            if (CarryOutput.Value != 0 | Output.Value != 0)
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
