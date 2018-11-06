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

            int ReturnPointer = BitConverter.ToInt32(mMemory.Get(Memory.RETURN_POINTER, 4), 0);         

            mMemory.Set(Memory.PROGRAM_COUNT, mMemory.Get(ReturnPointer, 4));
            mMemory.Set(Memory.RETURN_POINTER, mMemory.Get(ReturnPointer + 4, 4));
                        
        }

    }
}
