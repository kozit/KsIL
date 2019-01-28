using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public class Debugger
    {

        public enum types
        {

            code = 0,
            info = 1,
            vm = 2

        }

        public static int _type = 0;

        public static void Log(int data, string name = "", int type = 1)
        {

            Log(data.ToString(), name, type);

        }

        public static void Log(uint data, string name = "", int type = 1)
        {

            Log(data.ToString(), name, type);

        }

        public static void Log(byte[] data, string name = "", int type = 1)
        {

            if (!(_type >= type))
                return;

            string output = "";


            foreach (byte itema in data)
            {

                output += itema + " ";

            }

            Log(output, name, type);


        }

        public static void Log(string data, string name = "", int type = 1)
        {

            if (!(_type >= type))
                return;

            string msg;

            if (name == "")
            {
                msg = data;
            }
            else
            {
                msg = name + " : " + data;
            }

            Console.WriteLine("[{1}] {0}", msg, ((types)type).ToString());

        }

    }
}
