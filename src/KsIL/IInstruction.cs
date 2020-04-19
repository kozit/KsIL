using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public interface IInstruction
    {
        void Run(CPU CPU, byte[] CommandBuffer);
    }
}
