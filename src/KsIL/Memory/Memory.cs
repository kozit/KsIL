using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace KsIL
{
    public partial class Memory : IDisposable
    {

        public class Pointer {

            public const uint PROGRAM_RUNNING = 1;
            public const uint GRAPHICS = 2;
            public const uint CPU = 6;
            public const uint SYSTEM = 10;

        }

        internal KsILVM VM;

        List<byte[]> memory;
        uint Size;
        uint ChunkSize;
        byte[] BlankChunk;

        private byte[] Make0x00Arrary(uint Size)
        {

            byte[] temp = new byte[Size];
            for (int i = 0; i < Size; i++)
                temp[i] = 0x00;

            return temp;

        }

        public Memory(uint Size, KsILVM VM, uint ChunkSize = 1024)
        {
            this.VM = VM;
            this.Size = Size;
            this.ChunkSize = ChunkSize;
            Debugger.Log(Size, "Memory");
            Debugger.Log(ChunkSize, "Memory");
            BlankChunk = Make0x00Arrary(ChunkSize);

            memory = new List<byte[]>((int)Math.Ceiling((float)(Size / ChunkSize)));
            
            for (int i = 0; i <= Size / ChunkSize; i++)
            {
                memory[i] = null;
            }
            
            Set(0, Make0x00Arrary(ChunkSize * 5));

        }

        public uint GetSize()
        {
            return Size;
        }

        public void Clear()
        {
            Set(Pointer.PROGRAM_RUNNING, 0x01);
        }

        // if a Chunk i null then return a BlankChunk ie all 0x00
        private byte[] GetMemoryChunk(uint Chunk)
        {
            return GetMemoryChunk((int)Chunk);
        }

        private byte[] GetMemoryChunk(int Chunk)
        {

            if (memory[Chunk] == null)
                return BlankChunk;
            else
                return memory[Chunk];


        }

        private bool MemoryChunkNull(uint Chunk, bool set = false)
        {
            return MemoryChunkNull((int)Chunk, set);
        }

        private bool MemoryChunkNull(int Chunk, bool set = false)
        {

            if (memory[Chunk] == null)
            {

                if (set == true)
                {
                    memory[Chunk] = new byte[ChunkSize];
                    return true;
                }
                return false;

            }
            else
                return true;

        }

        public byte Get(uint Addr)
        {
            return GetMemoryChunk(Addr / ChunkSize)[Addr - (ChunkSize * (Addr / ChunkSize))];          
        }
               
        public byte[] Get(uint Addr, uint Length)
        {

            byte[] temp = new byte[Length];
            uint Startchunk = Addr / ChunkSize;
            uint EndChunk   = (Addr + Length) / ChunkSize;

            if (Startchunk == EndChunk)
            {

                Array.Copy(GetMemoryChunk(Startchunk), (int)(Addr - (ChunkSize * Startchunk)), temp, 0, Length);
                return temp;
            }

            int Offset = 0;

            Array.Copy(GetMemoryChunk(Startchunk), (int)(Addr - (ChunkSize * Startchunk)), temp, 0, (int)ChunkSize - (Addr - (ChunkSize * Startchunk)));

            Offset += (int)(ChunkSize - (Addr - ChunkSize * Startchunk));

            for (int i = (int)Startchunk + 1; i <= (int)EndChunk - 1; i++)
            {
                                             
                Array.Copy(GetMemoryChunk((uint)i), (int)(Addr - (ChunkSize * i)), temp, Offset, (int)ChunkSize - (Addr - (ChunkSize * i)));

                Offset += (int)ChunkSize;

            }


            Array.Copy(GetMemoryChunk(EndChunk), (int)(Addr - (ChunkSize * EndChunk)), temp, 0, (int)ChunkSize - (Addr - (ChunkSize * EndChunk)));

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

            Set(Addr, new byte[BitConverter.ToUInt32(Get(Addr, 4), 0) + 4] );

        }

        public void Set(uint Addr, byte Value)
        {
            int chunk = (int)(Addr / ChunkSize);

            MemoryChunkNull(Addr, true);

            memory[chunk][Addr - (ChunkSize * chunk)] = Value;
            
        }

        
        public void Set(uint Addr, byte[] Value)
        {

            // use this for now
            for (uint i = 0; i < Value.Length; i++)
            {
                Set(Addr + i, Value[i]);
            }

            //Array.Copy(Value, 0, memory, Addr, Value.Length);


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
               
        public void Dispose()
        {

            

        }
    }
}
