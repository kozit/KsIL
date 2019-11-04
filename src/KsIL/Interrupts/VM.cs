using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL.Interrupts
{
    public class VM : Interrupt
    {

        enum code : byte {

            INTadd    = 0x31,
            INTremove = 0x32,
            RAMadd    = 0x21,
            RAMset    = 0x22,
            VMreset   = 0x01

        }

        public VM()
        {

            Code = 5;

        }

        public override void Run(byte[] Parameters, CPU CPU)
        {

            switch (Parameters[0]) {

                case (byte)code.RAMadd:

                    break;

            }
               

        }

    }
}
