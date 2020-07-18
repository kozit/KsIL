using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KsIL.BusItem
{
    public class SystemBusItem : IBusItem
    {

        private KsIL.KsILSystem GetSystem;

        private static Dictionary<UInt64, (byte[] data, bool ReadOnly)> SystemData;

        public SystemBusItem(KsIL.KsILSystem System)
        {
            GetSystem = System;
            if (SystemData != null) return;
            SystemData = new Dictionary<ulong, (byte[] data, bool ReadOnly)>
            {
                [0x01] = (new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, false),
                [0x02] = (Encoding.ASCII.GetBytes("core"), true)
            };
        }

        private (byte[] data, bool ReadOnly) getItem(ulong Addr)
        {
            return SystemData[Addr];
        }

        public byte Read(ulong Addr, ulong Offset)
        {
            return Read(Addr, 1, Offset)[0];
        }

        public byte[] Read(ulong Addr, ulong Size, ulong Offset)
        {
            return getItem(Addr - Offset).data.Take((int)Size).ToArray();
        }

        public byte[] ReadData(ulong Addr, ulong Offset)
        {
            return getItem(Addr - Offset).data;
        }

        public byte[] ReadData(ulong Addr, ulong Size, ulong Offset)
        {
            return getItem(Addr - Offset).data.Take((int)Size).ToArray();
        }

        public void Write(ulong Addr, byte[] Data, ulong Offset)
        {
            var item = getItem(Addr - Offset);
            if (item.ReadOnly) return;
            item.data = Data;
        }

        public void WriteData(ulong Addr, byte[] Data, ulong Offset)
        {
            var item = getItem(Addr - Offset);
            if (item.ReadOnly) return;
            item.data = Data;
        }
    }
}
