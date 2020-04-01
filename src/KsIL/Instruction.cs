using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public class Instruction
    {
        private uint offset = 0;
        public OpCode OPCode;
        public byte[] data;
        public CPU CPU;

        public void getOPcode(byte[] data)
        {
            int i = 0;
            bool lastFF = true;
            int opcode = 0;

            while (lastFF)
            {

                if (data[i] != 0xFF)
                    lastFF = false;

                opcode += data[i];

            }

        }

        public byte[] getData()
        {

            uint size = BitConverter.ToUInt32(data, (int)offset);

            byte[] outputdata = new byte[size];
            Array.Copy(data, offset + 4, outputdata, 0, size);
            offset = offset + 4 + size;

            return outputdata;

        }

        public string getString()
        {
            return Encoding.UTF8.GetString(getData());
        }

        #region getNumbers

        public UInt16 getUInt16()
        {
            return BitConverter.ToUInt16(get(2), 0);
        }

        public UInt32 getUInt32()
        {
            return BitConverter.ToUInt32(get(4), 0);
        }

        public UInt64 getUInt64()
        {
            return BitConverter.ToUInt64(get(8), 0);
        }

        public Int16 getInt16()
        {
            return BitConverter.ToInt16(get(2), 0);
        }

        public Int32 getInt32()
        {
            return BitConverter.ToInt32(get(4), 0);
        }

        public Int64 getInt64()
        {
            return BitConverter.ToInt64(get(8), 0);
        }
        #endregion

        public byte[] get(int length = -1)
        {
            
            Debugger.Log(data[offset], "Read:first");

            byte[] temp = read(length);

            Debugger.Log(temp, "Read:data_out");
            Debugger.Log(temp.Length, "Read:offset");
            offset += (uint)temp.Length;

            return temp;

        }

        #region helpers
        
        // this has no debug info
        private byte[] read(int length)
        {

            switch (data[offset])
            {

                case 0xF1:
                    return getPart((int)offset + 1);

                case 0xFF:
                    return CPU.Memory.GetDataPionter(BitConverter.ToUInt32(getPart((int)offset + 1, 4), 0));

                case 0xFE:
                    return CPU.Memory.Get(BitConverter.ToUInt32(getPart((int)offset + 1, 4), 0), 8);

                case 0xFD:
                    return CPU.Registers[BitConverter.ToUInt16(getPart((int)offset + 1, 2), 0)];

            }

            return getPart((int)offset, length);

        }

        private byte[] getPart(int startindex, int Length = -1)
        {

            if (Length == -1)
            {
                Length = data.Length - startindex;
            }

            byte[] output = new byte[Length];

            Debugger.Log(data, "getPart:data", Debugger.DebugTypes.cpu);
            Debugger.Log(startindex, "getPart:startindex", Debugger.DebugTypes.cpu);
            Debugger.Log(Length, "getPart:length", Debugger.DebugTypes.cpu);

            Array.Copy(data, startindex, output, 0, Length);

            Debugger.Log(output, "getPart:output", Debugger.DebugTypes.cpu);

            return output;

        }

        #endregion

    }
}
