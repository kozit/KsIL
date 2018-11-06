using System;


namespace KsIL
{
    public class Memory
    {
        
        public static readonly int PROGRAM_RUNNING = 1;
        public static readonly int CONDITIONAL_RESULT = 2;
        public static readonly int PROGRAM_COUNT = 4;
        public static readonly int RETURN_POINTER = 9;

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
            return Buffer[Addr];
            //return Get(Addr, 1)[0];            
        }

        public byte[] Get(int Addr, int Length)
        {

            byte[] temp = new byte[Length];

            for (int i = 0; i < Length; i++)
            {

                temp[i] = Get(Addr + i);

            }

            return temp;
        }

        public byte[] GetDataPionter(int Addr)
        {

             Int32 point = BitConverter.ToInt32(Get(Addr, 4), 0);
             return GetData(point);
            
        }

        public byte[] GetData(int Addr)
        {

            return Get(Addr + 4, BitConverter.ToInt32(Get(Addr, 4), 0));

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

        public void SetData(int Addr, byte[] Value)
        {

                Set(Addr, BitConverter.GetBytes(Value.Length));
                Set(Addr + 4, Value);

        }

        internal void Destroy()
        {
            Buffer = null;
            Size = 0;
        }
    }
}
