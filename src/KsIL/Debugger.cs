using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public class Debugger
    {

        public enum DebugTypes
        {
            none = -1,
            code =  0,
            info =  1,
            cpu  =  2,
            vm   =  3 

        }

        public static DebugTypes _type = DebugTypes.none;

        public static void Log(bool data, string name = "", DebugTypes type = DebugTypes.info)
        {

            Log(data.ToString(), name, type);

        }

        public static void Log(int data, string name = "", DebugTypes type = DebugTypes.info)
        {

            Log(data.ToString(), name, type);

        }

        public static void Log(uint data, string name = "", DebugTypes type = DebugTypes.info)
        {

            Log(data.ToString(), name, type);

        }

        public static void Log(byte[] data, string name = "", DebugTypes type = DebugTypes.info)
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

        public static void Log(string data, string name = "", DebugTypes type = DebugTypes.info)
        {

            if (!(_type >= type))
                return;

            ConsoleColor consoleColor = Console.ForegroundColor;
            
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[{0}]", (type).ToString());
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
