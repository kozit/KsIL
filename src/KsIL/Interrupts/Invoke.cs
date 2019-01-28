using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace KsIL.Interrupts
{
    public class Invoke : Interrupt
    {

        enum codes {

            Load = 0x00,
            Call = 0x01,
            New  = 0x02,
            
        }

        public Invoke()
        {

            Code = 4;

        }

        public override void Run(byte[] Parameters, CPU CPU)
        {

            codes code = (codes)Parameters[0];

            switch (code)
            {

                case codes.Load:

                    Assembly.LoadFile(Encoding.UTF8.GetString(Parameters, 1, Parameters.Length - 2));
                                       
                break;

                case codes.Call:

                    object TheObject = CPU.Memory.GetObject(BitConverter.ToUInt32(Parameters, 1));

                    Type calledType = TheObject.GetType();
                    MethodInfo method = calledType.GetMethod(Encoding.UTF8.GetString(Parameters, 13, BitConverter.ToInt32(Parameters, 9)));
                    object result = method.Invoke(TheObject, null);

                    if (BitConverter.ToUInt32(Parameters, 1) != 0)
                    {

                        CPU.Memory.SetObject(BitConverter.ToUInt32(Parameters, 1), result);

                    }

                break;

                case codes.New:

                    CPU.Memory.SetObject(BitConverter.ToUInt32(Parameters, 1), Activator.CreateInstance(Type.GetType(Encoding.UTF8.GetString(Parameters, 1, Parameters.Length - 6)) ));

                break;

            }



        }

    }
}
