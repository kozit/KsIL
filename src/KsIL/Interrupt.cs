using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public class Interrupt
    {

        public static List<Interrupt> Default ()
        {

            List<Interrupt> r = new List<Interrupt>() { new Interrupts.Invoke() };

            r.AddRange(DefaultCosmos());

            return r;  
        }

        public static List<Interrupt> DefaultCosmos()
        {
            return new List<Interrupt>() { new Interrupts.Console(), new Interrupts.File() };
        }

        public Int16 Code;
                
        public Interrupt()
        {
        }

        public virtual void Run(byte[] Parameters, Memory mMemory)
        {
        }

    }
}
