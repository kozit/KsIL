using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public class ThreadManagerBase
    {     

        /// <summary>
        /// Runs on Main Thread every tick the KsIL Thread should run on this
        /// </summary>
        public virtual void Tick()
        { }

        public virtual void AddThread(List<InstructionBase> code, Memory memory)
        { }

        public virtual void RemoveThread(Int64 ID)
        { }

        public virtual void StopThread(Int64 ID)
        { }

        public virtual void StartThread(Int64 ID)
        { }

        
    }
}
