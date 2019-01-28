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
