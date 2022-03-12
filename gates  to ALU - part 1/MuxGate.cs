using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A mux has 2 inputs. There is a single output and a control bit, selecting which of the 2 inpust should be directed to the output.
    class MuxGate : TwoInputGate
    {
        public Wire ControlInput { get; private set; }
        //your code here
        private NotGate notC;
        private AndGate andNotC_x;
        private AndGate andC_y;
        private OrGate or;

        public MuxGate()
        {
            ControlInput = new Wire();
            notC = new NotGate();
            andNotC_x = new AndGate();
            andC_y = new AndGate();
            or = new OrGate();
            Input1 = new Wire();
            Input2 = new Wire();
            Output = new Wire();

            notC.ConnectInput(ControlInput);
            andNotC_x.ConnectInput1(notC.Output);
            andNotC_x.ConnectInput2(Input1);

            andC_y.ConnectInput1(ControlInput);
            andC_y.ConnectInput2(Input2);

            or.ConnectInput1(andNotC_x.Output);
            or.ConnectInput2(andC_y.Output);

            Output.ConnectInput(or.Output);

            //your code here

        }

        public void ConnectControl(Wire wControl)
        {
            ControlInput.ConnectInput(wControl);
        }


        public override string ToString()
        {
            return "Mux " + Input1.Value + "," + Input2.Value + ",C" + ControlInput.Value + " -> " + Output.Value;
        }



        public override bool TestGate()
        {
            ControlInput.Value = 1;
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
            if (Output.Value != 0)
            {
                //Console.WriteLine(ToString());
                return false;
            }

            Input1.Value = 1;
            Input2.Value = 1;
            if (Output.Value != 1)
            {
                //Console.WriteLine(ToString());
                return false;
            }

            ControlInput.Value = 0;

            Input1.Value = 0;
            Input2.Value = 0;
            if (Output.Value != 0)
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

            Input1.Value = 0;
            Input2.Value = 1;
            if (Output.Value != 0)
            {
                //Console.WriteLine(ToString());
                return false;
            }

            Input1.Value = 1;
            Input2.Value = 1;
            if (Output.Value != 1)
            {
                //Console.WriteLine(ToString());
                return false;
            }
            return true;
        }
    }
}
