using System;
using System.Collections.Generic;

namespace KsIL.Runtime
{
    class Program
    {

        static KsIL KsIL;
        
        static void Main(string[] args)
        {

            if (args.Length > 0)
            {
            }
            KsIL = new KsIL(1024 * 1024, System.IO.File.ReadAllBytes("test.KsIL"));

            

            while (true)
            {
            }
        }
    }
}
