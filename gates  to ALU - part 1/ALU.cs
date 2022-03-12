using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class is used to implement the ALU
    class ALU : Gate
    {
        //The word size = number of bit in the input and output
        public int Size { get; private set; }

        //Input and output n bit numbers
        //inputs
        public WireSet InputX { get; private set; }
        public WireSet InputY { get; private set; }
        public WireSet Control { get; private set; }

        //outputs
        public WireSet Output { get; private set; }
        public Wire Zero { get; private set; }
        public Wire Negative { get; private set; }


        //your code here
        BitwiseMultiwayMux mainMux;
        WireSet oneWireSet;
        public ALU(int iSize)
        {
            Size = iSize;
            InputX = new WireSet(Size);
            InputY = new WireSet(Size);
            Control = new WireSet(6);
            Zero = new Wire();

            //Create and connect all the internal components
            //main mux
            Negative = new Wire();
            mainMux = new BitwiseMultiwayMux(iSize, Control.Size);
            mainMux.ConnectControl(Control);
            //0,1 -0,1
            WireSet zeroWireSet = new WireSet(Size);
            mainMux.ConnectInput(0, zeroWireSet);
            oneWireSet = new WireSet(Size);
            oneWireSet.SetValue(1);
            mainMux.ConnectInput(1, oneWireSet);
            //x,y -2,3
            mainMux.ConnectInput(2, InputX);
            mainMux.ConnectInput(3, InputY);
            //!x,!y -4,5            
            BitwiseMux negMux = new BitwiseMux(Size);
            negMux.ConnectControl(Control[0]);
            negMux.ConnectInput1(InputX);
            negMux.ConnectInput2(InputY);
            BitwiseNotGate multiNotXY = new BitwiseNotGate(Size);
            multiNotXY.ConnectInput(negMux.Output);
            mainMux.ConnectInput(4, multiNotXY.Output);
            mainMux.ConnectInput(5, multiNotXY.Output);
            //-x,-y -6,7

            BitwiseMultiwayMux setTwoComplementMux = new BitwiseMultiwayMux(Size,1);
            WireSet tr = new WireSet(1);
            tr[0].ConnectInput(Control[0]);
            setTwoComplementMux.ConnectControl(tr);           
            setTwoComplementMux.ConnectInput(0,InputX);           
            setTwoComplementMux.ConnectInput(1,InputY);
            WireSet setTwoComplementXY = new WireSet(Size);
            setTwoComplementXY.ConnectInput(setTwoComplement(setTwoComplementMux.Output));
            mainMux.ConnectInput(6, setTwoComplementXY);
            mainMux.ConnectInput(7, setTwoComplementXY);
            //8-11
            
            BitwiseMux muxYX = new BitwiseMux(Size);
            muxYX.ConnectControl(Control[0]);
            muxYX.ConnectInput1(InputX);
            muxYX.ConnectInput2(InputY);
            BitwiseMux mux1 = new BitwiseMux(Size);
            mux1.ConnectControl(Control[1]);
            WireSet minusOne = new WireSet(Size);
            minusOne.Set2sComplement(-1);
            mux1.ConnectInput2(minusOne);
            mux1.ConnectInput1(oneWireSet);
            MultiBitAdder fullAdder = new MultiBitAdder(Size);


            BitwiseMux muxX = new BitwiseMux(Size);
            muxX.ConnectControl(Control[1]);
            muxX.ConnectInput1(InputX);
            muxX.ConnectInput2(setTwoComplementXY);
            BitwiseMux muxY = new BitwiseMux(Size);
            muxY.ConnectControl(Control[0]);
            muxY.ConnectInput2(setTwoComplementXY);
            muxY.ConnectInput1(InputY);

            BitwiseMux muxOpt1 = new BitwiseMux(Size);
            BitwiseMux muxOpt2 = new BitwiseMux(Size);
            muxOpt1.ConnectControl(Control[2]);
            muxOpt2.ConnectControl(Control[2]);

            muxOpt1.ConnectInput1(mux1.Output);
            muxOpt1.ConnectInput2(muxX.Output);

            muxOpt2.ConnectInput1(muxYX.Output);
            muxOpt2.ConnectInput2(muxY.Output);

            fullAdder.ConnectInput1(muxOpt1.Output);
            fullAdder.ConnectInput2(muxOpt2.Output);
            mainMux.ConnectInput(8, fullAdder.Output);
            mainMux.ConnectInput(9, fullAdder.Output);
            mainMux.ConnectInput(10, fullAdder.Output);
            mainMux.ConnectInput(11, fullAdder.Output);
            mainMux.ConnectInput(12, fullAdder.Output);
            mainMux.ConnectInput(13, fullAdder.Output);
            mainMux.ConnectInput(14, fullAdder.Output);
            //x&y -15
            BitwiseAndGate bitWiseAnd = new BitwiseAndGate(Size);
            bitWiseAnd.ConnectInput1(InputX);
            bitWiseAnd.ConnectInput2(InputY);
            mainMux.ConnectInput(15, bitWiseAnd.Output);
            //x^y - 16
            MultiBitOrGate multiAndX = new MultiBitOrGate(Size);
            MultiBitOrGate multiAndY = new MultiBitOrGate(Size);
            multiAndX.ConnectInput(InputX);
            multiAndY.ConnectInput(InputY);
            AndGate andGate = new AndGate();
            andGate.ConnectInput1(multiAndX.Output);
            andGate.ConnectInput2(multiAndY.Output);
            WireSet andOutput = new WireSet(Size);
            andOutput[0].ConnectInput(andGate.Output);
            mainMux.ConnectInput(16, andOutput);
            //x|y -17
            BitwiseOrGate bitWiseOr = new BitwiseOrGate(Size);
            bitWiseOr.ConnectInput1(InputX);
            bitWiseOr.ConnectInput2(InputY);
            mainMux.ConnectInput(17, bitWiseOr.Output);
            //x or y -18
            MultiBitOrGate multiOrX = new MultiBitOrGate(Size);
            multiOrX.ConnectInput(InputX);
            MultiBitOrGate multiOrY = new MultiBitOrGate(Size);
            multiOrY.ConnectInput(InputY);
            OrGate or = new OrGate();
            or.ConnectInput1(multiOrX.Output);
            or.ConnectInput2(multiOrY.Output);
            WireSet outputOr = new WireSet(Size);
            outputOr[0].ConnectInput(or.Output);
            mainMux.ConnectInput(18, outputOr);
            Output = new WireSet(Size);
            Output.ConnectInput(mainMux.Output);
            //zr
            MultiBitOrGate zrOr = new MultiBitOrGate(Size);
            NotGate zrNot = new NotGate();
            zrOr.ConnectInput(Output);
            zrNot.ConnectInput(zrOr.Output);
            Zero.ConnectInput(zrNot.Output);

            //ng
            Negative.ConnectInput(Output[Size - 1]);
        }

        private WireSet setTwoComplement(WireSet wireSet)
        {
            BitwiseNotGate not = new BitwiseNotGate(Size);
            not.ConnectInput(wireSet);
            MultiBitAdder adder = new MultiBitAdder(Size);
            adder.ConnectInput1(oneWireSet);
            adder.ConnectInput2(not.Output);
            return adder.Output;
        }

        public override bool TestGate()
        {
            return true;
        }
    }
}
