using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL.Memory
{
    class ROM : IMemory
    {

        private byte[] Data;
        public ROM(byte[] Data)
        {
            this.Data = Data;
        }

        public byte Get(UInt64 address, UInt64 Offset)
        {
            return Data[address - Offset];
        }

        public byte[] GetBlock(UInt64 address, UInt64 size, UInt64 Offset)
        {
            byte[] output = new byte[size];
            Array.Copy(Data, (int)(address - Offset), output, 0, (int)size);
            return output;
        }

        public byte[] GetData(UInt64 address, UInt64 Offset)
        {
            return GetBlock(address + 4, BitConverter.ToUInt64(GetBlock(address, 4, Offset)), Offset);
        }

        public void Set(UInt64 address, byte data, UInt64 Offset)
        {            
        }

        public void SetBlock(UInt64 address, byte[] data, UInt64 Offset)
        {            
        }

        public void SetData(UInt64 address, byte[] data, UInt64 Offset)
        {            
        }
    }
}
