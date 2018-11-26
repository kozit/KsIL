using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL.Interrupts
{
    public class Threading : Interrupt
    {

        ThreadManagerBase ThreadManager;

        public Threading(ThreadManagerBase threadManagerBase)
        {
            Code = 2;
            ThreadManager = threadManagerBase;
        }

        public override void Run(byte[] Parameters, Memory mMemory)
        {

            if (Parameters[0] == 0x00)
            {

                ThreadManager.StartThread(ThreadManager.AddThread(BitConverter.ToInt64(Parameters, 1)));

            }
            else if (Parameters[0] == 0x01)
            {

                ThreadManager.StopThread(BitConverter.ToInt64(Parameters, 1));

            }

        }

    }
}
