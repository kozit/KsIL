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
            KsIL = new KsIL(1024 * 1024 * 2, System.IO.File.ReadAllBytes("test.KsIL"));

            
            System.IO.File.WriteAllBytes("mem.txt" ,KsIL.memory.Get(0, KsIL.memory.GetSize() - 1));

            while (true)
            {
            }
        }
    }
}
