# Interrupts

# OS

Code: 0x00 0x00 0x00 0x00

Use: This is reserved for the OS

## Must Have 

## Command: 0x00 (OS Name) 

Description: Stores the OS Name in memory at the location specified no matter if it is already occupied.

Parameters: location (8 Bytes, in memory position to save name)

## Command: 0x01 (KsIL Spec version) 

Description: Stores the KsIL Spec min/max version in memory at the location specified no matter if it is already occupied.

Parameters: location (8 Bytes, in memory position to save name) min or max (1 Byte, 0x00 for min 0x01 for max)

## Command: 0x02 (Has Graphics)

Description: sets CONDITIONAL_RESULT based off it the OS suports Graphics mode

# Graphics

Code: 0x01 0x00 0x00 0x00

## Command: 0x00 (Enter Graphics Mode)

Parameters: location (8 Bytes, in memory position) Width (8 Bytes, 64int) Height (8 Bytes, 64int) Color Depth (8 Bytes, 64int)

This will set memory point "location" to location + (Width * Height * Color Depth) + 8  

## Command: 0x01 (Update Screen)

will update from the Graphics Pointer

# CPU

{!} update

Code: 0x02 0x00 0x00 0x00

## Command: 0x00 (Start Thread)
Parameters: location (8 Bytes, the line of code to start with)

## Command: 0x01 (Stop Thread)
Parameters: ThreadID (8 Bytes)

# Console

Code: 0x04 0x00 0x00 0x00

Parameters: Command (1 Byte), location (8 Bytes, in memory position)

# IO

Code: 0x05 0x00 0x00 0x00

## Command: 0x00 (Delete File)

Parameters: location (8 Bytes, in memory position)

## Command: 0x01 (WriteAllText)

Parameters: location0 (8 Bytes, in memory position) location1 (8 Bytes, in memory position)

## Command: 0x02 (WriteAllBytes)

Parameters: location0 (8 Bytes, in memory position) location1 (8 Bytes, in memory position)

## Command: 0x03 (AppendAllText)

Parameters: location0 (8 Bytes, in memory position) location1 (8 Bytes, in memory position)

## Command: 0x04 (Delete Directory)

Parameters: location (8 Bytes, in memory position)

## Command: 0x05 (Create Directory)

Parameters: location (8 Bytes, in memory position)

## Command: 0x06 (Directory Exists)

Parameters: location (8 Bytes, in memory position)

## Command: 0x07 (File Exists)

Parameters: location (8 Bytes, in memory position)
