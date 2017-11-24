using System;
using System.Collections.Generic;

namespace KsIL
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
            else if (input[0] == 0xFF)
            {

                int Addr = BitConverter.ToInt32(input, 0);
                r.AddRange(mMemory.GetData(Addr));

            }
            else if (input[0] == 0xFE)
            {

                int Addr = BitConverter.ToInt32(input, 0);

                r.AddRange(mMemory.Get(Addr, 4));

            }

            return r.ToArray();

        }

        public static string[] GetStringArray(byte[] input, Memory mMemory)
        {
            
            int ItemCount = BitConverter.ToInt32(input, 0);

            string[] Items = new string[ItemCount];

            int[] ItemLength = new int[ItemCount];

            for (int i = 0; i < ItemCount; i ++)
            {

                ItemLength[i] = BitConverter.ToInt32(input, 4 + (i * 4));

            }

            int offset = 4 + (ItemCount * 4);

            for (int i = 0; i < ItemCount; i++)
            {

                Items[i] = System.Text.Encoding.UTF8.GetString(Read(input, mMemory), offset, ItemLength[i]);
                offset += ItemLength[i];
            }
            
            return Items;

        }

        public static byte[] GetData(int Addr, byte[] input)
        {

            int l = BitConverter.ToInt32(input, Addr);
            byte[] r = new byte[l];

            Array.Copy(input, r, Addr + 4);

            return r;

        }

    }
}
