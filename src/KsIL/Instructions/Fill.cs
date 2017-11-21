﻿using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL.Instructions
{
    class Fill : InstructionBase
    {
        int Location0;
        int Location1;
        byte fill; 
        public Fill(Memory memory, byte[] Parameters) : base(memory)
        {

            Location0 = BitConverter.ToInt32(Parameters, 0);
            Location1 = BitConverter.ToInt32(Parameters, 4);
            fill = Parameters[8];

        }

        public override void Run()
        {
            int dif = Location1 - Location0;


            byte[] tofill = new byte[dif - 1];
            if (fill != 0x00)
            {
                for (int i = 0; i < dif; i++)
                {
                    tofill[i] = fill;
                }
            }
            mMemory.Set(Location0, tofill);
        }

    }
}
