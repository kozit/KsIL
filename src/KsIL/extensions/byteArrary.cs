using System;
using System.Linq;

namespace KsIL.extensions
{
#pragma warning disable IDE1006 // Naming Styles
    public static class byteArrary
#pragma warning restore IDE1006 // Naming Styles
    {

        public static byte[] GetData(this byte[] data, int Offset = 0) {

            return data.GetDataPrefix(Offset).Data;

        }

        public static (byte[] Data, int PrefixSize) GetDataPrefix(this byte[] data, int Offset = 0, CPU CPU = null) {

            if      (data[0] == (byte)DataTypes.bit16)
            {
                return (data.Skip(1).Take(BitConverter.ToUInt16(data, 1)).ToArray(), 3);
            }
            else if (data[0] == (byte)DataTypes.bit32)
            {
                return (data.Skip(1).Take((int)BitConverter.ToUInt32(data, 1)).ToArray(), 5);
            }
            else if (data[0] == (byte)DataTypes.bit64)
            {
                return (data.Skip(1).Take((int)BitConverter.ToUInt64(data, 1)).ToArray(), 9);
            }
            else if (data[0] == (byte)DataTypes.pointer)
            {
                return (CPU.GetBus.ReadData(BitConverter.ToUInt64(data, 1)), 9);
            }

            return (data.Skip(1).ToArray(), 1);
            
        }
    }
}
