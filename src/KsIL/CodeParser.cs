using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public class CodeParser
    {

        public static List<Instruction> Make(byte[] code)
        {
            List<Instruction> output = new List<Instruction>();

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

                    i = offset;
                                                           
                }

                output.Add(new Instruction() { OPCode = (OpCode)OpCode, data = data.ToArray()});
                

            }

            return output;
        }

    }
}
