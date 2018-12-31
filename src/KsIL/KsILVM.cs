using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public class KsILVM
    {

        internal Memory memory;

        internal List<Interrupt> Interrupts;

        internal List<CPU> cpu;

        public KsILVM(int size, List<Interrupt> Interrupts = null)
        {

            cpu = new List<CPU>();
            
            memory = new Memory(size);
            

            if (Interrupts == null)
            {
                Interrupts = Interrupt.Default;
            }

            this.Interrupts = Interrupts;

        }

        public void Tick()
        {

            foreach (CPU cpu in this.cpu)
                cpu.Tick();

        }

        public void AutoTick()
        {

            while (memory.Get(Memory.PROGRAM_RUNNING) == 0x01)
            {
                Tick();
            }

        }

        public byte[] memDump()
        {

            return memory.Get(0, memory.GetSize());

        }

        public void Load(string Path, int point = 0, int pointer = 4)
        {

            Load(System.IO.File.ReadAllBytes(Path), point, pointer);

        }

        public void Load(byte[] ByteCode, int point = 0, int pointer = 4)
        {
                       
            memory.SetDataPionter(pointer, ByteCode);

            StartCPU(pointer);
            
        }

        public void StartCPU(int pointer = 4, int point = 0)
        {

            cpu.Add(new CPU(this, memory, pointer) { InstructionPoint = point});

        }


    }
}
