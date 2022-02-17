using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public enum RegisterCode : byte
    {

        A = 0x00,
        B = 0x01,
        UA = 0x02,
        UB = 0x03,

        X = 0x10,
        Y = 0x11,

        AX = 0x20,

        SP = 0x30,
        BP = 0x31

    }

    public struct Register
    {
        public Int64 A;
        public Int64 B;
        public UInt64 UA;
        public UInt64 UB;
        
        public Int64 X;
        public Int64 Y;

        public Int64 AX;

        public Int64 SP;
        public Int64 BP;
    }
}
