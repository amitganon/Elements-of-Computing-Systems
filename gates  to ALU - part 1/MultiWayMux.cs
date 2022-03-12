using System;
using System.Collections.Generic;
using System.Text;

namespace Components
{
    class MultiWayMux : Gate
    {
        public WireSet Input { get; private set; }
        public WireSet Control { get; private set; }
        public Wire Output { get; private set; }
        private MultiBitAndGate [] arrMultiAnd;
        private MultiBitOrGate multiOr;
        public int Size;
        //private MuxGate[] arrMux;
        public MultiWayMux(int iSize)
        {
            Size = iSize;
            int numOfControllers = (int)Math.Log(iSize, 2);
            Input = new WireSet(iSize);
            Control = new WireSet(numOfControllers);
            Output = new Wire();
            arrMultiAnd = new MultiBitAndGate[iSize];
            multiOr = new MultiBitOrGate(iSize);

            for (int i=0; i< iSize; i++)
            {
                MultiBitAndGate multiAnd = new MultiBitAndGate(numOfControllers + 1);
                WireSet wireSet = new WireSet(numOfControllers + 1);
                wireSet[numOfControllers].ConnectInput(Input[i]);

                WireSet IWire = new WireSet(numOfControllers);
                IWire.SetValue(i);
                for (int j=0; j< numOfControllers; j++)
                {
                    if (IWire[j].Value == 1)
                        wireSet[j].ConnectInput(Control[j]);
                    else
                    {
                        NotGate not = new NotGate();
                        not.ConnectInput(Control[j]);
                        wireSet[j].ConnectInput(not.Output);
                    }
                }
                multiAnd.ConnectInput(wireSet);
                arrMultiAnd[i] = multiAnd;
            }

            WireSet andWireSet = new WireSet(iSize);
            for(int i=0; i< iSize; i++)
                andWireSet[i].ConnectInput(arrMultiAnd[i].Output);
            multiOr.ConnectInput(andWireSet);
            Output.ConnectInput(multiOr.Output);
        }

        public void ConnectInput(WireSet wInput)
        {
            Input.ConnectInput(wInput);
        }

        public void ConnectControlers(WireSet wInput)
        {
            Control.ConnectInput(wInput);
        }

        public override bool TestGate()
        {
            
            for(int i = 0; i <Size; i++)
            {
                Input.SetValue(i);
                for(int j=0; j<Math.Log(Size,2); j++)
                {
                    Control.SetValue(j);
                    if (Output.Value != Input[Control.GetValue()].Value)
                    {
                        //Console.WriteLine(Output);
                        //Console.WriteLine(Input);
                        //Console.WriteLine(Control.GetValue());
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
   
