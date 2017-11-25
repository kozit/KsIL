using System;
using System.Collections.Generic;

namespace KsIL.Runtime
{
    class Program
    {

        static KsILVM KsIL;
        
        static void Main(string[] args)
        {

            int Memory = 1024 * 4;

            string File = "test.KsIL";

            bool MemBump = true;

            if (args.Length > 1)
            {

                for (int i = 0; i < args.Length; i++)
                {

                    if (args[i] == "-mem")
                    {

                        Memory = int.Parse(args[i + 1]);
                        i++;

                    }
                    else if (args[i] == "-file")
                    {

                        File = args[i + 1];
                        i++;
                    }
                    else if (args[i] == "-memdump")
                    {

                        MemBump = true;
                        i++;
                    }

                }

            }
            else if (args.Length == 1)
            {
                File = args[0];
            }


            KsIL = new KsILVM(Memory, System.IO.File.ReadAllBytes(File));

            if(MemBump)
            System.IO.File.WriteAllBytes("mem.bin" ,KsIL.memory.Get(0, KsIL.memory.GetSize() - 1));

            while (true)
            {
            }
        }
    }
}
