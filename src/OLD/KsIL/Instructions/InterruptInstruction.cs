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
            foreach (byte item in Parameters)
                Console.Write(item);
            mParameters = Parameters;
         
        }

        public override void Run()
        {

            Int16 INT = BitConverter.ToInt16(mParameters, 0);


            foreach (Interrupt Int in Interrupts)
            {

                if (INT == Int.Code)
                {

                    List<byte> Parameters = new List<byte>();
                    //Utill.ArrayRemoveAt(mParameters, 2, mParameters.Length - 2);

                    for (int i = 2; i < mParameters.Length; i++)
                        Parameters.Add(mParameters[i]);

                    Int.Run(Parameters.ToArray(), mMemory);

                }

            }

        }

    }
}
