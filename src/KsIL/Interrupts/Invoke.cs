using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace KsIL.Interrupts
{
    public class Invoke : Interrupt
    {

        public Invoke(Memory memory) : base(memory)
        {
            Code = 0;
        }

        public override void Run(byte[] Parameters)
        {

            if (Parameters[0] == 0x00)
            {



            }

        }


        public static object CreateAndInvoke(string typeName, object[] constructorArgs, string methodName, object[] methodArgs)
        {

            Type type = Type.GetType(typeName);
            object instance = Activator.CreateInstance(type, constructorArgs);

            MethodInfo method = type.GetMethod(methodName);
            return method.Invoke(instance, methodArgs);

        }

    }
}
