# Elements-of-Computing-Systems
My full implementation from nand gate to a 16-bit compiler

3 parts of implementation

1. - gates - nand, or, and, mux, demux, multibit, bitwise
   - Set2sCompliment, flip-flop, counter, register.
   - I implement a RAM, ALU with 19 functions (x+y,x-y,!x and more)

2. - I implement a full Assembly and CPU.
   - connect all the parts (Cpu, Alu, memory, and more).
   - Implement all the functions that translate assembly to machine code.
   *** part 1 is fraudulently implemented for efficiency
  
3. - Tokenazing step
   - Implement of Parser that gets input of tokens stack and makes parse tree.
   - Implement all the functions that connect all together to a full compiler (GenerateCode, SimplifyExpression, ComputeSymbolTable, and more).
