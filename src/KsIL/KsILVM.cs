using System;
using System.Collections.Generic;

namespace KsIL
{
    public class KsILVM
    {

        public Memory memory;

        List<InstructionBase> Code = new List<InstructionBase>();

        public KsILVM(int _memory, byte[] mCode, List<Interrupt> Interrupts = null)
        {

            if (Interrupts == null)
            {

                Interrupts = Interrupt.Default();

            }

            Instructions.InterruptInstruction.Interrupts = Interrupts;

            memory = new Memory(_memory);

            Int32 qwe = 0;

            // Memory Mode 0x00 (8 Bit), 0x01 (16 Bit), 0x02 (32 Bit), 0x03 (64 Bit)
            memory.Set(0, mCode[0]);
            // Is program running
            memory.Set(1, 0x01);
            // Conditional Result
            memory.Set(2, 0x00);
            // Program Counter
            memory.Set(4, BitConverter.GetBytes(qwe));
            //Return Pointer
            memory.Set(9, BitConverter.GetBytes(qwe));


            for (int i = 1; i < mCode.Length; )
            {

                byte bytecode = mCode[i];
                
                int ii = 0;
                List<byte> Parameters = new List<byte>();

                for (ii = 1; ii + i  + 3< mCode.Length; ii++)
                {

                    if (mCode[ii + i] == 0x00 && mCode[ii + i + 1] == 0xFF && mCode[ii + i + 2] == 0x00 && mCode[ii + i + 3] == 0xFF)
                    {
                        break;
                    }

                    Parameters.Add(mCode[i + ii]);

                }

                InstructionBase instructionBase;
                

                if (bytecode == 0x00)
                {

                    instructionBase = new Instructions.InterruptInstruction(memory, Parameters.ToArray());

                }
                else if (bytecode == 0x01)
                {

                    instructionBase = new Instructions.Store(memory, Parameters.ToArray());

                }
                else if (bytecode == 0x02)
                {

                    instructionBase = new Instructions.DynamicStore(memory, Parameters.ToArray());

                }
                else if (bytecode == 0x03)
                {

                    instructionBase = new Instructions.ReadInto(memory, Parameters.ToArray());

                }
                else if (bytecode == 0x04)
                {

                    instructionBase = new Instructions.Store(memory, Parameters.ToArray());

                }
                else if (bytecode == 0x05)
                {

                    instructionBase = new Instructions.Fill(memory, Parameters.ToArray());

                }
                else if (bytecode == 0x06)
                {

                    instructionBase = new Instructions.Clear(memory, Parameters.ToArray());

                }
                else if (bytecode == 0x10)
                {

                    instructionBase = new Instructions.TestEqual(memory, Parameters.ToArray());

                }
                else if (bytecode == 0x11)
                {
                    // Test Greater Than
                    instructionBase = new Instructions.TestGreaterThan(memory, Parameters.ToArray());

                }
                else if (bytecode == 0x12)
                {

                    instructionBase = new Instructions.JumpIf(memory, Parameters.ToArray(), true);

                }
                else if (bytecode == 0x13)
                {

                    instructionBase = new Instructions.JumpIf(memory, Parameters.ToArray(), false);

                }
                else if (bytecode == 0x20)
                {

                    instructionBase = new Instructions.Jump(memory, Parameters.ToArray());

                }
                else
                {

                    instructionBase = null;

                }
                i = i + ii + 4;
                if(instructionBase != null)
                Code.Add(instructionBase);
            }
                 
            mCode = null;

            //Console.ReadLine();

            while (memory.Get(1) == 0x01)
            {

                int Line = BitConverter.ToInt32(memory.Get(4, 4), 0);

                memory.Set(4, BitConverter.GetBytes(Line + 1));

                
                if (Line >= Code.Count)
                {
                    memory.Set(1, 0x00);
                    continue;
                }

                Code[Line].Run();

            }

        }

    }
}
