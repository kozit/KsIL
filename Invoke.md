# Invoke

Code: 0x03 0x00 0x00 0x00


## Command: 0x00 (load)

Parameters: Path (UTF8 String can be any langth)

## command: 0x01 (Call)

### Parameters: 
	location (8 Bytes (UInt), in memory position of the object)
	location (8 Bytes (UInt), in memory position for result Or null)
	Method String (first 8 Bytes is length)
	Args (An arrary or None)

## command: 0x03 (New)

### Parameters: 
	location (8 Bytes, in memory position of the object Or null)
	Method String (first 8 Bytes is length)
	Args (An arrary or None)