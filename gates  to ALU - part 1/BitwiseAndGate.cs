using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A two input bitwise gate takes as input two WireSets containing n wires, and computes a bitwise function - z_i=f(x_i,y_i)
    class BitwiseAndGate : BitwiseTwoInputGate
    {
        //your code here
        private AndGate[] arrAnd;
        public BitwiseAndGate(int iSize)
            : base(iSize)
        {
            //your code here
            arrAnd = new AndGate[iSize];
            for (int i=0; i< iSize; i++)
            {
                AndGate and = new AndGate();
                arrAnd[i] = and;
                and.ConnectInput1(Input1[i]);
                and.ConnectInput2(Input2[i]);
                Output[i].ConnectInput(and.Output);
            }
        }

        //an implementation of the ToString method is called, e.g. when we use Console.WriteLine(and)
        //this is very helpful during debugging
        public override string ToString()
        {
            return "And " + Input1 + ", " + Input2 + " -> " + Output;
        }

        public override bool TestGate()
        {
            
            for (int i=0; i<this.Size; i++)
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
                if (Output[i].Value != 0)
                {
                    //Console.WriteLine(ToString());
                    return false;
                }

                Input1[i].Value = 0;
                Input2[i].Value = 1;
                if (Output[i].Value != 0)
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
