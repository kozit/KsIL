# Interrupts

## Invoke

Code: 0x00 0x00 0x00 0x00

Parameters: return type (1 Byte), length0 32int (4 Bytes), Class Path (MUST BE AS LONG AS SPECIFIED IN length0), ConstructorArgs String Array, length1 32int (4 Bytes), Method Name (MUST BE AS LONG AS SPECIFIED IN length1), MethodArgs String Array, location (4 Bytes, in memory position, only set if needed by return type)
