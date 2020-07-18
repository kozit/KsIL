using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public class Debugger
    {

        public enum DebugLevel { 
            none = 0,
            error,
            info,
            cpu,
            ksil
        }

        public static DebugLevel CurrentDebugLevel = DebugLevel.none;

        public static void Log(string format, params string[] args) {
            if (CurrentDebugLevel >= DebugLevel.info) return;
            Console.WriteLine(String.Format("[info] {0}", format), args);
        }

        public static void Error(string format, params string[] args)
        {
            if (CurrentDebugLevel >= DebugLevel.error) return;
            Console.WriteLine(String.Format("[error] {0}", format), args);
        }

        public static void CPU(string format, params string[] args)
        {
            if (CurrentDebugLevel >= DebugLevel.cpu) return;
            Console.WriteLine(String.Format("[cpu] {0}", format), args);
        }

        public static void KsIL(string format, params string[] args)
        {
            if (CurrentDebugLevel >= DebugLevel.ksil) return;
            Console.WriteLine(String.Format("[ksil] {0}", format), args);
        }

    }
}
