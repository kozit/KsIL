using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace KsIL
{
    public partial class Memory
    {

        public List<object> GetArray(uint Addr)
        {

            List<object> output = new List<object>();

            byte[] Header = GetData(Addr);

            uint Count = BitConverter.ToUInt32(Header, 0);
            
            for (int i = 0; i < Count; i++)
            {

                uint point = BitConverter.ToUInt32(Header, 4 + (4 * i));

                output.Add(GetObject(point));

            }

            return output;

        }

        public void SetArray(uint Addr, List<object> data)
        {

            SetArray(Addr, data.ToArray());

        }

        public void SetArray(uint Addr, object[] data)
        {
            
            uint headerOffset = (uint)data.Length * 4 + 4;
            uint dataOffset   = 0;
            
            List<byte> Header = new List<byte>();

            foreach (object item in data)
            {

                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                bf.Serialize(ms, item);
                
                SetData(headerOffset + dataOffset + Addr, ms.ToArray());

                Header.AddRange(BitConverter.GetBytes( (uint)(headerOffset + dataOffset + Addr) ));

                dataOffset += (uint)ms.ToArray().Length + 4;

            }

            SetData(Addr, Header.ToArray());
            
        }


    }
}
