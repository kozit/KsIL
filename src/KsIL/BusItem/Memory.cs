using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL.BusItem
{
    public class Memory: IBusItem
    {
        readonly IMemory Backend;
        public Memory(IMemory backend)
        {
            Backend = backend;
        }

        public byte Read(ulong Addr, ulong Offset)
        {
            throw new NotImplementedException();
        }

        public byte[] Read(ulong Addr, ulong Size, ulong Offset)
        {
            throw new NotImplementedException();
        }

        public byte[] ReadData(UInt64 Addr, UInt64 Offset)
        {
            return Backend.GetData(Addr, Offset);
        }

        public byte[] ReadData(UInt64 Addr, UInt64 Size, UInt64 Offset)
        {
            return Backend.GetBlock(Addr, Size, Offset);
        }

        public void Write(ulong Addr, byte[] Data, ulong Offset)
        {
            throw new NotImplementedException();
        }

        public void WriteData(UInt64 Addr, byte[] Data, UInt64 Offset)
        {
            if (Data.Length == 1)
            {
                Backend.Set(Addr, Data[0], Offset);
            }
            else
            {
                Backend.SetData(Addr, Data, Offset);
            }
        }

    }
}
