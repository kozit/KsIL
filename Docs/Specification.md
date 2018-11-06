# Execution Information
## Basic Execution Information
The first byte is used to pick the memory mode,
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
Each program should be given virtual memory of at least 10,240 bytes or more, this should be available to this program only and no others. Each variable is declared using the command store (0x01), which stores the bytes into memory. The command uses the following parameters: length (8 Bytes), content (in Bytes), location (8 Bytes) so to store the ASCII string hello world at memory position 113 (0x0D 0x00 0x00 0x00) the compiler could spit out: {todo}

Variables stored in memory are each preceded with 8 Bytes telling us the length of the data at the position. So storing 0x02 at position 0x0F would result in the memory at position 0x0F onwards being {todo}

In order to access the stored content in memory the command Read (0xFF) is used which has the parameters: location (8 bytes) so to move the string we created at location 0x06 in memory to location 0x10 the bytecode would be {todo}

## Reserved Memory
The first 100 bytes of memory are positions that are used by the executor to store vital executing state information. These can be read but should NEVER be modified. Any modification of these bytes can result in an operation exception which will cause the program to crash.


| Register Name | Description | Memory Position |
| ------------- | ----------- | --------------- |
| Unused | this is un used | 0x00 (1 Byte) |
| Program Running | If false the program will end. 0x00 (False) 0x01 (True) | 0x01 (1 Byte) |
| Conditional Result | The result of the conditional test if it has just been completed. 0x00 (False) 0x01 (True) 0x02 (Not Set) | 0x02-0x03 (1 Byte) |
| Unused | this is un used | 0x04-0x09 |
| Program Counter | The position in the program of the next command to be processed relative to the start of the program if not used if Thread Pointer is set. (64int) | 0x0A-0x12 (8 Bytes) |
| Return Pointer | Points to the next return position in memory in the program of the next return. (64int) | 0x13-0x1A (8 Bytes) |
| Graphics Pointer | Points to the Graphics interrupt memory (64int) | 0x1B-0x22 (8 Bytes) |
| Thread Pointer | Like Program Counter but in a array format only use if there is more then one Thread (64Bit) | 0x23-2A (8Bytes) |
| Unused | this is un used | 0x2B-0x64 |


# Commands

## Interrupts

Bytecode: 0x00

Mnemonic: INT

Description: Calls an Interrupt.

Parameters: Code 16int (2 Bytes), Interrupt Parameters (See Interrupts.md for Info) 


# Memory Management
## Store
Bytecode: 0x01

Mnemonic: STR

Description: Stores content in memory at the location specified no matter if it is already occupied.

Parameters: length 64int (8 Bytes), content (MUST BE AS LONG AS SPECIFIED IN length), location (8 Bytes, in memory position)

## Dynamic Store
Bytecode:0x02

Mnemonic: DST

Description: Stores content in memory at the next free destination of that length and places 4 bytes with the position it was stored in at the location specified. If no free memory is available an exception will be thrown.

Parameters: length 64int (8 Bytes), content (MUST BE AS LONG AS SPECIFIED IN length), location (8 Bytes, in memory position)

## Read
Bytecode: 0xFF

Mnemonic: [p1,p2,p3,p4] converts into a read command

Description: Reads content in memory (can only be used in place of an absolute value)

Parameters: location (8 Bytes, in memory position)

## Read Length
Bytecode: 0xFE

Mnemonic: (p1,p2,p3,p4) converts into a read length

Description: Reads the length of the memory content as a two byte value using the protocol above

Parameters: location (8 Bytes, in memory position)

## Bytecode 2 Bit
Bytecode: 0xFD

Mnemonic: (p1,p2) converts into a 2 Bit long Bytecode

Description: Perfix for 2 bit Byte codes

Parameters: Bytecode (2 Bytes)

## Read Into
Bytecode: 0x03

Mnemonic: RDI

Description: Reads the content of the memory at location 1 into location 2. Even if location 2 is already occupied.

Parameters: location 1 (8 Bytes, in memory position), location 2 (8 Bytes, in memory position)

## Dynamic Read Into
Bytecode: 0x04

Mnemonic: DRI

Description: Reads the content of the memory at location 1 into the next available memory of that length, the position of the new memory is stored as 8 bytes at position 2.

Parameters: location 1 (8 Bytes, in memory position), location 2 (8 Bytes, in memory position)

## Fill
Bytecode:0x05

Mnemonic: FLL

Description: Fills the memory from location 1 to location 2 with the byte specified.

Parameters: location 1 (8 Bytes, in memory position), location 2 (8 Bytes, in memory position), byte (1 Bytes)

## Clear
Bytecode: 0x06

Mnemonic: CLR

Description: Clears (nulls to 0x00) the memory content at the position specified and marks it for future use by the dynamic memory functions.

Parameters: location (8 Bytes, in memory position)

# Conditional Tests
For all conditional commands the Conditional Result byte in Reserved Memory is set according to the result of the conditional test.

## Test Equal
Bytecode: 0x10

Mnemonic: TEQ

Description: Tests if the two parameters are equal and if they are the result is true.

Parameters: location 1 (8 Bytes, in memory position), location 2 (8 Bytes, in memory position)

## Test Greater Than
Bytecode: 0x11

Mnemonic: TGT

Description: Tests if byte1 is greater than byte 2.

Parameters: location 1 (8 Bytes, in memory position), location 2 (8 Bytes, in memory position)

## Jump If True
Bytecode: 0x12

Mnemonic: JIT

Description: If the previous conditional test result was true jump to the position specified.

Parameters: position (8 Bytes, in position of execution)

## Jump If False
Bytecode: 0x13

Mnemonic: JIF

Description: If the previous conditional test result was false jump to the position specified.

Parameters: position (8 Bytes, in position of execution)

## Jump
Bytecode: 0x20

Mnemonic: JMP

Description: Jump to the position specified.

Parameters: add return position (1 Bytes, 0x01 for true, 0x00 for false), position (8 Bytes, in position of execution)


## Return
Bytecode: 0x21

Mnemonic: RTN

Description: Jump to the position specified in the next return position and set the return pointer to the next return or 0x00 0x00 0x00 0x00 if none.

Parameters: none


# Arithmetic Operations
## Add
Bytecode: 0x30

Mnemonic: ADD

Description: Adds SOURCE to DESTINATION and puts the result in DESTINATION (DESTINATION = DESTINATION + SOURCE)

Parameters: source is memory position (1 Bytes, 0x01 for true, 0x00 for false), source (8 Bytes, in memory location), destination is memory position, (1 Bytes, 0x01 for true, 0x00 for false), destination (8 Bytes, memloc or absolute value)

## Subtract
Bytecode: 0x31

Mnemonic: SUB

Description: Subtracts SOURCE from DESTINATION and puts the result in DESTINATION (DESTINATION = DESTINATION - SOURCE)

Parameters: source is memory position (1 Bytes, 0x01 for true, 0x00 for false), source (8 Bytes, in memory location), destination is memory position, (1 Bytes, 0x01 for true, 0x00 for false), destination (8 Bytes, memloc or absolute value)


## Multiply
Bytecode: 0x32

Mnemonic: MUL

Description: Multiplys SOURCE from DESTINATION and puts the result in DESTINATION (DESTINATION = DESTINATION * SOURCE)

Parameters: source is memory position (1 Bytes, 0x01 for true, 0x00 for false), source (8 Bytes, in memory location), destination is memory position, (1 Bytes, 0x01 for true, 0x00 for false), destination (8 Bytes, memloc or absolute value)

## Divide
Bytecode: 0x33

Mnemonic: DIV

Description: Divides SOURCE from DESTINATION and puts the result in DESTINATION (DESTINATION = DESTINATION / SOURCE)

Parameters: source is memory position (1 Bytes, 0x01 for true, 0x00 for false), source (8 Bytes, in memory location), destination is memory position, (1 Bytes, 0x01 for true, 0x00 for false), destination (8 Bytes, memloc or absolute value)
