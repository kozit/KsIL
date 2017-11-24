using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL.Instructions
{
    public class InterruptInstruction : InstructionBase
    {

        public static List<Interrupt> Interrupts = new List<Interrupt>();

        byte[] mParameters;

        public InterruptInstruction(Memory memory, byte[] Parameters) : base(memory)
        {

            mParameters = Parameters;
         
        }

        public override void Run()
        {

            int INT = BitConverter.ToInt16(Utill.Read(mParameters, mMemory), 0);

            foreach (Interrupt Int in Interrupts)
            {

                if (INT == Int.Code)
                {
                    
                    byte[] Parameters = new byte[mParameters.Length - 3];

                    Array.Copy(Utill.Read(mParameters, mMemory), Parameters, 4);
                    
                    Int.Run(Parameters, mMemory);

                }

            }

        }

    }
}
