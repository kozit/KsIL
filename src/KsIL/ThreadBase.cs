using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public class ThreadBase
    {

        public Int64 ID;

        public Int64 ProgramCountPoint;

        protected Memory Memory;

        protected List<InstructionBase> mCode;

        public ThreadBase(Int64 id, List<InstructionBase> code, Memory memory)
        {
            mCode = code;
            ID = id;
            Memory = memory;
        }

        public void SetProgramCount(Int64 count)
        {

            Memory.SetArray(Memory.THREAD_POINTER, BitConverter.GetBytes(count), ID);

        }

        public Int64 GetProgramCount()
        {
            return BitConverter.ToInt64(Memory.GetArray(Memory.THREAD_POINTER,ID),0);
        }



        public virtual void Tick() { }

    }
}
