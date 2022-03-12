# Elements-of-Computing-Systems
My full implementation from nand gate to compiler

3 parts of implemntations

1. - gates - nand, or, and, mux, demux, multibit, bitwise
   - Set2sCompliment, flip-flpo, counter, register.
   - I implement an RAM,ALU with 19 functions (x+y,x-y,!x and more)

2. - I implement a full Assembly and CPU.
   - connect all the parts together (cpu,alu, memory and more).
   - Implement all the function that translate assembly to MachineCode.
   *** part 1 is fraudulently implemented for efficiency
  
3. - Tokenazing step
   - Implement of Parser that get input of tokens stack and make parse tree.
   - Implement all the function that connect all together to full compiler (GenerateCode, SimplifyExpression, ComputeSymbolTable and more).
