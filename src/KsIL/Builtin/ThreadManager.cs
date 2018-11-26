using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL.Builtin
{
    public class ThreadManager : ThreadManagerBase
    {

        List<Thread> Threads;

        public ThreadManager()
        {

            Threads = new List<Thread>();
            

        }

        public override void Tick()
        {

            foreach (Thread item in Threads)
            {
                if (item != null)
                    continue;
                item.Tick();
            }

        }

        public override Int64 AddThread(Int64 startindex)
        {
            Threads.Add(new Thread(Threads.Count, code, Memory));
            Threads[Threads.Count - 1].SetProgramCount(startindex);
            return Threads.Count - 1;
        }

        public override void RemoveThread(Int64 ID)
        {

            Threads[(int)ID] = null;

        }

        public override void StopThread(Int64 ID)
        {

            Threads[(int)ID] = null;

        }

        public override void StartThread(Int64 ID)
        { }

    }
}
