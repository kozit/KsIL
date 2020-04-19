using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KsIL
{
    public class Bus
    {

        private Dictionary<(UInt64 lower, UInt64 upper), IBusItem> BussItem;

        public Bus() {
            BussItem = new Dictionary<(UInt64 lower, UInt64 upper), IBusItem>();
        }

        public KeyValuePair<(UInt64, UInt64), IBusItem> GetBussItem(UInt64 Addr) {
            return BussItem.Where(x => x.Key.lower <= Addr && (x.Key.upper >= Addr || x.Key.upper == 0)).First();
        }

        public void AppendBus(Bus bus)
        {
            RegisterBusItems(bus.BussItem);
        }

        public void RegisterBusItems(Dictionary<(UInt64 lower, UInt64 upper), IBusItem> Items)
        {
            foreach(KeyValuePair<(UInt64, UInt64), IBusItem> Item in Items)
                BussItem.Add(Item.Key, Item.Value);
        }

        public void RegisterBusItem(IBusItem Item, UInt64 Lower, UInt64 Upper) {
            BussItem.Add((Lower, Upper), Item);
        }

        public void WriteData(UInt64 Addr, byte[] Data) {
            KeyValuePair<(UInt64, UInt64), IBusItem> Item = GetBussItem(Addr);
            Item.Value.WriteData(Addr, Data, Item.Key.Item1);
        }

        public void WriteData(UInt64 Addr, byte Data) {
            WriteData(Addr, new byte[] { Data });
        }

        public byte[] ReadData(UInt64 Addr, UInt64 Size) {
            KeyValuePair<(UInt64, UInt64), IBusItem> Item = GetBussItem(Addr);
            return Item.Value.ReadData(Addr, Size, Item.Key.Item1);
        }

        public byte[] ReadData(UInt64 Addr) {
            KeyValuePair<(UInt64, UInt64), IBusItem> Item = GetBussItem(Addr);
            return Item.Value.ReadData(Addr, Item.Key.Item1);
        }

    }
}
