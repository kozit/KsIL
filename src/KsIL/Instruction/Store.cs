using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL.Instruction
{
    public class Store : InstructionBase
    {

        int Location;
        byte[] mContent;
        public Store(Memory memory, byte[] Parameters) : base(memory)
        {

            mContent = new byte[Parameters.Length - 4];
            for (int i = 3; i != Parameters.Length; i++)
                mContent[i - 3] = Parameters[i];
            Location = BitConverter.ToInt32(Parameters, Parameters.Length - 4);

        }

        public override void Run()
        {
            //mMemory.Set(Location, BitConverter.GetBytes( mContent.Length));
            mMemory.Set(Location, mContent);
        }

    }
}
