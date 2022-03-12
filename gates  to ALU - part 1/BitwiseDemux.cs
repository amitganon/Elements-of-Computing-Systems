using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A bitwise gate takes as input WireSets containing n wires, and computes a bitwise function - z_i=f(x_i)
    class BitwiseDemux : Gate
    {
        public int Size { get; private set; }
        public WireSet Output1 { get; private set; }
        public WireSet Output2 { get; private set; }
        public WireSet Input { get; private set; }
        public Wire Control { get; private set; }

        //your code here
        private Demux[] arrDemux;
        public BitwiseDemux(int iSize)
        {
            Size = iSize;
            Control = new Wire();
            Input = new WireSet(Size);

            //your code here
            Control = new Wire();
            Input = new WireSet(iSize);
            Output1 = new WireSet(iSize);
            Output2 = new WireSet(iSize);
            arrDemux = new Demux[iSize];

            for (int i=0; i< iSize; i++)
            {
                Demux demux = new Demux();
                arrDemux[i] = demux;
                demux.ConnectControl(Control);
                demux.ConnectInput(Input[i]);
                Output1[i].ConnectInput(demux.Output1);
                Output2[i].ConnectInput(demux.Output2);
            }

        }

        public void ConnectControl(Wire wControl)
        {
            Control.ConnectInput(wControl);
        }
        public void ConnectInput(WireSet wsInput)
        {
            Input.ConnectInput(wsInput);
        }

        public override bool TestGate()
        {
            for (int i = 0; i < this.Size; i++)
            {
                Control.Value = 1;
                Input[i].Value = 0;
                if (Output1[i].Value != 0 | Output2[i].Value != 0)
                {
                    return false;
                }


                Input[i].Value = 1;
                if (Output1[i].Value != 0 | Output2[i].Value != 1)
                {
                    return false;
                }

                
                Control.Value = 0;

                Input[i].Value = 0;
                if (Output1[i].Value != 0 | Output2[i].Value != 0)
                {
                    return false;
                }

                Input[i].Value = 1;
                if (Output1[i].Value != 1 | Output2[i].Value != 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
