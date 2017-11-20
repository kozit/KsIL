using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public class InstructionBase
    {

        Memory mMemory;

        public  InstructionBase(Memory memory, byte[] Parameters)
        {
            mMemory = memory;
        }

        public virtual void Run()
        {
        }

    }
}
