using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{

    public class KsILTypes {
        public static Type GetType(DataTypes DataType) {
            switch (DataType)
            {

                case DataTypes.bit8:
                    return typeof(byte);
                case DataTypes.bit16:
                    return typeof(Int16);
                case DataTypes.bit32:
                    return typeof(Int32);
                case DataTypes.bit64:
                    return typeof(Int64);

                case DataTypes.unsignedbit8:
                    return typeof(byte);
                case DataTypes.unsignedbit16:
                    return typeof(UInt16);
                case DataTypes.unsignedbit32:
                    return typeof(UInt32);
                case DataTypes.unsignedbit64:
                    return typeof(UInt64);
                case DataTypes.string_type:
                    return typeof(String);

                default:
                    return null;
            }
        }
    }

    public enum DataTypes: byte
    {
        void_type = 0,
        bit8    = 1,
        bit16   = 2,
        bit32   = 4,
        bit64   = 8,
        unsignedbit8  = unsigned | bit8,
        unsignedbit16 = unsigned | bit16,
        unsignedbit32 = unsigned | bit32,
        unsignedbit64 = unsigned | bit64,
        

        pointer = 16,

        unsigned = 32,
                
        string_type = 64,
        
        EX    = 0xFE,
        NUll  = 0xFF

    }
}
