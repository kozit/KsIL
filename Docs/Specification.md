# Execution Information
## Basic Execution Information
Instructions are separated by 0x00 0xFF 0x00 0xFF,
Parameters MUST BE AS LONG AS SPECIFIED Below, If a parameter ends in 0x00 0xFF add any byte from 0x01 to 0xFE to the end

##  Multiple Byte Parameters; how do they work?
Each non null (0x00) byte is added together. So if I said a parameter with length of 4 bytes was [0xFF 0x00 0x00 0x00] the resulting length would be 0xFF or 255.

## Handling commands (Read (0xFF) & Read Length (0xFE)) that are used instead of absolute values
When reading parameters, if 0xFF or 0xFE is read as the first byte in any parameter, that command is executed from the current point with the value that is returned being used instead of the command bytecode value.

## 0xFF, 0xFE, 0xF1 the forbidden values and the escape byte
In code 0xFF, 0xFE and 0xF1 cannot be used as absolute values if they are the first byte, instead the escape byte 0xF1 must be placed in front of them if they are absolute values.

When reading a parameter and the first byte is 0xF1 the next byte is an absolute value. This byte should be ignored and not count as a byte in the parameter.


# Memory Use
Each program should be given virtual memory of at least 10,240 bytes or more, this should be available to this program only and no others. Each variable is declared using the command store (0x01), which stores the bytes into memory. The command uses the following parameters: length (4 Bytes), content (in Bytes), location (4 Bytes) so to store the ASCII string hello world at memory position 113 (0x0D 0x00 0x00 0x00) the compiler could spit out: {todo}

Variables stored in memory are each preceded with 4 Bytes telling us the length of the data at the position. So storing 0x02 at position 0x0F would result in the memory at position 0x0F onwards being {todo}

In order to access the stored content in memory the command Read (0xFF) is used which has the parameters: location (4 bytes) so to move the string we created at location 0x06 in memory to location 0x10 the bytecode would be {todo}

## Reserved Memory
The first 100 bytes of memory are positions that are used by the executor to store vital executing state information. These can be read but should NEVER be modified. Any modification of these bytes can result in an operation exception which will cause the program to crash.


| Register Name | Description | Memory Position |
| ------------- | ----------- | --------------- |
| Program Running | If false the program will end. 0x00 (False) 0x01 (True) | 0x00 (1 Byte) |
| Code Pointer | Points to where to code for the main CPU is in memory (32int) | 0x04-0x08 (4 Bytes) |
| Graphics Pointer | Points to the Graphics interrupt memory if 0x00, 0x00, 0x00, 0x00 the not in Graphics mode (32int) | 0x14-0x17 (4 Bytes) |


# Commands

## Interrupts

Bytecode: 0x02

Mnemonic: INT

Description: Calls an Interrupt.

Parameters: Code 16int (2 Bytes), Interrupt Parameters (See Interrupts.md for Info) 


# Memory Management
## Store
Bytecode: 0x35

Mnemonic: STR

Description: Stores content in memory at the location specified no matter if it is already occupied.

Parameters: length int32 (4 Bytes), content (MUST BE AS LONG AS SPECIFIED IN length), location (4 Bytes, in memory position)

## Dynamic Store
Bytecode:0x36

Mnemonic: DST

Description: Stores content in memory at the next free destination of that length and places 4 bytes with the position it was stored in at the location specified. If no free memory is available an exception will be thrown.

Parameters: length int32 (4 Bytes), content (MUST BE AS LONG AS SPECIFIED IN length), location (4 Bytes, in memory position)

## Clear
Bytecode: 0x33

Mnemonic: CLR

Description: Clears (nulls to 0x00) the memory content at the position specified and marks it for future use by the dynamic memory functions.

Parameters: location (4 Bytes, in memory position)

## Dynamic Clear
Bytecode: 0x34

Mnemonic: DCL

Description: Clears (nulls to 0x00) the memory content at the position specified and marks it for future use by the dynamic memory functions.

Parameters: location int32 (4 Bytes, in memory position)


# Conditional Tests
For all conditional commands the Conditional Result byte in Reserved Memory is set according to the result of the conditional test.

## Test Equal
Bytecode: 0x11

Mnemonic: TEQ

Description: Tests if the two parameters are equal and if they are the result is true.

Parameters: location 1 (4 Bytes, in memory position), location 2 (4 Bytes, in memory position)

## Test Greater Than
Bytecode: 0x12

Mnemonic: TGT

Description: Tests if byte1 is greater than byte 2.

Parameters: location 1 (4 Bytes, in memory position), location 2 (4 Bytes, in memory position)

## Jump
Bytecode: 0x51

Mnemonic: JMP

Description: Jump to the position specified.

Parameters: position (4 Bytes, in position of execution)

## Call
Bytecode: 0x53

Mnemonic: CAL

Description: Jump to the position specified and stores in return pointer.

Parameters: position (4 Bytes, in position of execution)

## Return
Bytecode: 0x54

Mnemonic: RTN

Description: Jump to the position specified in the next return position and set the return pointer to the next return or 0x00 0x00 0x00 0x00 if none.

Parameters: none


# Arithmetic Operations
## Add
Bytecode: 0x21

Mnemonic: ADD

Description: Adds SOURCE to DESTINATION and puts the result in DESTINATION (DESTINATION = DESTINATION + SOURCE)

Parameters: source is memory position (1 Bytes, 0x01 for true, 0x00 for false), source (4 Bytes, in memory location), destination is memory position, (1 Bytes, 0x01 for true, 0x00 for false), destination (4 Bytes, memloc or absolute value)

## Subtract
Bytecode: 0x22

Mnemonic: SUB

Description: Subtracts SOURCE from DESTINATION and puts the result in DESTINATION (DESTINATION = DESTINATION - SOURCE)

Parameters: source is memory position (1 Bytes, 0x01 for true, 0x00 for false), source (4 Bytes, in memory location), destination is memory position, (1 Bytes, 0x01 for true, 0x00 for false), destination (4 Bytes, memloc or absolute value)


## Multiply
Bytecode: 0x24

Mnemonic: MUL

Description: Multiplys SOURCE from DESTINATION and puts the result in DESTINATION (DESTINATION = DESTINATION * SOURCE)

Parameters: source is memory position (1 Bytes, 0x01 for true, 0x00 for false), source (4 Bytes, in memory location), destination is memory position, (1 Bytes, 0x01 for true, 0x00 for false), destination (4 Bytes, memloc or absolute value)

## Divide
Bytecode: 0x23

Mnemonic: DIV

Description: Divides SOURCE from DESTINATION and puts the result in DESTINATION (DESTINATION = DESTINATION / SOURCE)

Parameters: source is memory position (1 Bytes, 0x01 for true, 0x00 for false), source (4 Bytes, in memory location), destination is memory position, (1 Bytes, 0x01 for true, 0x00 for false), destination (4 Bytes, memloc or absolute value)
