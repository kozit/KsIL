using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public class CodeParser
    {

        public static InstructionBase SingleOpCode(byte OpCode, byte[] data)
        {
            InstructionBase output = null;

            return output;
        }

        public static List<InstructionBase> Make(byte[] code)
        {
            List<InstructionBase> output = new List<InstructionBase>();

            for (int i = 0; i < code.Length; i++)
            {

                byte OpCode = code[i];

                List<byte> data = new List<byte>();

                for (int offset = i + 1; offset < code.Length; offset++)
                {
                    if (code[offset] == 0x00 && code[offset + 1] == 0xFF && code[offset + 2] == 0x00 && code[offset + 3] == 0xFF)
                    {
                            break;
                    }

                    data.Add(code[offset]);
                                       
                }


                if (OpCode == 0xFD)
                {

                }
                else
                {
                    output.Add(SingleOpCode(OpCode, data.ToArray()));
                }

            }

            return output;
        }

    }
}
