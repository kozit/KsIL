using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public class ThreadManagerBase
    {

        protected List<InstructionBase> code;
        protected Memory Memory;

        public ThreadManagerBase()
        {
            
        }

        public void LoadCode(List<InstructionBase> code)
        {
            this.code = code;
        }

        public void LoadMemory(Memory Memory)
        {
            this.Memory = Memory;
        }

        /// <summary>
        /// Runs on Main Thread every tick the KsIL Thread should run on this
        /// </summary>
        public virtual void Tick()
        { }

        public virtual Int64 AddThread(Int64 startindex)
        {
            return -0;
        }

        public virtual void RemoveThread(Int64 ID)
        { }

        public virtual void StopThread(Int64 ID)
        { }

        public virtual void StartThread(Int64 ID)
        { }

        
    }
}
