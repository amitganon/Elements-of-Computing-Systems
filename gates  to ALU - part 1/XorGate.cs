using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This gate implements the xor operation. To implement it, follow the example in the And gate.
    class XorGate : TwoInputGate
    {
        //your code here
        private NotGate notX;
        private NotGate notY;
        private AndGate andNotX_Y;
        private AndGate andNotY_X;
        private OrGate or;

    public XorGate()
        {
            notX = new NotGate();
            notY = new NotGate();
            andNotX_Y = new AndGate();
            andNotY_X = new AndGate();
            or = new OrGate();
            Input1 = new Wire();
            Input2 = new Wire();
            Output = new Wire();

            notX.ConnectInput(Input1);
            andNotX_Y.ConnectInput1(notX.Output);
            andNotX_Y.ConnectInput2(Input2);

            notY.ConnectInput(Input2);
            andNotY_X.ConnectInput1(Input1);
            andNotY_X.ConnectInput2(notY.Output);

            or.ConnectInput1(andNotY_X.Output);
            or.ConnectInput2(andNotX_Y.Output);

            Output.ConnectInput(or.Output);
        }

        //an implementation of the ToString method is called, e.g. when we use Console.WriteLine(xor)
        //this is very helpful during debugging
        public override string ToString()
        {
            return "Xor " + Input1.Value + "," + Input2.Value + " -> " + Output.Value;
        }


        //this method is used to test the gate. 
        //we simply check whether the truth table is properly implemented.
        public override bool TestGate()
        {
            Input1.Value = 0;
            Input2.Value = 0;
            if (Output.Value != 0)
            {
                //Console.WriteLine(ToString());
                return false;
            }


            Input1.Value = 0;
            Input2.Value = 1;
            if (Output.Value != 1)
            {
                //Console.WriteLine(ToString());
                return false;
            }

            Input1.Value = 1;
            Input2.Value = 0;
            if (Output.Value != 1)
            {
                //Console.WriteLine(ToString());
                return false;
            }

            Input1.Value = 1;
            Input2.Value = 1;
            if (Output.Value != 0)
            {
                //Console.WriteLine(ToString());
                return false;
            }

            return true;
        }
    }
}
