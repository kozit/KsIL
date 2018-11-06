using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;

namespace CosmosTest
{
    public class Kernel : Sys.Kernel
    {
       Int64 t;
        protected override void BeforeRun()
        {
            t = 99993499249249999;
                
        }

        protected override void Run()
        {
            Console.WriteLine(t);
        }
    }
}
