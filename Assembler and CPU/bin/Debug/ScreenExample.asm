

   @SCREEN
   D=A
   @address
   M=D-1
(ZERO_DIGIT)
   @48
   D=A
   @digit
   M=D
(LOOP)
   @digit
   D=M
   M=M+1
   @address
   M=M+1
   A=M
   M=D
   @57
   D=D-A
   @ZERO_DIGIT
   D;JEQ
   @LOOP
   1;JGT
(INFINITE_LOOP)
   @INFINITE_LOOP
   0;JMP
