using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;

namespace CosmosTest
{
    public class Kernel : Sys.Kernel
    {

        KsIL.KsILVM KsILVM;

        protected override void BeforeRun()
        {
            KsILVM = new KsIL.KsILVM(1024 * 1024, new KsIL.Builtin.ThreadManager(), null);


            KsILVM.Load(new byte[] { });
            

        }

        protected override void Run()
        {
            KsILVM.Tick();
        }
    }
}
