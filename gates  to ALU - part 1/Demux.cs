using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A demux has 2 outputs. There is a single input and a control bit, selecting whether the input should be directed to the first or second output.
    class Demux : Gate
    {
        public Wire Output1 { get; private set; }
        public Wire Output2 { get; private set; }
        public Wire Input { get; private set; }
        public Wire Control { get; private set; }
        private NotGate notC;
        private AndGate andNotC_x;
        private AndGate andC_x;

        //your code here

        public Demux()
        {
            Input = new Wire();
            //your code here
            Output1 = new Wire();
            Output2 = new Wire();
            Input = new Wire();
            Control = new Wire();
            notC = new NotGate();
            andC_x = new AndGate();
            andNotC_x = new AndGate();

            notC.ConnectInput(Control);
            andNotC_x.ConnectInput1(notC.Output);
            andNotC_x.ConnectInput2(Input);

            andC_x.ConnectInput1(Control);
            andC_x.ConnectInput2(Input);

            Output1.ConnectInput(andNotC_x.Output);
            Output2.ConnectInput(andC_x.Output);
        }

        public void ConnectControl(Wire wControl)
        {
            Control.ConnectInput(wControl);
        }
        public void ConnectInput(Wire wInput)
        {
            Input.ConnectInput(wInput);
        }



        public override bool TestGate()
        {
            Control.Value = 0;

            Input.Value = 0;
            if (Output1.Value != 0 | Output2.Value !=0)
            {
                //Console.WriteLine(ToString());
                return false;
            }

            Input.Value = 1;
            if (Output1.Value != 1 | Output2.Value != 0)
            {
                //Console.WriteLine(ToString());
                return false;
            }

            Control.Value = 1;

            Input.Value = 0;
            if (Output1.Value != 0 | Output2.Value != 0)
            {
                //Console.WriteLine(ToString());
                return false;
            }

            Input.Value = 1;
            if (Output1.Value != 0 | Output2.Value != 1)
            {
                //Console.WriteLine(ToString());
                return false;
            }

            return true;
        }
    }
}
