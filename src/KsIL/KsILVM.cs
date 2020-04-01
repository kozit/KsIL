using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using System.Threading;

namespace KsIL
{
    public class KsILVM
    {

        internal Memory memory;

        internal Dictionary<UInt16, Interrupt> Interrupts;

        internal List<CPU> cpu;

        public Dictionary<UInt16, List<(CPU CPU, uint CodePoint)>> Interrupt_CallBack = new Dictionary<UInt16, List<(CPU CPU, uint CodePoint)>>();

        public volatile bool isRunning = false;

        public KsILVM(int size, Dictionary<UInt16, Interrupt> Interrupts = null)
        {

            cpu = new List<CPU>();
            
            memory = new Memory((uint)size, this);
 
            if (Interrupts == null)
            {
                Interrupts = Interrupt.Default;
            }

            this.Interrupts = Interrupts;

            memory.Set(Memory.Pointer.PROGRAM_RUNNING, 0x01);


        }

        public void Start()
        {

            memory.Set(Memory.Pointer.PROGRAM_RUNNING, 0x01);

        }

        public void Stop()
        {

            memory.Set(Memory.Pointer.PROGRAM_RUNNING, 0x00);

        }

        public byte[] memDump()
        {

            return memory.Get(1, memory.GetSize());

        }

        public void Load_File(string Path, int point = 0)
        {

            Debugger.Log(Path, "File Path");

            Load_Code(File.ReadAllBytes(Path), point);

        }

        public void Load_Code(byte[] ByteCode, int point = 0)
        {
            uint pointer = memory.GetSize() - 16;
            Debugger.Log("loading code", "MainCPU");
            Debugger.Log(ByteCode, "MainCPU");
            memory.SetDataPionter(pointer, (uint)(memory.GetSize() - ByteCode.Length - 64), ByteCode);

            Debugger.Log("loaded code", "MainCPU");

            StartCPU(pointer);
            
        }

        public void StartCPU(uint pointer = 4, uint point = 0)
        {

            CPU temp = new CPU(this, memory, pointer) { InstructionPoint = point };
            
            cpu.Add(temp);

        }


    }
}
