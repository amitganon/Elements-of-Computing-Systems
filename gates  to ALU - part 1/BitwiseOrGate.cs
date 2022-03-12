using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A two input bitwise gate takes as input two WireSets containing n wires, and computes a bitwise function - z_i=f(x_i,y_i)
    class BitwiseOrGate : BitwiseTwoInputGate
    {
        //your code here
        private OrGate[] arrOr;
        public BitwiseOrGate(int iSize)
            : base(iSize)
        {
            //your code here
            arrOr = new OrGate[iSize];
            for(int i=0; i<iSize; i++)
            {
                OrGate or = new OrGate();
                arrOr [i]= or;
                or.ConnectInput1(Input1[i]);
                or.ConnectInput2(Input2[i]);
                Output[i].ConnectInput(or.Output);
            }
        }

        //an implementation of the ToString method is called, e.g. when we use Console.WriteLine(or)
        //this is very helpful during debugging
        public override string ToString()
        {
            return "Or " + Input1 + ", " + Input2 + " -> " + Output;
        }

        public override bool TestGate()
        {

            for (int i = 0; i < this.Size; i++)
            {
                Input1[i].Value = 0;
                Input2[i].Value = 0;
                if (Output[i].Value != 0)
                {
                    //Console.WriteLine(ToString());
                    return false;
                }

                Input1[i].Value = 1;
                Input2[i].Value = 0;
                if (Output[i].Value != 1)
                {
                    //Console.WriteLine(ToString());
                    return false;
                }

                Input1[i].Value = 0;
                Input2[i].Value = 1;
                if (Output[i].Value != 1)
                {
                    //Console.WriteLine(ToString());
                    return false;
                }

                Input1[i].Value = 1;
                Input2[i].Value = 1;
                if (Output[i].Value != 1)
                {
                    //Console.WriteLine(ToString());
                    return false;
                }
            }
            return true;
        }
    }
}
