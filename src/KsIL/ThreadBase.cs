using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public class ThreadBase
    {

        public Int64 ID;

        Memory Memory;

        List<InstructionBase> Code;

        public ThreadBase(Int64 id, List<InstructionBase> code, Memory memory)
        {
            Code = code;
            ID = id;
            Memory = memory;
        }

        public virtual void Tick() { }

    }
}
