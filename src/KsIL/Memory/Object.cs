using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace KsIL
{
    public partial class Memory
    {

        public void SetObject(uint Addr, object obj)
        {

            if (obj == null)
                return;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            SetData(Addr, ms.ToArray());

        }

        public object GetObject(uint Addr)
        {

            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            byte[] data = GetData(Addr);
            memStream.Write(data, 0, data.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            object obj = binForm.Deserialize(memStream);

            return obj;

        }

    }
}
