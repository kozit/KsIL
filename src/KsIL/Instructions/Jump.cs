using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL.Instructions
{
    public class Jump : InstructionBase
    {
        byte[] Location;
        byte Return;
        public Jump(Memory memory, byte[] Parameters) : base(memory)
        {
            Location = new byte[Parameters.Length - 2];
            Parameters.CopyTo(Location, 1);
            Return = Parameters[0];

        }

        public override void Run()
        {
            if (Return == 0x01)
            {

                int i;
                for (i = 13; i < mMemory.GetSize(); i++)
                {

                    if (mMemory.Get(i, 12) == new byte[12])
                    {
                        break;
                    }

                }

                List<byte> r = new List<byte>();

                r.AddRange(BitConverter.GetBytes(8));

                r.AddRange(mMemory.Get(4, 4));
                r.AddRange(mMemory.Get(9, 4));
                mMemory.Set(i, r.ToArray());
            }

            mMemory.Set(4, Location);

        }

    }
}
