using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL.Instruction
{
    public class Return : IInstruction
    {
        public void Run(byte[] CommandBuffer)
        {
            CPU.Current.PC = CPU.Current.CallStack[^1];
            CPU.Current.CallStack.RemoveAt(CPU.Current.CallStack.Count - 1);
        }
    }
}
