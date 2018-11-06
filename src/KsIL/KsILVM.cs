using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public class KsILVM
    {

        Memory memory;
        ThreadManagerBase ThreadManager;

        List<InstructionBase> code;


        public KsILVM(int size, ThreadManagerBase ThreadManager)
        {

            memory = new Memory(size);
            this.ThreadManager = ThreadManager;

        }

        public void Tick()
        {
            ThreadManager.Tick();
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

        }


    }
}
