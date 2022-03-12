using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This gate implements the or operation. To implement it, follow the example in the And gate.
    class OrGate : TwoInputGate
    {
        //your code here 
        private NotGate notX;
        private NotGate notY;
        //private AndGate and;
        //private NotGate notOutPut;

        private NAndGate nand;
        public OrGate()
        {
            //your code here 
            
            notX = new NotGate();
            notY = new NotGate();
            //and = new AndGate();
            //notOutPut = new NotGate();
            Input1 = new Wire();
            Input2 = new Wire();
            Output = new Wire();

            notX.ConnectInput(Input1);
            notY.ConnectInput(Input2);

            nand = new NAndGate();
            nand.ConnectInput1(notX.Output);
            nand.ConnectInput2(notY.Output);

            //notOutPut.ConnectInput(and.Output);
            Output.ConnectInput(nand.Output);
            



        }


        public override string ToString()
        {
            return "Or " + Input1.Value + "," + Input2.Value + " -> " + Output.Value;
        }

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
            if (Output.Value != 1)
            {
                //Console.WriteLine(ToString());
                return false;
            }

            return true;
        }
    }

}
