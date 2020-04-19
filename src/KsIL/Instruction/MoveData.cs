using KsIL.extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KsIL.Instruction
{
    public class MoveData : IInstruction
    {
        public void Run(CPU CPU, byte[] CommandBuffer)
        {
            (byte[], int) src = CommandBuffer.GetDataPrefix(0, CPU);
            CPU.GetBus.WriteData(BitConverter.ToUInt64(CommandBuffer, src.Item2), src.Item1);
        }
    }
}
