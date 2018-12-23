namespace KsIL.Runtime
{
    class Program
    {

        static KsILVM KsIL;
        
        static void Main(string[] args)
        {

            int Memory = 1024 * 4;

            string File = "";

            bool MemBump = false;

            bool MemLoad = false;
            string MemFile = "";

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
                    else if (args[i] == "-memload")
                    {

                        MemLoad = true;
                        MemFile = args[i + 1];
                        i++;

                    }

                }

            }
            else if (args.Length == 1)
            {

                File = args[0];

            }
            else
            {

                File = "TestFile.KsIL";

            }



            KsIL = new KsILVM(Memory, new KsIL.Builtin.ThreadManager());


            KsIL.LoadFile(File);

            if (MemLoad)
            {

                KsIL.memory.Set(0, System.IO.File.ReadAllBytes(MemFile));

            }

            KsIL.AutoTick();

            if (MemBump)
            {

                System.IO.File.WriteAllBytes("mem.bin", KsIL.memory.Get(0, KsIL.memory.GetSize() - 1));

            }

            while (true)
            {
            }

        }
    }
}