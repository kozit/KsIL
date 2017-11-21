using System;
using System.Collections.Generic;

namespace KsIL.Instructions
{
    public class Utill
    {

        public static byte[] Read(byte[] input, Memory mMemory)
        {

            List<byte> r = new List<byte>();
            if (input[0] == 0xF1)
            {

                byte[] temp = new byte[input.Length - 2];
                input.CopyTo(temp, 1);
                r.AddRange(temp);

            }
            else if(input[0] == 0xFF)
            {

                int Addr = BitConverter.ToInt32(input, 1);
                int i = BitConverter.ToInt32(mMemory.Get(Addr, 4),0);
                r.AddRange(mMemory.Get(Addr + 4, i));

            }
            else if (input[0] == 0xFE)
            {

                int Addr = BitConverter.ToInt32(input, 1);
                
                r.AddRange(mMemory.Get(Addr, 4));

            }
            
            return r.ToArray();

        }

    }
}
