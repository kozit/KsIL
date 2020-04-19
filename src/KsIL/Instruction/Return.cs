using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL.Instruction
{
    public class Return : IInstruction
    {
        public void Run(CPU CPU, byte[] CommandBuffer)
        {
            CPU.PC = CPU.CallStack[^1];
            CPU.CallStack.RemoveAt(CPU.CallStack.Count - 1);
        }
    }
}
