using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public enum DataTypes: byte
    {

        bit     = 0x00,
        bit16   = 0x01,
        bit32   = 0x02,
        bit64   = 0x03,
        pointer = 0x04,
                
        EX    = 0xFE,
        NUll  = 0xFF

    }
}
