using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    class Program
    {
        static void Main(string[] args)
        {
            //This is an example of a testing code that you should run for all the gates that you create

            //Create a gate           

            AndGate and = new AndGate();
            //Test that the unit testing works properly

            if (!and.TestGate())
            {
                Console.WriteLine(and + "");
                Console.WriteLine("bugbug");
            }

            OrGate or = new OrGate();
            if (!or.TestGate())
            {
                Console.WriteLine("or -");
                Console.WriteLine("bug in orGate");
            }

            XorGate xor = new XorGate();
            if (!xor.TestGate())
            {
                Console.WriteLine("xor -");
                Console.WriteLine("bug in xorGate");
            }

            MuxGate mux = new MuxGate();
            if (!mux.TestGate())
            {
                Console.WriteLine("mux -");
                Console.WriteLine("bug in muxGate");
            }

            Demux demux = new Demux();
            if (!demux.TestGate())
            {
                Console.WriteLine("bug in demuxGate");
                Console.WriteLine("demux -");
            }

            MultiBitAndGate multiAnd = new MultiBitAndGate(4);

            if (!multiAnd.TestGate())
            {
                Console.WriteLine("MultiBitAndGate -");
                Console.WriteLine("bug in MultiBitAndGate");
            }

            MultiBitOrGate multiOr = new MultiBitOrGate(4);
            if (!multiOr.TestGate())
            {
                Console.WriteLine("MultiBitOrGate -");
                Console.WriteLine("bug in MultiBitOrGate");
            }

            BitwiseAndGate bitWiseAnd = new BitwiseAndGate(4);

            if (!bitWiseAnd.TestGate())
            {
                Console.WriteLine("BitwiseAndGate -");
                Console.WriteLine("bug in BitwiseAndGate");
            }

            BitwiseNotGate bitWiseNot = new BitwiseNotGate(4);
            if (!bitWiseNot.TestGate())
            {
                Console.WriteLine("BitwiseNotGate -");
                Console.WriteLine("bug in BitwiseNotGate");
            }

            BitwiseOrGate bitWiseOr = new BitwiseOrGate(4);
            if (!bitWiseOr.TestGate())
            {
                Console.WriteLine("BitwiseOrGate -");
                Console.WriteLine("bug in BitwiseOrGate");
            }

            BitwiseMux bitWiseMux = new BitwiseMux(4);
            if (!bitWiseMux.TestGate())
            {
                Console.WriteLine("bitWiseMux -");
                Console.WriteLine("bug in bitWiseMux");
            }

            BitwiseDemux bitWiseDemux = new BitwiseDemux(4);
            if (!bitWiseDemux.TestGate())
            {
                Console.WriteLine("bitWiseDemux -");
                Console.WriteLine("bug in bitWiseDemux");
            }

            MultiWayMux MultiMux = new MultiWayMux(4);
            if (!MultiMux.TestGate())
            {
                Console.WriteLine("MultiWayMux -");
                Console.WriteLine("bug in MultiWayMux");
            }

            
            BitwiseMultiwayMux BitWiseMultiMux = new BitwiseMultiwayMux(4,2);
            if (!BitWiseMultiMux.TestGate())
            {
                Console.WriteLine("BitwiseMultiwayMux -");
                Console.WriteLine("bug in BitwiseMultiwayMux");
            }
            

            MultiWayDemux MultiDemux = new MultiWayDemux(4);
            if (!MultiDemux.TestGate())
            {
                Console.WriteLine("MultiWayDemux -");
                Console.WriteLine("bug in MultiWayDemux");
            }
            
            BitwiseMultiwayDemux BitWiseMultiDemux = new BitwiseMultiwayDemux(4, 2);
            if (!BitWiseMultiDemux.TestGate())
            {
                Console.WriteLine("BitwiseMultiwayDemux -");
                Console.WriteLine("bug in BitwiseMultiwayDemux");
            }
            

            HalfAdder halfAdder = new HalfAdder();
            if (!halfAdder.TestGate())
            {
                Console.WriteLine("HalfAdder -");
                Console.WriteLine("bug in HalfAdder");
            }


            FullAdder fallAdder = new FullAdder();
            if (!fallAdder.TestGate())
            {
                Console.WriteLine("FullAdder -");
                Console.WriteLine("bug in FullAdder");
            }


            MultiBitAdder multiBitAdder = new MultiBitAdder(3);
            if (!multiBitAdder.TestGate())
            {
                Console.WriteLine("MultiBitAdder -");
                Console.WriteLine("bug in MultiBitAdder");
            }                      

            ALU alu = new ALU(6);            
            if (!alu.TestGate())
            {
                Console.WriteLine("ALU -");
                Console.WriteLine("bug in ALU");
            }

            SingleBitRegister reg = new SingleBitRegister();
            if (!reg.TestGate())
            {
                Console.WriteLine("SingleBitRegister -");
                Console.WriteLine("bug in SingleBitRegister");
            }

            MultiBitRegister multiReg = new MultiBitRegister(4);
            if (!multiReg.TestGate())
            {
                Console.WriteLine("MultiBitRegister -");
                Console.WriteLine("bug in MultiBitRegister");
            }

            Memory memory = new Memory(3,6);
            if (!memory.TestGate())
            {
                Console.WriteLine("Memory -");
                Console.WriteLine("bug in Memory");
            }

            Console.WriteLine("");

            //Now we ruin the nand gates that are used in all other gates. The gate should not work properly after this.
            NAndGate.Corrupt = true;
            if (and.TestGate())
            {
                Console.WriteLine(and + "");
                Console.WriteLine("bugbug");
            }

            if (or.TestGate())
            {
                Console.WriteLine("ruin or -");
                Console.WriteLine("bug in orGate");
            }

            if (xor.TestGate())
            {
                Console.WriteLine("ruin xor -");
                Console.WriteLine("bug in xorGate");
            }

            if (mux.TestGate())
            {
                Console.WriteLine("ruin  mux -");
                Console.WriteLine("bug in muxGate");
            }

            if (demux.TestGate())
            {
                Console.WriteLine("ruin demux -");
                Console.WriteLine("bug in demuxGate");
            }

            if (multiAnd.TestGate())
            {
                Console.WriteLine("ruin MultiBitAndGate -");
                Console.WriteLine("bug in MultiBitAndGate");
            }

            if (multiOr.TestGate())
            {
                Console.WriteLine("ruin MultiBitOrGate -");
                Console.WriteLine("bug in MultiBitOrGate");
            }

            if (bitWiseAnd.TestGate())
            {
                Console.WriteLine("ruin BitwiseAndGate -");
                Console.WriteLine("bug in BitwiseAndGate");
            }
            
            if (bitWiseOr.TestGate())
            {
                Console.WriteLine("ruin BitwiseOrGate -");
                Console.WriteLine("bug in BitwiseOrGate");
            }

            if (bitWiseMux.TestGate())
            {
                Console.WriteLine("ruin bitWiseMux -");
                Console.WriteLine("bug in bitWiseMux");
            }

            if (bitWiseDemux.TestGate())
            {
                Console.WriteLine("ruin bitWiseDemux -");
                Console.WriteLine("bug in bitWiseDemux");
            }

            if (MultiMux.TestGate())
            {
                Console.WriteLine("ruin MultiWayMux -");
                Console.WriteLine("bug in MultiWayMux");
            }
           
            if (MultiDemux.TestGate())
            {
                Console.WriteLine("ruin MultiWayDemux -");
                Console.WriteLine("bug in MultiWayDemux");
            }

            if (halfAdder.TestGate())
            {
                Console.WriteLine("ruin HalfAdder -");
                Console.WriteLine("bug in HalfAdder");
            }


            if (fallAdder.TestGate())
            {
                Console.WriteLine("ruin FullAdder -");
                Console.WriteLine("bug in FullAdder");
            }


            if (multiBitAdder.TestGate())
            {
                Console.WriteLine("ruin MultiBitAdder -");
                Console.WriteLine("bug in MultiBitAdder");
            }
            
            WireSet testWire = new WireSet(4);
            for(int i=-8; i < 8; i++)
            {
                testWire.Set2sComplement(i);
                if(testWire.Get2sComplement() != i)
                {
                    Console.WriteLine("Set2sComplement -test fail");
                    Console.WriteLine(testWire.ToString());
                }
            }

            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}
