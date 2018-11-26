using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public class KsILVM
    {

        Memory memory;
        ThreadManagerBase ThreadManager;
        

        List<Interrupt> Interrupts;

        public KsILVM(int size, ref ThreadManagerBase ThreadManager, List<Interrupt> Interrupts = null)
        {


            
            memory = new Memory(size);
            this.ThreadManager = ThreadManager;
            this.ThreadManager.LoadMemory(memory);
            

            if (Interrupts == null)
            {
                Interrupts = Interrupt.Default;
            }

            this.Interrupts = Interrupts;
            this.Interrupts.Add(new Interrupts.Threading(this.ThreadManager));


            this.ThreadManager.StartThread(this.ThreadManager.AddThread(0));

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


            //ThreadManager.LoadCode(code);
        }


    }
}
