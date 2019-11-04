# CPU

Code: 0x02 0x00 0x00 0x00

## Command: 0x01 (Stop Thread)

Description: stops a CPU by ID

Parameters: ID (4 Bytes CPU ID)

## Command: 0x02 (Start Thread)

Description: starts a CPU by ID

Parameters: ID (4 Bytes CPU ID)

## Command: 0x03 (Make Thread)

Description: make a CPU

Parameters: Code point (4 Bytes in memory position), ID (4 Bytes in memory position or none)

## Command: 0x04 (Kill Thread)

Description: kill a CPU by ID

Parameters: ID (4 Bytes CPU ID)