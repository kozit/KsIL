using System;

namespace KsIL
{
    public class KsIL
    {

        public Memory memory;

        byte[] Code;

        public KsIL(int _memory, byte[] input)
        {
            Code = input;
            memory = new Memory(_memory);

            Int32 qwe = 0;
            memory.Set(0, BitConverter.GetBytes(qwe));
            memory.Set(4, BitConverter.GetBytes(qwe));

            memory.Set(9, 0x02);

        }

    }
}
