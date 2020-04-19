using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL.Instruction
{
    public class Stack : IInstruction
    {
        public void Run(CPU CPU, byte[] CommandBuffer)
        {
            if (CommandBuffer[0] == 0x00)
            {
                CPU.RegisterStack.Add(CPU.Registers);
            }
            else
            {
                CPU.Registers = CPU.RegisterStack[^1];
                CPU.RegisterStack.RemoveAt(CPU.CallStack.Count - 1);
            }
        }
    }
}
