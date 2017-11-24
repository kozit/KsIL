using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public class Interrupt
    {

        public Int16 Code;

        Memory mMemory;

        public Interrupt(Memory memory)
        {
            mMemory = memory;
        }

        public virtual void Run(byte[] Parameters)
        {
        }

    }
}
