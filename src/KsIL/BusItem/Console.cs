using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL.BusItem
{
    public class Console : IBusItem
    {
        public uint MinAddressSpace()  { return 0; }
        public enum ConsoleCodes {
            ScreenBuffer = 0x00,
            Cursor = 0x01,
            Clear = 0x02
        }

        public byte[] Read(ulong Addr, ulong Size, ulong Offset)
        {
            throw new NotImplementedException();
        }

        public byte Read(ulong Addr, ulong Offset)
        {
            return Read(Addr, 1, Offset)[0];
        }

        public byte[] ReadData(ulong Addr, ulong Offset)
        {
            return new byte[] { Read(Addr, Offset) };
        }

        public byte[] ReadData(ulong Addr, ulong Size, ulong Offset)
        {
            return Read(Addr, Size, Offset);
        }

        public void Write(ulong Addr, byte[] Data, ulong Offset)
        {
            
            System.Console.Write(Data);
        }

        public void WriteData(ulong Addr, byte[] Data, ulong Offset)
        {
            Write(Addr, Data, Offset);
        }
    }
}
