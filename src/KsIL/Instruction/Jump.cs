using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL.Instruction
{
    public class Jump : IInstruction
    {
        public void Run(byte[] CommandBuffer)
        {
            
            CPU.Current.CallStack.Add((UInt64)(CPU.Current.PC + 1));
            if(CommandBuffer[0] == 0xFF)
                CPU.Current.PC += BitConverter.ToUInt64(CommandBuffer, 1);
            else
                CPU.Current.PC  = BitConverter.ToUInt64(CommandBuffer, 1);
        }
    }
}
