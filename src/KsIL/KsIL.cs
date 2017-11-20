using System;
using System.Collections.Generic;

namespace KsIL
{
    public class KsIL
    {

        public Memory memory;

        List<InstructionBase> Code = new List<InstructionBase>();

        public KsIL(int _memory, byte[] input)
        {
            
            memory = new Memory(_memory);

            Int32 qwe = 0;

            // Memory Mode 0x00 (16bit), 0x01 (32 bit), 0x02 (64 bit)
            memory.Set(0, 0x01);
            // Is program running
            memory.Set(1, 0x01);
            // Conditional Result
            memory.Set(2, 0x00);
            // Program Counter
            memory.Set(4, BitConverter.GetBytes(qwe));
            //Return Pointer
            memory.Set(9, BitConverter.GetBytes(qwe));
            

            
            while (memory.Get(1) == 0x01)
            {

                int Line = BitConverter.ToInt32(memory.Get(4, 4), 0);

                memory.Set(4, BitConverter.GetBytes(Line + 1));

                
                if (Line >= Code.Count)
                {
                    memory.Set(1, 0x00);
                    continue;
                }

                Code[Line].Run();

            }

        }

    }
}
