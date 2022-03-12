using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A bitwise gate takes as input WireSets containing n wires, and computes a bitwise function - z_i=f(x_i)
    class BitwiseMux : BitwiseTwoInputGate
    {
        public Wire ControlInput { get; private set; }

        //your code here
        private MuxGate[] arrMux;

        public BitwiseMux(int iSize)
            : base(iSize)
        {
            ControlInput = new Wire();
            //your code here
            arrMux = new MuxGate[iSize];
            for (int i=0; i< iSize; i++)
            {
                MuxGate mux = new MuxGate();
                arrMux[i] = mux;
                mux.ConnectControl(ControlInput);
                mux.ConnectInput1(Input1[i]);
                mux.ConnectInput2(Input2[i]);
                Output[i].ConnectInput(mux.Output);
            }
        }

        public void ConnectControl(Wire wControl)
        {
            ControlInput.ConnectInput(wControl);
        }



        public override string ToString()
        {
            return "Mux " + Input1 + "," + Input2 + ",C" + ControlInput.Value + " -> " + Output;
        }




        public override bool TestGate()
        {
            for (int i = 0; i < this.Size; i++)
            {
                ControlInput.Value = 1;
                Input1[i].Value = 0;
                Input2[i].Value = 0;
                if (Output[i].Value != 0)
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
                Input2[i].Value = 0;
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

                ControlInput.Value = 0;

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
