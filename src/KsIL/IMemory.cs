using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public interface IMemory
    {

        byte Get(UInt64 address, UInt64 Offset);
        byte[] GetBlock(UInt64 address, UInt64 size, UInt64 Offset);
        byte[] GetData(UInt64 address, UInt64 Offset);

        void Set(UInt64 address, byte data, UInt64 Offset);
        void SetBlock(UInt64 address, byte[] data, UInt64 Offset);
        void SetData(UInt64 address, byte[] data, UInt64 Offset);

    }
}
