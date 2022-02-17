using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL.Instruction
{
    public class Stack : IInstruction
    {
        public void Run(byte[] CommandBuffer)
        {
            if (CommandBuffer[0] == 0x00)
            {
                CPU.Current.RegisterStack.Add(CPU.Current.Registers);
            }
            else
            {
                CPU.Current.Registers = CPU.Current.RegisterStack[^1];
                CPU.Current.RegisterStack.RemoveAt(CPU.Current.CallStack.Count - 1);
            }
        }
    }
}
