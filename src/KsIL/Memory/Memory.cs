using System;

namespace KsIL
{
    public partial class Memory
    {
               
        public static readonly uint PROGRAM_RUNNING = 1;
        public static readonly uint GRAPHICS_POuintER = 2;

        internal KsILVM VM;

        byte[] Buffer;
        uint Size;

        public Memory(uint Size, KsILVM VM)
        {
            this.VM = VM;
            this.Size = Size;
            Debugger.Log(Size, "Memory");
            Buffer = new byte[Size];
        }

        public uint GetSize()
        {
            return Size;
        }

        public void Clear()
        {
            Buffer = new byte[Size];
        }

        public byte Get(uint Addr)
        {
            return Buffer[Addr];          
        }

        public byte[] Get(uint Addr, uint Length)
        {

            byte[] temp = new byte[Length];

            Array.Copy(Buffer, Addr, temp, 0, Length);

            return temp;

        }

        public byte[] GetDataPionter(uint Addr)
        {

            uint point = BitConverter.ToUInt32(Get(Addr, 4), 0);
            return GetData(point);
            
        }

        public byte[] GetData(uint Addr)
        {

            return Get(Addr + 4, BitConverter.ToUInt32(Get(Addr, 4), 0));

        }

        public void ClearData(uint Addr)
        {

            SetData(Addr, new byte[BitConverter.ToUInt32(Get(Addr, 4), 0)] );

        }

        public void Set(uint Addr, byte Value)
        {
            Buffer[Addr] = Value;
        }

        public void Set(uint Addr, byte[] Value)
        {

            Array.Copy(Value, 0, Buffer, Addr, Value.Length);

        }

        public void SetDataPionter(uint Pouinter, uint Addr, byte[] Value)
        {

            Set(Pouinter, BitConverter.GetBytes(Addr));
            SetData(Addr, Value);
            
        }

        public void SetDataPionter(uint Pointer, byte[] Value)
        {

            uint i;
            for (i = 100; i < GetSize(); i++)
            {

                if (i + Value.Length >= Pointer)
                {

                    i = Pointer + 4;
                    continue;

                }

                if (Get(i, (uint)Value.Length) == new byte[Value.Length])
                {
                    break;
                }

            }


            SetDataPionter(Pointer, i, Value);

        }

        public void SetData(uint Addr, byte[] Value)
        {

                Set(Addr, BitConverter.GetBytes(Value.Length));
                Set(Addr + 4, Value);

        }

        bool BytesClean(byte[] bytes)
        {
            if (bytes == null) return false;

            for (uint i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] != 0x00) return false;
            }
            return true;
        }

        public void Destroy()
        {
            Buffer = null;
            Size = 0;
        }

        public void Dispose()
        {
            Destroy();
        }
    }
}
