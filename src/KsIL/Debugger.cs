using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public class Debugger
    {

        public enum types
        {
            none = -1,
            code =  0,
            info =  1,
            cpu  =  2,
            vm   =  3 

        }

        public static int _type = (int)types.none;

        public static void Log(bool data, string name = "", int type = 1)
        {

            Log(data.ToString(), name, type);

        }

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

                output += itema.ToString("X") + " ";

            }

            output += "(" + Encoding.UTF8.GetString(data) + ")";

            Log(output, name, type);


        }

        public static void Log(string data, string name = "", int type = 1)
        {

            if (!(_type >= type))
                return;

            ConsoleColor consoleColor = Console.ForegroundColor;
            
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[{0}]", ((types)type).ToString());
            Console.ForegroundColor = ConsoleColor.Red;
            if (name != "")
            {
                Console.Write("{0}:", name);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{0}", data);
            Console.ForegroundColor = consoleColor;


        }

    }
}
