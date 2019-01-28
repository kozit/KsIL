using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public class Interrupt
    {

        public static Dictionary<UInt16, Interrupt> Default
        {

            get
            {

                Dictionary<UInt16, Interrupt> r = new Dictionary<UInt16, Interrupt>
                {
                    { 2, new Interrupts.CPU_Interrupt() },
                    { 3, new Interrupts.Console() },
                    { 4, new Interrupts.Invoke() }
                };

                return r;

            }
        }

        public UInt16 Code;

        public Interrupt()
        {
        }

        public virtual void Run(byte[] Parameters, CPU CPU)
        {
        }

    }
}
