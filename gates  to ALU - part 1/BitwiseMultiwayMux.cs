using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    class BitwiseMultiwayMux : Gate, Component
    {
        public int Size { get; private set; }
        public int ControlBits { get; private set; }
        public WireSet Output { get; private set; }
        public WireSet Control { get; private set; }
        public WireSet[] Inputs { get; private set; }


        public BitwiseMultiwayMux(int iSize, int cControlBits)
        {
            ControlBits = cControlBits;//bug fix

            Size = iSize;
            Output = new WireSet(Size);
            Control = new WireSet(cControlBits);
            Inputs = new WireSet[(int)Math.Pow(2, cControlBits)];

            for (int i = 0; i < Inputs.Length; i++)
            {
                Inputs[i] = new WireSet(Size);
                for (int j = 0; j < Inputs[i].Size; j++)
                    Inputs[i][j].ConnectOutput(this);
            }
            for (int i = 0; i < cControlBits; i++)
                Control[i].ConnectOutput(this);

        }


        public void ConnectInput(int i, WireSet wsInput)
        {
            Inputs[i].ConnectInput(wsInput);
        }
        public void ConnectControl(WireSet wsControl)
        {
            Control.ConnectInput(wsControl);
        }

        public override bool TestGate()
        {
            Random rnd = new Random();
            WireSet[] aInputs = new WireSet[Inputs.Length];
            for (int i = 0; i < Inputs.Length; i++)
            {
                aInputs[i] = new WireSet(Size);
                ConnectInput(i, aInputs[i]);
                aInputs[i].SetValue(i + 1);
            }
            WireSet wsControl = new WireSet(ControlBits);
            ConnectControl(wsControl);
            for (int i = 0; i < Inputs.Length; i++)
            {

                wsControl.SetValue(i);

                if (Output.GetValue() != Inputs[i].GetValue())
                    return false;
            }
            return true;
        }

        #region Component Members

        public void Compute()
        {
            Output.SetValue(Inputs[Control.GetValue()].GetValue());
        }

        #endregion


    }
}
