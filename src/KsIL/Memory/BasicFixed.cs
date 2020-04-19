using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL.Memory
{
    class BasicFixed : IMemory
    {

        private byte[] Data;
        public BasicFixed(UInt64 Size)
        {
            Data = new byte[Size];
        }

        public byte Get(UInt64 address, UInt64 offset)
        {
            return Data[address - offset];
        }

        public byte[] GetBlock(UInt64 address, UInt64 size, UInt64 offset)
        {
            byte[] output = new byte[size];
            Array.Copy(Data, (int)(address - offset), output, 0, (int)size);
            return output;
        }

        public byte[] GetData(UInt64 address, UInt64 offset)
        {
            return GetBlock(address + 4, BitConverter.ToUInt64(GetBlock(address, 4, offset)), offset);
        }

        public void Set(UInt64 address, byte data, UInt64 offset)
        {
            Data[address - offset] = data;
        }

        public void SetBlock(UInt64 address, byte[] data, UInt64 offset)
        {
            Array.Copy(data, 0, Data, (int)(address - offset), data.Length);
        }

        public void SetData(UInt64 address, byte[] data, UInt64 offset)
        {
            SetBlock(address, BitConverter.GetBytes(data.Length), offset);
            SetBlock(address + 4, data, offset);
        }
    }
}
