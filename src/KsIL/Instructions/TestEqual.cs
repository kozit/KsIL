using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL.Instructions
{
    public class TestEqual : InstructionBase
    {

        int Location0;
        int Location1;

        public TestEqual(Memory memory, byte[] Parameters) : base(memory)
        {

            Location0 = BitConverter.ToInt32(Parameters, 0);
            Location1 = BitConverter.ToInt32(Parameters, 4);

        }

        public override void Run()
        {

            if (mMemory.GetData(Location0) == mMemory.GetData(Location1))
            {
                Console.WriteLine("1");
                mMemory.Set(2, 0x01);
            }
            else
            {
                Console.WriteLine("2");
                mMemory.Set(2, 0x00);
            }

            Console.WriteLine(Location0 + ":" + Location1);
            System.IO.File.WriteAllBytes("1.txt", mMemory.GetData(Location0));

            System.IO.File.WriteAllBytes("2.txt", mMemory.GetData(Location1));
        }

    }
}
