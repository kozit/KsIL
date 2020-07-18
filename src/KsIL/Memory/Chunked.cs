using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL.Memory
{
    public class Chunked : IMemory
    {

        UInt64 ChunkSize;
        private List<byte[]> mMemory;
        readonly byte[] BlankChunk;
        public Chunked(UInt64 ChunkSize, int ChunkCount)
        {
            this.ChunkSize = ChunkSize;
            BlankChunk = new byte[ChunkSize];
            mMemory = new List<byte[]>();
            for (int i = 0; i < ChunkCount; i++)
                mMemory.Add(null);
        }

        byte[] GetChunk(ulong address)
        {
            byte[] chunk = mMemory[GetChunkOffset(address)];
            if (chunk == null)
                return BlankChunk;
            return chunk;
        }

        int GetChunkOffset(ulong address) { return (int)MathF.Floor(address / ChunkSize); }
        int GetChunkAddress(ulong address) { return (int)address - (GetChunkOffset(address) * (int)ChunkSize); }
        int GetChunkCount(ulong size) { return (int)MathF.Ceiling(size / ChunkSize); }
        bool IsAddressInChunk(ulong address, int ChunkOffset) { return ((ChunkOffset * (int)ChunkSize) >= (int)address && (int)address <=((ChunkOffset * (int)ChunkSize) - 1));  }

        public byte Get(ulong address, ulong Offset)
        {
            return GetChunk(address - Offset)[GetChunkAddress(address - Offset)];
        }

        public byte[] GetBlock(ulong address, ulong size, ulong Offset)
        {
            ulong addressOffest = 0;
            byte[] output = new byte[size];
            List<byte[]> chunks = mMemory.GetRange(GetChunkOffset(address - Offset), GetChunkCount(size) + 1);
            if (GetChunkCount(size) == 1 && ((int)size + GetChunkAddress(address)) < ((int)ChunkSize + GetChunkAddress(address)))
            {
                if (chunks[0] == null) return BlankChunk;

                Array.Copy(chunks[0], GetChunkAddress(address), output, 0, (int)size);
                return output;

            }

            // come up with a better way later
            for (int i = 0; i < (int)size; i++)
                output[i] = Get(address + 1, Offset);

            return output;
            
        }

        public byte[] GetData(ulong address, ulong offset)
        {
            return GetBlock(address + 4, BitConverter.ToUInt64(GetBlock(address, 4, offset)), offset);
        }

        public void Set(ulong address, byte data, ulong Offset)
        {
            mMemory[GetChunkOffset(address - Offset)][GetChunkAddress(address - Offset)] = data;
        }

        public void SetBlock(ulong address, byte[] data, ulong Offset)
        {
            for (int i = 0; i < data.Length; i++)
                Set(address + (ulong)i, data[i], Offset);
        }

        public void SetData(ulong address, byte[] data, ulong Offset)
        {
            SetBlock(address, BitConverter.GetBytes((ulong)data.Length), Offset);
            SetBlock(address + 4, data, Offset);
        }
    }
}
