using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public class Memory
    {
        byte[] Buffer;
        int Size;

        public Memory(int Size)
        {
            this.Size = Size;
            Buffer = new byte[Size];
        }

        public int GetSize()
        {
            return Size;
        }

        public void Clear()
        {
            for (int i = 11; i < Size; i++)
                Buffer[i] = 0;
        }

        public byte Get(int Addr)
        {
            return Get(Addr, 1)[0];            
        }

        public byte[] Get(int Addr, int Length)
        {

            byte[] temp = new byte[Length];

            for (int i = 0; i < Length; i++)
            {

                temp[i] = Buffer[i];

            }

            return temp;
        }

        public void Set(int Addr, byte[] Value)
        {
            for (int i = 0; i < Value.Length; i++)
            {

                Set(Addr + i, Value[i]);

            }
        }

        public void Set(int Addr, byte Value)
        {
            Buffer[Addr] = Value;
        }

        internal void Destroy()
        {
            Buffer = null;
            Size = 0;
        }
    }
}
