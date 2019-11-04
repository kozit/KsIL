
# Graphics

Code: 0x01 0x00 0x00 0x00

## Command: 0x00 (Enter Graphics Mode)

Parameters: location (4 Bytes, in memory position) Width (4 Bytes, 32uint) Height (4 Bytes, 32uint) Color Depth (4 Bytes, 32uint)

This will set graphics memory point "location" to location with the size of (Width * Height * Color Depth) + 4 

## Command: 0x01 (Update Screen)

will update from the Graphics Pointer
