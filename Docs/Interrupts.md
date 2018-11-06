# Interrupts

## OS

Code: 0x00 0x00 0x00 0x00

Use: This is reserved for the OS

## Graphics

Code: 0x01 0x00 0x00 0x00

# Command: 0x00 (Enter Graphics Mode)

Parameters: location (8 Bytes, in memory position)

# Command: 0x01 (Update Screen)

## Console

Code: 0x04 0x00 0x00 0x00

Parameters: Command (1 Byte), location (8 Bytes, in memory position)

## IO

Code: 0x05 0x00 0x00 0x00

# Command: 0x00 (Delete File)

Parameters: location (8 Bytes, in memory position)

# Command: 0x01 (WriteAllText)

Parameters: location0 (8 Bytes, in memory position) location1 (8 Bytes, in memory position)

# Command: 0x02 (WriteAllBytes)

Parameters: location0 (8 Bytes, in memory position) location1 (8 Bytes, in memory position)

# Command: 0x03 (AppendAllText)

Parameters: location0 (8 Bytes, in memory position) location1 (8 Bytes, in memory position)

# Command: 0x04 (Delete Directory)

Parameters: location (8 Bytes, in memory position)

# Command: 0x05 (Create Directory)

Parameters: location (8 Bytes, in memory position)

# Command: 0x06 (Directory Exists)

Parameters: location (8 Bytes, in memory position)

# Command: 0x07 (File Exists)

Parameters: location (8 Bytes, in memory position)
