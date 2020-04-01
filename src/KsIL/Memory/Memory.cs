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
        List<object> memoryLocks;
        uint Size;
        uint ChunkSize;
        uint TotalChunks;
        byte[] BlankChunk;

        private byte[] Make0x00Arrary(uint Size)
        {

            byte[] temp = new byte[Size];
            for (int i = 0; i < Size; i++)
                temp[i] = 0x00;

            return temp;

        }

        public Memory(uint Size, KsILVM VM, uint mChunkSize = 1024)
        {
            this.VM          = VM;
            this.Size        = Size;
            this.ChunkSize   = mChunkSize;
            this.TotalChunks = (uint)Math.Ceiling((float)(Size / ChunkSize));
            this.BlankChunk  = Make0x00Arrary(ChunkSize);
            Debugger.Log(Size, "Memory", Debugger.DebugTypes.cpu);
            Debugger.Log(ChunkSize, "Memory", Debugger.DebugTypes.cpu);

            memoryLocks = new List<object>((int)this.TotalChunks);
            memory = new List<byte[]>((int)this.TotalChunks);
            
            for (int i = 0; i <= Size / ChunkSize; i++)
            {
                memoryLocks.Add(null);
                memory.Add(null);
            }
            
        }

        public uint GetSize()
        {
            return Size;
        }

        public void Clear()
        {
            Set(Pointer.PROGRAM_RUNNING, 0x01);
        }

        private byte[] GetMemoryChunkAddr(uint Addr)
        {

            return GetMemoryChunk((int)Math.Floor((decimal)(Addr / ChunkSize)));

        }

        // if a Chunk is null then return a BlankChunk ie all 0x00
        private byte[] GetMemoryChunk(uint Chunk)
        {
            return GetMemoryChunk((int)Chunk);
        }

        private byte[] GetMemoryChunk(int Chunk)
        {

            if (memory[Chunk] == null)
                if(this.TotalChunks - 1 == Chunk)
                    return Make0x00Arrary(this.ChunkSize * (this.Size / this.TotalChunks) - this.TotalChunks);
                else
                    return BlankChunk;
            else
                return memory[Chunk];
            
        }

        private bool MemoryChunkNullAddr(int Addr, bool set = false)
        {
            return MemoryChunkNull((int)Math.Floor((decimal)(Addr / ChunkSize)), set);
        }

        private bool MemoryChunkNullAddr(uint Addr, bool set = false)
        {
            return MemoryChunkNull((int)Math.Floor((decimal)(Addr / ChunkSize)), set);
        }

        private bool MemoryChunkNull(uint Chunk, bool set = false)
        {
            return MemoryChunkNull((int)Chunk, set);
        }

        private bool MemoryChunkNull(int Chunk, bool set = false)
        {

            if (memory[Chunk] != null)
                return false;
            
            if (set == true)
            {

                memory[Chunk] = new byte[ChunkSize];
                
            }

            return true;

        }

        private int MemoryAddrToChunk(uint Addr)
        {
            return MemoryAddrToChunk((int)Addr);
        }

        private int MemoryAddrToChunk(int Addr)
        {
            return (int)Math.Floor((decimal)Addr / this.ChunkSize );
        }

        public byte Get(uint Addr)
        {
            return GetMemoryChunk(Addr / ChunkSize)[Addr - (ChunkSize * (Addr / ChunkSize))];          
        }
               
        public byte[] Get(uint Addr, uint Length)
        {

            byte[] temp = new byte[Length];
            uint Startchunk    = Addr / ChunkSize;
            uint TotalChunks   = (uint)Math.Ceiling((decimal)(Length / ChunkSize));

            if (Startchunk == (Startchunk + TotalChunks) - 1)
            {

                Array.Copy(GetMemoryChunk(Startchunk), (int)(Addr - (ChunkSize * Startchunk)), temp, 0, Length);
                return temp;
            }

            int Offset = 0;

            Array.Copy(GetMemoryChunk(Startchunk), (int)(Addr - (ChunkSize * Startchunk)), temp, 0, (int)ChunkSize - (Addr - (ChunkSize * Startchunk)));

            Offset += (int)(Addr - ChunkSize * Startchunk);

            for (int i = (int)Startchunk; i <= (int)(Startchunk + TotalChunks) - 2; i++)
            {
                Debugger.Log(i + ":" + (Addr - (ChunkSize * i)) + ":" + (ChunkSize - (Addr - (ChunkSize * i))), "Memory:getPart", Debugger.DebugTypes.cpu);
                int t = (int)(Addr - (ChunkSize * i));
                if (t < 0)
                    t = 0;
                Array.Copy(GetMemoryChunk((uint)i), t, temp, Offset, (int)ChunkSize - (Addr - (ChunkSize * i)));

                Offset += (int)ChunkSize;

            }
            
            Array.Copy(GetMemoryChunk((Startchunk + TotalChunks) - 1), (int)(Addr - (ChunkSize * (Startchunk + TotalChunks) - 1)), temp, 0, (int)ChunkSize - (Addr - (ChunkSize * (Startchunk + TotalChunks) - 1)));

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

        private void rawSet(uint Addr, byte Value)
        {
            int chunk = (int)(Addr / ChunkSize);

            MemoryChunkNullAddr(Addr, true);

            memory[chunk][Addr - (ChunkSize * chunk)] = Value;

            if (GetMemoryChunkAddr(Addr) == BlankChunk)
            {
                memory[(int)Math.Floor((decimal)(Addr / ChunkSize))] = null;
            }

        }

        public void Set(uint Addr, byte Value, bool safe = false)
        {
            if (safe)
            {
                lock (memoryLocks[(int)(Addr / ChunkSize)])
                {
                    rawSet(Addr, Value);
                }
            }
            else
            {
                rawSet(Addr, Value);
            }

        }

        
        public void Set(uint Addr, byte[] Value, bool safe = false)
        {

            // use this for now
            for (uint i = 0; i < Value.Length; i++)
            {
                Set(Addr + i, Value[i], safe);
            }

            //Array.Copy(Value, 0, memory, Addr, Value.Length);


        }

        public void SetDataPionter(uint Pouinter, uint Addr, byte[] Value, bool safe = false)
        {

            Set(Pouinter, BitConverter.GetBytes(Addr), safe);
            SetData(Addr, Value, safe);
            
        }

        public void SetDataPionter(uint Pointer, byte[] Value, bool safe = false)
        {

            uint i;
            for (i = 100; i < ( GetSize() - Value.Length ); i++)
            {

                if (i + Value.Length == Pointer)
                {

                    i = Pointer + 4;
                    continue;

                }

                if (Get(i, (uint)Value.Length) == new byte[Value.Length])
                {
                    break;
                }

            }
            
            SetDataPionter(Pointer, i, Value, safe);

        }

        public void SetData(uint Addr, byte[] Value, bool safe = false)
        {

                Set(Addr, BitConverter.GetBytes(Value.Length), safe);
                Set(Addr + 4, Value, safe);

        }

        uint FindClean(uint size)
        {

            for (uint i = this.ChunkSize * 3; i < this.GetSize() - size; i++)
            {

                //if(Memo)

            }

            return this.GetSize();

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
