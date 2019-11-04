# Invoke

Code: 0x03 0x00 0x00 0x00

## Command: 0x00 (load)

Description: load a DLL from a file

Parameters: Path (4 Bytes in memory position or if 6+ Bytes UTF8 String path)

## Command: 0x01 (load_mem)

Description: load a DLL from memory

Parameters: Path (4 Bytes in memory position)

## command: 0x02 (Call)

### Parameters: 
	location (4 Bytes in memory position of the object)
	location (4 Bytes in memory position for result Or null)
	Method String (first 4 Bytes is length)
	Args (An arrary or None)

## command: 0x03 (Call_Static)

### Parameters:
	location (4 Bytes in memory position for result Or null)
	Method String (first 4 Bytes is length)
	Args (An arrary or None)

## command: 0x04 (New)

### Parameters: 
	location (4 Bytes in memory position of the object Or null)
	Method String (first 4 Bytes is length)
	Args (An arrary or None)

## command: 0x05 (Compile)

### Parameters: 
	location (4 Bytes in memory position of the code to compile)
