using System;
using System.Collections.Generic;

namespace KsIL
{
    public class Memory
    {
        
        public static readonly Int64 PROGRAM_RUNNING = 1;
        public static readonly Int64 CONDITIONAL_RESULT = 2;
        public static readonly Int64 PROGRAM_COUNT = 10;
        public static readonly Int64 RETURN_POINTER = 19;
        public static readonly Int64 GRAPHICS_POINTER = 28;
        public static readonly Int64 THREAD_POINTER = 37;


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

             Int64 point = BitConverter.ToInt64(Get(Addr, 8), 0);
             return GetData(point);
            
        }

        public byte[] GetData(Int64 Addr)
        {

            return Get(Addr + 8, BitConverter.ToInt64(Get(Addr, 8), 0));

        }

        public byte[] GetArray(Int64 Addr, Int64 index)
        {
            Int64 pos;

            byte[] data = GetDataPionter(Addr);
            pos = BitConverter.ToInt64(data, data.Length - 9);

            for (int i = 1; i < index - 1; i++)
            {

                data = GetDataPionter(pos);
                pos = BitConverter.ToInt64(data, data.Length - 9);

            }

            return GetDataPionter(pos);

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

        public Int64 SetDataPionter(Int64 Addr, byte[] Value)
        {

            Int64 i;
            for (i = 100; i < GetSize(); i++)
            {

                if (i + Value.Length >= Addr)
                {

                    i = Addr + 8;
                    continue;

                }

                if (Get(i, Value.Length) == new byte[Value.Length])
                {
                    break;
                }

            }

            Set(Addr, BitConverter.GetBytes(i));
            SetData(i, Value);
            return i;
        }

        public void SetData(Int64 Addr, byte[] Value)
        {

                Set(Addr, BitConverter.GetBytes(Value.Length));
                Set(Addr + 8, Value);

        }
                
        public void SetArray(Int64 Addr, byte[] Value, Int64 index)
        {
            Int64 prepos;
            Int64 pos;
            Int64 nextpos;
            byte[] data = GetDataPionter(Addr);
            pos = BitConverter.ToInt64(data, data.Length - 9);
            prepos = -1;

            for (int i = 1; i < index - 1; i++)
            {

                data = GetDataPionter(pos);
                prepos = pos;
                pos = BitConverter.ToInt64(data, data.Length - 9);

            }

            data = GetDataPionter(pos);
            nextpos = BitConverter.ToInt64(data, data.Length - 9);

            List<byte> mValue = new List<byte>();
            mValue.AddRange(Value);
            mValue.AddRange(BitConverter.GetBytes(nextpos));

            pos = SetDataPionter(prepos + GetData(prepos).Length - 9, mValue.ToArray());
                                 

        }

        internal void Destroy()
        {
            Buffer = null;
            Size = 0;
        }
    }
}
