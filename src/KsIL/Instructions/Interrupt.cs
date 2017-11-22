using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL.Instructions
{
    public class Interrupt : InstructionBase
    {

        public static List<Interrupts> Interrupts = new List<Interrupts>();

        byte[] mParameters;

        public Interrupt(Memory memory, byte[] Parameters) : base(memory)
        {

            mParameters = Parameters;
         
        }

        public override void Run()
        {

            foreach (Interrupts Int in Interrupts)
            {
                if (BitConverter.ToInt16(mParameters, 0) == Int.Code)
                {
                    byte[] Parameters = new byte[mParameters.Length - 3];

                    mParameters.Clone(Parameters, 2);

                    Int.Run(Parameters);
                }
            }

        }

    }
}
