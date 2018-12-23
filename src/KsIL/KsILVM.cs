using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public class KsILVM
    {

        private Memory memory;

        private List<Interrupt> Interrupts;

        private List<CPU> cpu;

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

        public void Load(string Path)
        {

            Load(System.IO.File.ReadAllBytes(Path));

        }

        public void Load(byte[] ByteCode)
        {
                       
            memory.SetDataPionter(4, ByteCode);
                       
            cpu.Add(new CPU(this, memory));
                       
        }


    }
}
