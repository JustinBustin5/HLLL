# HLLL
An assembly-like scripting language.

To get started, download the latest release of HLLL and run ```hlll HelloWorld.hlll``` (Windows) or ```dotnet hlll.dll HelloWorld.hlll```. (Cross-Platform)

Note: HLLL comes with a UDL (User Defined Language) for Notepad++. This gives you syntax highlighting for HLLL. Copy "hlll-version.udl.xml" to "C:\Users\justi\AppData\Roaming\Notepad++\userDefineLangs\"

# Command Line Interface
Note: This application can run on any OS, but the commands listed here are written for Windows. To run it on platforms other than Windows, substitute ```hlll``` for ```dotnet hlll.dll```.

To run a script, just type ```hlll scriptname[.extension]```. Note: If you don't specify an extension, the program will assume you mean .hlll.

Windows: You can also run a script by dragging the file into HLLL.exe or using the Open With dialog to open the file with HLLL.exe

# Language Grammar
Each line of code contains an **instruction**. There are 2 parts to an instruction: the *opcode*, and the *arguments*. The opcode is the main part of the instruction, determining what the instruction actually does. For example, ```add``` adds the two uppermost numbers on the stack and pushes the result, ```rdl``` reads a line of user input and pushes it to the stack, and ```nop``` does nothing, just to name a few.

The arguments are the part of the instruction sometimes needed by the opcode. This is used for instructions like ```hlt``` where putting the exit code on the stack is quite inefficient, or instructions like ```psh``` where putting the value on the stack is literally not an option. When argumemts are specified on an instruction, it is also sometimes called an immediate.

***Please note that strings in HLLL are not like other languages. They do NOT have quotes surrounding them, and adding a space is done by using "\s" instead of a space.***

# List of instructions
The following is a list of valid HLLL instructions as of version 1.1:

```sof``` | Immediates: None; Stack inputs: None | Marks the beginning of code execution, also called the entry point. (SOF stands for start of file, but it can be put wherever you need execution to begin.) Put this later in the script if you want to make "functions" using ```flg``` without running them all at the start. ***You must have an ```sof``` in your script.***

```hlt``` | Immediates: Exit code; Stack inputs: None | Exits the program with the specified error code.

```wrt``` | Immediates: None; Stack inputs: Object to write | Pops the uppermost object from the stack and writes it to the console. *Does not add a newline.*

```wln``` | Immediates: None; Stack inputs: Object to write | Pops the uppermost object from the stack and writes it to the console. *A newline is added to the end of the line.*

```rln``` | Immediates: None; Stack inputs: None | Reads a line of user input and pushes it to the stack. *A newline is added after reading the user input. There is no instruction to prevent this.*

```stv``` | Immediates: Variable name; Stack inputs: Value | Pops the uppermost object from the stack and stores it in the specified variable. If the variable does not exist, HLLL will create it for you. Otherwise, it updates the value of the variable.

```ldv``` | Immediates: Variable name; Stack inputs: None | Reads the value from the specified variable and pushes it to the stack.

```dlv``` | Immediates: Variable name; Stack inputs: None | Deletes a variable.

```psh``` | Immediates: Object to push; Stack inputs: None | Pushes the immediate object to the stack.

```pop``` | Immediates: None; Stack inputs: None | Pops the uppermost object from the stack.

```add``` | Immediates: None; Stack inputs: Operand A, Operand B | Pops the two uppermost numbers from the stack and adds them together. Pushes the sum to the stack.

```sub``` | Immediates: None; Stack inputs: Operand A, Operand B | Pops the two uppermost numbers from the stack and subtracts B from A. Pushes the difference to the stack. This is order sensitive; the first value added to the stack is operand B and the last value added (the top of the stack) is operand A.

```mul``` | Immediates: None; Stack inputs: Operand A, Operand B | Pops the two uppermost numbers from the stack and multiplies them. Pushes the product to the stack.

```div``` | Immediates: None; Stack inputs: Operand A, Operand B | Pops the two uppermost numbers from the stack and divides A by B. Pushes the quotient to the stack. This is order sensitive; the first value added to the stack is operand B and the last value added (the top of the stack) is operand A.

```flg``` | Immediates: Flag name; Stack inputs: None  | Creates a flag which can be jumped to by ```jmp``` or any conditional branch instruction. You can name the flag whatever you want, even if a variable has already been declared with that name. (Flags and variables are stored separately.)

```jmp``` | Immediates: Target; Stack inputs: None | Jumps to the specified flag.

```bie``` | Immediates: Target; Stack inputs: Operand A, Operand B | Pops the two uppermost objects from the stack and checks if they are equal. If so, jumps to the specified target. *This is the only conditional jump that accepts strings as stack inputs.*

```big``` | Immediates: Target; Stack inputs: Operand A, Operand B | Pops the two uppermost numbers from the stack and checks if A is greater than B. If so, jumps to the specified target. This is order sensitive; the first value added to the stack is operand B and the last value added (the top of the stack) is operand A.

```bge``` | Immediates: Target; Stack inputs: Operand A, Operand B | Pops the two uppermost numbers from the stack and checks if A is greater than or equal to B. If so, jumps to the specified target. This is order sensitive; the first value added to the stack is operand B and the last value added (the top of the stack) is operand A.

```bil``` | Immediates: Target; Stack inputs: Operand A, Operand B | Pops the two uppermost numbers from the stack and checks if A is less than B. If so, jumps to the specified target. This is order sensitive; the first value added to the stack is operand B and the last value added (the top of the stack) is operand A.

```ble``` | Immediates: Target; Stack inputs: Operand A, Operand B | Pops the two uppermost numbers from the stack and checks if A is less than or equal to B. If so, jumps to the specified target. This is order sensitive; the first value added to the stack is operand B and the last value added (the top of the stack) is operand A.

```biz``` | Immediates: Target; Stack inputs: Operand | Pops the uppermost number from the stack and checks if it is equal to zero. If so, jumps to the specified target.

```nop``` | Immediates: None; Stack inputs: None | Does nothing.

# Examples
Hello, World!
```
sof
psh Hello,\sWorld!
wln
hlt 0
```
A or B?
```
flg choice
	psh A\sor\sB?
	wrt
	rln
	stv readChoice
	ldv readChoice
	psh A
	bie choiceA
	ldv readChoice
	psh B
	bie choiceB
	jmp choice
	
flg choiceA
	psh you\spicked\sa
	wln
	dlv readChoice
	hlt 1

flg choiceB
	psh you\spicked\sb
	wln
	dlv readChoice
	hlt 2

sof
jmp choice
hlt 0
```
Fibonacci Sequence
```
sof
psh 1
psh 1
stv prev
stv prev2

psh 10
stv limit
psh 3
stv iteration

psh 1
psh 1
wln
wln

flg start
ldv prev
ldv prev2
add
stv result

ldv prev
stv prev2
ldv result
stv prev

ldv result
wln

ldv iteration
psh 1
add
stv iteration
ldv limit
ldv iteration
ble start
hlt 0
```
Factorial
```
sof
psh 10
stv input

psh 1
stv currentFactorial
psh 1
stv currentCount

flg start
ldv currentFactorial
ldv currentCount
mul
stv currentFactorial

ldv currentCount
psh 1
add
stv currentCount

ldv input
ldv currentCount
ble start
ldv currentFactorial
wln
hlt 0
```
