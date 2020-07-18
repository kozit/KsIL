# Parameters

## Array Item

ItemLength (4 Bytes) ItemType (1 Byte) ItemContent (Lenght as said in ItemLength)


## Array

ItemCount (4 Bytes) ArrayItems (all Array Item pointers in a row)

## pointer

Type:
0x00: absolute
0x10: relative positive
0x20: relative negative

Type (1 Byte) Value (4 Bytes)