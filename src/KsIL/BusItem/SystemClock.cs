using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL.BusItem
{
    class SystemClock : IBusItem
    {
        public byte ReadData(UInt64 Addr, UInt64 Offset)
        {
            throw new NotImplementedException();
        }

        public byte[] ReadData(UInt64 Addr, UInt64 Size, UInt64 Offset)
        {
            throw new NotImplementedException();
        }

        public void WriteData(UInt64 Addr, byte[] Data, UInt64 Offset)
        {
            throw new NotImplementedException();
        }
    }
}
