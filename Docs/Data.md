# Data layout

| Header | DataSize / Pointer | Data |
| ------------- | ----------- | --------------- |
| bit      (0x00) | 1 Byte | Data (Value of DataSize is Size) |
| bit16    (0x01) | 2 Byte | Data (Value of DataSize is Size) |
| bit32    (0x02) | 4 Byte | Data (Value of DataSize is Size) |
| bit64    (0x03) | 8 Byte | Data (Value of DataSize is Size) |
| pointer  (0x04) | 8 Byte | NULL |
| rpointer (0x05) | 8 Byte | NULL |
