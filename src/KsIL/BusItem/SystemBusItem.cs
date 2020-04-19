using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL.BusItem
{
    public class SystemBusItem : IBusItem
    {


        private KsIL.KsILSystem GetSystem;

        public SystemBusItem(KsIL.KsILSystem System)
        {
            GetSystem = System;
        }

        public byte Read(ulong Addr, ulong Offset)
        {
            throw new NotImplementedException();
        }

        public byte[] Read(ulong Addr, ulong Size, ulong Offset)
        {
            throw new NotImplementedException();
        }

        public byte[] ReadData(ulong Addr, ulong Offset)
        {
            throw new NotImplementedException();
        }

        public byte[] ReadData(ulong Addr, ulong Size, ulong Offset)
        {
            throw new NotImplementedException();
        }

        public void Write(ulong Addr, byte[] Data, ulong Offset)
        {
            throw new NotImplementedException();
        }

        public void WriteData(ulong Addr, byte[] Data, ulong Offset)
        {
            throw new NotImplementedException();
        }
    }
}
