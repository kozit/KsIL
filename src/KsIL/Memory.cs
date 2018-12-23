using System;
using System.Collections.Generic;

namespace KsIL
{
    public class Memory
    {
               
        public static readonly int PROGRAM_RUNNING = 1;
        public static readonly int GRAPHICS_POINTER = 2;
        
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
            Buffer = new byte[Size];
        }

        public byte Get(int Addr)
        {
            return Buffer[Addr];          
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

            int point = BitConverter.ToInt32(Get(Addr, 8), 0);
            return GetData(point);
            
        }

        public byte[] GetData(int Addr)
        {

            return Get(Addr + 8, BitConverter.ToInt32(Get(Addr, 8), 0));

        }

        public byte[] GetArray(int Addr, int index)
        {
            int pos;

            byte[] data = GetDataPionter(Addr);
            pos = BitConverter.ToInt32(data, data.Length - 9);

            for (int i = 1; i < index - 1; i++)
            {

                data = GetDataPionter(pos);
                pos = BitConverter.ToInt32(data, data.Length - 9);

            }

            data = GetDataPionter(pos);
            byte[] output = new byte[data.Length - 9];
            for (int i = 0; i < data.Length - 9; i++)
                output[i] = data[i];

            return output;
            
        }


        public void Set(int Addr, byte Value)
        {
            Buffer[Addr] = Value;
        }

        public void Set(int Addr, byte[] Value)
        {
            for (int i = 0; i < Value.Length; i++)
            {

                Set(Addr + i, Value[i]);

            }
        }

        public int SetDataPionter(int Addr, byte[] Value)
        {

            int i;
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

        public void SetData(int Addr, byte[] Value)
        {

                Set(Addr, BitConverter.GetBytes(Value.Length));
                Set(Addr + 8, Value);

        }
                
        public void SetArray(int Addr, byte[] Value, int index)
        {
            int prepos;
            int pos;
            int nextpos;
            byte[] data = GetDataPionter(Addr);
            pos = BitConverter.ToInt32(data, data.Length - 9);
            prepos = 0;

            for (int i = 1; i < index - 1; i++)
            {

                data = GetDataPionter(pos);
                prepos = pos;
                pos = BitConverter.ToInt32(data, data.Length - 9);

            }

            data = GetDataPionter(pos);
            nextpos = BitConverter.ToInt32(data, data.Length - 9);

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
