using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public class KsILSystem
    {

        public List<CPU> CPUs { private set; get; }
        public Bus SystemBus  { private set; get; }
        public byte[] Bootloader { private set; get; }
        public UInt64 StartAddress { private set; get; }

        public KsILSystem()
        {
            CPUs = new List<CPU>();
        }

        public void StartSystem()
        {
            AddCPU(StartAddress);
            StartCPU(0);
        }
        
        public void SetBus(Bus bus) { SystemBus = bus; }

        public void UseDefaultBus(Bus mBus = null)
        {
            SystemBus = new Bus();
            SystemBus.RegisterBusItem(new BusItem.SystemBusItem(this), 0, 49);
            SystemBus.RegisterBusItem(new BusItem.Memory(new Memory.BasicFixed(50)), 50, 99);
            SystemBus.RegisterBusItem(new BusItem.SystemClock(), 100, 129);
            if(mBus != null)
                SystemBus.AppendBus(mBus);
        }

        public void SetBootloader(byte[] Bootloader) { 
            this.Bootloader = Bootloader;
            this.SystemBus.WriteData(StartAddress, Bootloader);
        }

        public void SetStartAddress(UInt64 StartAddress) { this.StartAddress = StartAddress; }

        public void AddCPU(UInt64 PC)
        {

            CPUs.Add(new CPU()
            {
                PC = PC,
                GetBus = SystemBus
            });

        }

        public void StartCPU(UInt64 ID)
        {
            
            CPUs[(int)ID].Tick();
        }

    }
}
