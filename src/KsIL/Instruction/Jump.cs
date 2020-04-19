using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL.Instruction
{
    public class Jump : IInstruction
    {
        public void Run(CPU CPU, byte[] CommandBuffer)
        {
            CPU.CallStack.Add((UInt64)(CPU.PC + 1));
            if(CommandBuffer[0] == 0xFF)
                CPU.PC += BitConverter.ToUInt64(CommandBuffer, 1);
            else
                CPU.PC =  BitConverter.ToUInt64(CommandBuffer, 1);
        }
    }
}
