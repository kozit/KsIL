using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL.Interrupts
{
    public class Console : Interrupt
    {

        enum codes
        {

            print = 0x01,
            readline = 0x02,
            read = 0x03

        }

        public Console()
        {

            Code = 3;

        }

        public override void Run(byte[] Parameters, CPU CPU)
        {
                                   
            codes code = (codes)Parameters[0];
            
            uint point = BitConverter.ToUInt32(CPU.getPart(Parameters, 1, 4), 0);

            Debugger.Log(code.ToString(), "Console:code");

            Debugger.Log(point, "Console:point");

            switch (code)
            {

                case codes.print:


                    System.Console.Write(Encoding.UTF8.GetString(CPU.Memory.GetData(point)).Replace("/n", Environment.NewLine));

                break;

                case codes.readline:

                    string input = System.Console.ReadLine();

                    CPU.Memory.SetData(point, Encoding.UTF8.GetBytes(input));

                break;

                case codes.read:

                    char key = System.Console.ReadKey().KeyChar;

                    CPU.Memory.Set(point, Encoding.UTF8.GetBytes(key.ToString()));

                break;

            }

        }


    }
}
