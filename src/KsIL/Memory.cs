using System;


namespace KsIL
{
    public class Memory
    {
        
        public static readonly Int64 PROGRAM_RUNNING = 1;
        public static readonly Int64 CONDITIONAL_RESULT = 2;
        public static readonly Int64 PROGRAM_COUNT = 4;
        public static readonly Int64 RETURN_POINTER = 9;

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

        public byte Get(Int64 Addr)
        {
            return Buffer[Addr];
            //return Get(Addr, 1)[0];            
        }

        public byte[] Get(Int64 Addr, Int64 Length)
        {

            byte[] temp = new byte[Length];

            for (int i = 0; i < Length; i++)
            {

                temp[i] = Get(Addr + i);

            }

            return temp;
        }

        public byte[] GetDataPionter(Int64 Addr)
        {

             Int32 point = BitConverter.ToInt32(Get(Addr, 4), 0);
             return GetData(point);
            
        }

        public byte[] GetData(Int64 Addr)
        {

            return Get(Addr + 4, BitConverter.ToInt32(Get(Addr, 4), 0));

        }
        

        public void Set(Int64 Addr, byte[] Value)
        {
            for (int i = 0; i < Value.Length; i++)
            {

                Set(Addr + i, Value[i]);

            }
        }

        public void Set(Int64 Addr, byte Value)
        {
            Buffer[Addr] = Value;
        }

        public void SetData(Int64 Addr, byte[] Value)
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
