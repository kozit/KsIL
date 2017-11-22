using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public class Interrupts
    {

        public Int16 Code;

        Memory mMemory;

        public Interrupts(Memory memory)
        {
            mMemory = memory;
        }

        public virtual void Run(byte[] Parameters)
        {
        }

    }
}
