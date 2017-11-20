using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public class InstructionBase
    {

        internal Memory mMemory;

        public InstructionBase(Memory memory)
        {
            mMemory = memory;
        }

        public virtual void Run()
        {
        }

    }
}
