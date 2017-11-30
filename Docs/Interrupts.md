# Interrupts

## Invoke

Code: 0x00 0x00 0x00 0x00

Parameters: return type (1 Byte), length0 32int (4 Bytes), Class Path (MUST BE AS LONG AS SPECIFIED IN length0), ConstructorArgs String Array, length1 32int (4 Bytes), Method Name (MUST BE AS LONG AS SPECIFIED IN length1), MethodArgs String Array, location (4 Bytes, in memory position, only set if needed by return type)

## Console

Code: 0x01 0x00 0x00 0x00

Parameters: Command (1 Byte), location (4 Bytes, in memory position)

## IO

Code: 0x02 0x00 0x00 0x00

# Command: 0x00 (Delete File)

Parameters: location (4 Bytes, in memory position)

# Command: 0x01 (WriteAllText)

Parameters: location0 (4 Bytes, in memory position) location1 (4 Bytes, in memory position)

# Command: 0x02 (WriteAllBytes)

Parameters: location0 (4 Bytes, in memory position) location1 (4 Bytes, in memory position)

# Command: 0x03 (AppendAllText)

Parameters: location0 (4 Bytes, in memory position) location1 (4 Bytes, in memory position)

# Command: 0x04 (Delete Directory)

Parameters: location (4 Bytes, in memory position)

# Command: 0x05 (Create Directory)

Parameters: location (4 Bytes, in memory position)

# Command: 0x06 (Directory Exists)

Parameters: location (4 Bytes, in memory position)

# Command: 0x07 (File Exists)

Parameters: location (4 Bytes, in memory position)
