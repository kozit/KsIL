using System;
using System.Collections.Generic;

namespace KsIL.Instructions
{
    public class JumpIf : InstructionBase
    {
        byte[] Location;
        bool mtesttrue;
        public JumpIf(Memory memory, byte[] Parameters, bool testtrue) : base(memory)
        {

            Location = Parameters;
            mtesttrue = testtrue;

        }

        public override void Run()
        {

            if (mtesttrue == BitConverter.ToBoolean(mMemory.Get(2,1), 0))
            {
                
                if (Location.Length == 5)
                {
                    Location = Utill.Read(Location, mMemory);
                }


                mMemory.Set(4, Location);

            }
        }

    }
}
