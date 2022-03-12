using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This bitwise gate takes as input one WireSet containing n wires, and computes a bitwise function - z_i=f(x_i)
    class BitwiseNotGate : Gate
    {
        public WireSet Input { get; private set; }
        public WireSet Output { get; private set; }
        public int Size { get; private set; }

        //your code here
        private NotGate[] arrNot;
        public BitwiseNotGate(int iSize)
        {
            Size = iSize;
            arrNot = new NotGate [iSize];
            Input = new WireSet(Size);
            Output = new WireSet(Size);
            //your code here
            for (int i = 0; i < iSize; i++)
            {
                NotGate not = new NotGate();
                not.ConnectInput(Input[i]);
                arrNot[i] = not;
                Output[i].ConnectInput(not.Output);
            }
        }

        public void ConnectInput(WireSet ws)
        {
            Input.ConnectInput(ws);
        }

        //an implementation of the ToString method is called, e.g. when we use Console.WriteLine(not)
        //this is very helpful during debugging
        public override string ToString()
        {
            return "Not " + Input + " -> " + Output;
        }

        public override bool TestGate()
        {
            for (int i = 0; i < this.Size; i++)
            {
                Input[i].Value = 0;
                if (Output[i].Value != 1)
                {
                    //Console.WriteLine(ToString());
                    return false;
                }

                Input[i].Value = 1;
                if (Output[i].Value !=0)
                {
                    //Console.WriteLine(ToString());
                    return false;
                }
            }
            return true;
        }
    }
}
