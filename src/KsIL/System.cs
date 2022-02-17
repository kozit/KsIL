using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KsIL
{
    public class KsILSystem
    {

        
        public List<CPU> CPUs { private set; get; }
        public Bus SystemBus  { private set; get; }
        public byte[] Bootloader { private set; get; }
        public UInt64 StartAddress { private set; get; }
        public Dictionary<byte[], IInstruction> Instructions { private set; get; }

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
            SystemBus.RegisterBusItem(new BusItem.Memory(new Memory.BasicFixed(4)), 130, 133);
            SystemBus.RegisterBusItem(new BusItem.Console(), 134, 139);
            if (mBus != null)
                SystemBus.AppendBus(mBus);
        }

        public void SetInstruction(Dictionary<byte[], IInstruction> Instructions) { this.Instructions = Instructions; }

        public void UseDefaultInstruction(Dictionary<byte[], IInstruction> mInstructions = null)
        {
        
            Instructions = new Dictionary<byte[], IInstruction>
            {
                { new byte[] { 0x01 }, new Instruction.Jump()   },
                { new byte[] { 0x02 }, new Instruction.Return() },
                { new byte[] { 0x05 }, new Instruction.Stack()  },
                { new byte[] { 0x06 }, new Instruction.MoveData() }
            };

            if (mInstructions != null)
                Instructions = 
                    Instructions
                    .Concat(mInstructions).GroupBy(d => d.Key)
                    .ToDictionary(d => d.Key, d => d.First().Value);
        
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
                GetBus = SystemBus,
                Instructions = Instructions
                
            });

        }

        public void StartCPU(UInt64 ID)
        {            
            CPUs[(int)ID].Start();
        }

        public void Interrupt(UInt64 ID, UInt64 PC)
        {
            
        }

    }
}
