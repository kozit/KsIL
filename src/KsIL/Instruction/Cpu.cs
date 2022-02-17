using KsIL.extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KsIL.Instruction
{
    public class Cpu : IInstruction
    {
        public void Run(byte[] CommandBuffer)
        {
            (byte[], int) src = CommandBuffer.GetDataPrefix();
            CPU.Current.GetBus.WriteData(BitConverter.ToUInt64(CommandBuffer, src.Item2), src.Item1);
        }
    }
}
