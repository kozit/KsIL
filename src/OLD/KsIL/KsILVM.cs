using System;
using System.Collections.Generic;

namespace KsIL
{
    public class KsILVM
    {

        public Memory memory;

        List<InstructionBase> Code = new List<InstructionBase>();

        public KsILVM(int _memory, List<Interrupt> Interrupts = null)
        {

            if (Interrupts == null)
            {

                Interrupts = Interrupt.Default;

            }

            Instructions.InterruptInstruction.Interrupts = Interrupts;

            memory = new Memory(_memory);
            
        }

        public void LoadFile(string File)
        {

            Load(System.IO.File.ReadAllBytes(File));

        }

        public void Load(byte[] mCode)
        {
            
            int code_Offset = 0;

            Int32 qwe = 0;

            // Memory Mode 0x00 (8 Bit), 0x01 (16 Bit), 0x02 (32 Bit), 0x03 (64 Bit)
            memory.Set(Memory.MEMORY_MODE, mCode[code_Offset]);
            // Is program running
            memory.Set(Memory.PROGRAM_RUNNING, 0x01);
            // Conditional Result
            //memory.Set(Memory.CONDITIONAL_RESULT, 0x00);
            // Program Counter
            //memory.Set(Memory.PROGRAM_COUNT, new byte[] { 0x00, 0x00, 0x00, 0x00 });
            //Return Pointer
            //memory.SetData(Memory.RETURN_POINTER, new byte[] { 0x00, 0x00, 0x00, 0x00});
            
            for (int i = 1; i < mCode.Length - code_Offset;)
            {

                byte bytecode = mCode[i];

                int ii = 0;
                List<byte> Parameters = new List<byte>();

                for (ii = 1; ii + i + 3 < mCode.Length; ii++)
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

                    instructionBase = new Instructions.DynamicReadInto(memory, Parameters.ToArray());

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
                else if (bytecode == 0x21)
                {

                    instructionBase = new Instructions.Return(memory, Parameters.ToArray());

                }
                else
                {
                    
                    instructionBase = null;

                }

                i = i + ii + 4;
                if (instructionBase != null)
                    Code.Add(instructionBase);
            }

            mCode = null;
            
        }

        public void AutoTick()
        {

            while (memory.Get(Memory.PROGRAM_RUNNING) == 0x01)
            {

                Tick();

            }

        }

        public void Tick()
        {
            
            int Line = BitConverter.ToInt32(memory.Get(Memory.PROGRAM_COUNT, 4), 0);

            memory.Set(Memory.PROGRAM_COUNT, BitConverter.GetBytes(Line + 1));

            if (Line >= Code.Count)
            {

                memory.Set(Memory.PROGRAM_RUNNING, 0x00);
                return;

            }

            Code[Line].Run();
            
        }

    }

}

