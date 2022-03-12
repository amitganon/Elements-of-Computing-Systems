using System;
using System.Collections.Generic;
using System.Text;

namespace Components
{
    class MultiWayDemux : Gate
    {
        public Wire Input { get; private set; }
        public WireSet Control { get; private set; }
        public WireSet Output { get; private set; }
        MultiBitAndGate[] multiAndArr;
        public int controlSise;
        public MultiWayDemux(int ControlSize)
        {
            controlSise = ControlSize;
            Input = new Wire();
            Control = new WireSet(ControlSize);
            Output = new WireSet((int)Math.Pow(2, ControlSize));
            multiAndArr = new MultiBitAndGate[(int)Math.Pow(2, ControlSize)];
            WireSet controlNum = new WireSet(ControlSize);
            for (int i=0; i< (int)Math.Pow(2, ControlSize); i++)
            {
                MultiBitAndGate multiAnd = new MultiBitAndGate(ControlSize+1);
                WireSet wirePerAndGate = new WireSet(ControlSize + 1);
                wirePerAndGate[ControlSize].ConnectInput(Input);
                controlNum.SetValue(i);
                for (int j=0; j< ControlSize; j++)
                {
                    if (controlNum[j].Value == 1)
                        wirePerAndGate[j].ConnectInput(Control[j]);
                    else
                    {
                        NotGate not = new NotGate();
                        not.ConnectInput(Control[j]);
                        wirePerAndGate[j].ConnectInput(not.Output);
                    }
                }
                multiAnd.ConnectInput(wirePerAndGate);
                Output[i].ConnectInput(multiAnd.Output);
            }
        }

        public void ConnectInput(Wire wInput)
        {
            Input.ConnectInput(wInput);
        }

        public void ConnectControlers(WireSet wInput)
        {
            Control.ConnectInput(wInput);
        }

        public override bool TestGate()
        {
            Input.Value = 0;
            for(int i=0; i< Math.Pow(2, controlSise); i++)
            {
                Control.SetValue(i);
                for (int j = 0; j < Math.Pow(2, controlSise); j++)
                {
                    if (Output[j].Value != Input.Value & i == j)
                        return false;
                    if (Output[j].Value != 0 & i != j)
                        return false;
                }
            }
            Input.Value = 1;
            for (int i = 0; i < Math.Pow(2, controlSise); i++)
            {
                Control.SetValue(i);
                for (int j = 0; j < Math.Pow(2, controlSise); j++)
                {
                    if (Output[j].Value != Input.Value & i == j)
                        return false;
                    if (Output[j].Value != 0 & i != j)
                        return false;
                }
            }
            return true;
        }
    }
}
