using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public interface IBusItem
    {

        byte[] ReadData(UInt64 Addr, UInt64 Offset);
        byte[] ReadData(UInt64 Addr, UInt64 Size, UInt64 Offset);
        byte Read(UInt64 Addr, UInt64 Offset);
        byte[] Read(UInt64 Addr, UInt64 Size, UInt64 Offset);
        void WriteData(UInt64 Addr, byte[] Data, UInt64 Offset);
        void Write(UInt64 Addr, byte[] Data, UInt64 Offset);

    }
}
