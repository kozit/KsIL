# Execution Information

Spec:3

## Basic Execution Information

Instructions are separated by 0x00 0xFF 0x00 0xFF,

## Memory Use

Each program should be given virtual memory of at least 10,240 bytes or more, this should be available to this program only and no others

Data stored in memory follows [Data](Data.md)

## Reserved Memory

The first 200 bytes of memory are positions that are used by the executor to store vital executing state information.

| Register Name | Description | Memory Position |
| ------------- | ----------- | --------------- |
| Program Running | If false the program will end. 0x00 (False) 0x01 (True) | 0x00 (1 Byte) |
| Reserved | Reserved | 0x01 - 0x07 (7 Bytes) |
| Video Memory Pointer | a pointer to the Data point of the Vidoe memory | 0x08 - 0x12 (8 Bytes) |
| Reserved | Reserved | 0x13 - 0x64 (84 Bytes) |
