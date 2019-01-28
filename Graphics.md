
# Graphics

Code: 0x01 0x00 0x00 0x00

## Command: 0x00 (Enter Graphics Mode)

Parameters: location (8 Bytes, in memory position) Width (8 Bytes, 64int) Height (8 Bytes, 64int) Color Depth (8 Bytes, 64int)

This will set memory point "location" to location + (Width * Height * Color Depth) + 8  

## Command: 0x01 (Update Screen)

will update from the Graphics Pointer
