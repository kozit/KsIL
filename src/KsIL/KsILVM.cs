using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KsIL
{
    public class KsILVM
    {

        internal Memory memory;

        internal Dictionary<UInt16, Interrupt> Interrupts;

        internal List<CPU> cpu;

        public KsILVM(int size, Dictionary<UInt16, Interrupt> Interrupts = null)
        {

            cpu = new List<CPU>();
            
            memory = new Memory((uint)size, this);
            

            if (Interrupts == null)
            {
                Interrupts = Interrupt.Default;
            }

            this.Interrupts = Interrupts;

            memory.Set(Memory.PROGRAM_RUNNING, 0x01);


        }

        public void Tick()
        {

            Debugger.Log("Start", "Tick");

            foreach (CPU cpu in this.cpu)
                cpu.Tick();

            Debugger.Log("End", "Tick");
        }

        public void AutoTick()
        {

            Debugger.Log(memory.Get(Memory.PROGRAM_RUNNING), "AutoTick");

            while (memory.Get(Memory.PROGRAM_RUNNING) == 0x01)
            {
                Tick();
            }

        }

        public byte[] memDump()
        {

            return memory.Get(0, memory.GetSize());

        }

        public void Load_File(string Path, int point = 0, uint pointer = 4)
        {

            Debugger.Log(Path, "File Path");

            Load_Code(File.ReadAllBytes(Path), point, pointer);

        }

        public void Load_Code(byte[] ByteCode, int point = 0, uint pointer = 4)
        {

            Debugger.Log("loading code");
            Debugger.Log(ByteCode);
            memory.SetData(pointer, ByteCode);

            Debugger.Log("loaded code");

            StartCPU(pointer);
            
        }

        public void StartCPU(uint pointer = 4, uint point = 0)
        {

            CPU temp = new CPU(this, memory, pointer) { InstructionPoint = point };

            cpu.Add(temp);

        }


    }
}
