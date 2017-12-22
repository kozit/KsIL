using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL.Instructions
{
    public class Return : InstructionBase
    {


        public Return(Memory memory, byte[] Parameters) : base(memory)
        {

            

        }

        public override void Run()
        {

            int ReturnPointer = BitConverter.ToInt32(mMemory.Get(4, 4), 0);         

            mMemory.Set(4, mMemory.Get(ReturnPointer, 4));
            mMemory.Set(9, mMemory.Get(ReturnPointer + 4, 4));

            
        }

    }
}
