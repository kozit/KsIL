using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public class InstructionBase
    {

        internal Memory mMemory;

        public InstructionBase(Memory memory)
        {
            mMemory = memory;
        }

        public virtual void Run()
        {
        }


        public byte[] Read(Int64 location)
        {
            return mMemory.GetDataPionter(location);
        }

        public byte[] Read(byte[] data)
        {
            return Read(BitConverter.ToInt64(data, 0));
        }


        public Int64 ReadLength(Int64 location)
        {
            return BitConverter.ToInt64(mMemory.Get(location, 8), 0);
        }

        public Int64 ReadLength(byte[] data)
        {
            return ReadLength(BitConverter.ToInt64(data, 0));
        }

    }
}
