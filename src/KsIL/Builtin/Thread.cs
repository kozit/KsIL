using System;
using System.Collections.Generic;
using System.Text;

using KsIL;

namespace KsIL.Builtin
{
    public class Thread : ThreadBase
    {

        public Thread(Int64 id, List<InstructionBase> code, Memory memory) : base(id, code, memory)
        {

        }

        public override void Tick()
        {
            mCode[(int)GetProgramCount()].Run();
        }

    }
}
