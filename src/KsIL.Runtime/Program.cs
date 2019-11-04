using System;

namespace KsIL.Runtime
{
    class Program
    {

        static KsILVM _KsIL;
        
        static void Main(string[] args)
        {

            int Memory = Int32.MaxValue - (1024 * 4); //

            string File = "";

            bool MemBump = false;
            

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


                    }

                }

            }
            else if (args.Length == 1 && !(args[0] == "-memdump"))
            {

                File = args[0];

            }
            else
            {

                File = "TestFile.KsIL";
                MemBump = true;

            }
                     
            

            KsIL.Debugger._type = 2;

            KsIL.Debugger.Log("DebugMode:" + ((Debugger.types)KsIL.Debugger._type).ToString(), "Runtime", 1);

            KsIL.Debugger.Log("loading VM", "Runtime");
            

            _KsIL = new KsILVM(Memory);
            _KsIL.Load_File(File);
            _KsIL.Start();

            if (MemBump)
            {

                System.IO.File.WriteAllBytes("mem.bin", _KsIL.memDump());

            }
            
            while (true)
            {
            }

        }
    }
}