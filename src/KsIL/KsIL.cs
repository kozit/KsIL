using System;
//using System.Collections.Generic;

namespace KsIL
{
    public class KsIL
    {

        public Memory memory;

        //List<InstructionBase> Code = new List<InstructionBase>();

        public KsIL(int _memory, byte[] input)
        {
            
            memory = new Memory(_memory);

            Int32 qwe = 0;
            memory.Set(0, BitConverter.GetBytes(qwe));
            memory.Set(4, BitConverter.GetBytes(qwe));

            memory.Set(9, 0x02);
            memory.Set(10, 0xFF);

            while (memory.Get(10) == 0xFF)
            {

                int Line = BitConverter.ToInt32(memory.Get(0, 4), 0);

                memory.Set(0, BitConverter.GetBytes(Line + 1));

                Console.WriteLine(memory.Get(10) + ":" + Line + ":" + (Line + 1));

                if (Line == 15)
                {
                    memory.Set(10, 0x33);
                }

                //    Code[Line].Run();

            }

        }

    }
}
