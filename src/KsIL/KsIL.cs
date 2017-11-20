using System;
using System.Collections.Generic;

namespace KsIL
{
    public class KsIL
    {

        public Memory memory;

        List<InstructionBase> Code = new List<InstructionBase>();

        public KsIL(int _memory, byte[] mCode)
        {
            if(true == false)
            for (int i = 0; i < mCode.Length; )
            {

                byte bytecode = mCode[i];
                
                int ii = 0;
                List<byte> Parameters = new List<byte>();
                while (new byte[] { mCode[ii + i], mCode[ii + i + 1] , mCode[ii + i + 2] } != new byte[] { 0x00, 0xFF, 0x00})
                {
                    Parameters.Add(mCode[ii + i]);
                    ii++;    
                }

                InstructionBase instructionBase;

                if (bytecode == 0x01)
                {

                    instructionBase = new Instructions.Store(memory, Parameters.ToArray());

                }
                else
                {

                    instructionBase = null;

                }
                Code.Add(instructionBase);
            }

            mCode = null;

            memory = new Memory(_memory);

            string test = "this is a test";
            List<byte> Test = new List<byte>();

            Test.AddRange(BitConverter.GetBytes(System.Text.Encoding.UTF8.GetBytes(test).Length));
            Test.AddRange(System.Text.Encoding.UTF8.GetBytes(test));
            Test.AddRange(BitConverter.GetBytes(15));

            Code.Add(new Instructions.Store(memory, Test.ToArray()));


            Int32 qwe = 0;

            // Memory Mode 0x00 (16bit), 0x01 (32 bit), 0x02 (64 bit)
            memory.Set(0, 0x01);
            // Is program running
            memory.Set(1, 0x01);
            // Conditional Result
            memory.Set(2, 0x00);
            // Program Counter
            memory.Set(4, BitConverter.GetBytes(qwe));
            //Return Pointer
            memory.Set(9, BitConverter.GetBytes(qwe));
            
            
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
