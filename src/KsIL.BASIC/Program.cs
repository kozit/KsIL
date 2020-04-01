using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL.BASIC
{

    public class Debugger
    {

        public static void Log(bool data, string name = "")
        {

            Log(data.ToString(), name);

        }

        public static void Log(int data, string name = "")
        {

            Log(data.ToString(), name);

        }

        public static void Log(uint data, string name = "")
        {

            Log(data.ToString(), name);

        }

        public static void Log(byte[] data, string name = "")
        {

            string output = "";


            foreach (byte itema in data)
            {

                output += itema.ToString("X") + " ";

            }

            output += "(" + Encoding.UTF8.GetString(data) + ")";

            Log(output, name);


        }

        public static void Log(string[] data, string name = "")
        {

            string output = "";


            foreach (string itema in data)
            {

                output += itema + " _-_ ";

            }
            Log(output, name);
        }

        public static void Log(string data, string name = "")
        {
            
            ConsoleColor consoleColor = Console.ForegroundColor;

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

    class Program
    {

        static Dictionary<string, int> Labels = new Dictionary<string, int>();

        static int i = 1;
        static int LineCount = 0;

        static void Main(string[] args)
        {

            string File = "";
            string Output = "";

            if (args.Length > 1)
            {

                for (int i = 0; i < args.Length; i++)
                {

                    if (args[i] == "-o")
                    {

                        Output = args[i + 1];
                        i++;

                    }
                    else if (args[i] == "-file")
                    {

                        File = args[i + 1];
                        i++;

                    }

                }

            }
            else if (args.Length == 1)
            {

                File = args[0];

            }
            else
            {

                throw new ArgumentException("need to set a file");

            }

            if (Output == "")
            {
                Output = File.Split('.')[0];
            }


            List<byte> output = new List<byte>();

            string[] Lines = System.IO.File.ReadAllLines(args[0]);
            Debugger.Log(Lines, "File");
            for (int i = 0; i < Lines.Length; i++)
            {

                if (Lines[i].Trim() == "")
                {
                    continue;
                }

                if (Lines[i].Trim().StartsWith("#"))
                {
                    continue;
                }


                string[] Tokens = getTokens(Lines[i]);
                Debugger.Log(Lines[i]);
                Debugger.Log(Tokens);

                List<byte> log = new List<byte>();

                for (int t = 1; t < Tokens.Length; t++)
                {
                    output.AddRange(TokenDecode(Tokens[t]));
                    log.AddRange(TokenDecode(Tokens[t]));
                }
                Debugger.Log(log.ToArray());

                LineCount++;
                output.AddRange(new byte[] { 0x00, 0xFF, 0x00, 0xFF});

            }

            System.IO.File.WriteAllBytes(Output + ".KsIL", output.ToArray());
            Console.WriteLine("done");
            Console.ReadKey(true);

        }
        
        static byte[] TokenDecode(string Token)
        {

            List<byte> output = new List<byte>();
                        
            if (Token.StartsWith("i:"))
            {

                output.AddRange(BitConverter.GetBytes(Int32.Parse(Token.Remove(0, 2))));

            }
            else if (Token.StartsWith("i16:"))
            {

                output.AddRange(BitConverter.GetBytes(Int16.Parse(Token.Remove(0, 4))));

            }
            else if (Token.StartsWith("ui:"))
            {

                output.AddRange(BitConverter.GetBytes(UInt32.Parse(Token.Remove(0, 3))));

            }
            else if (Token.StartsWith("ui16:"))
            {

                output.AddRange(BitConverter.GetBytes(UInt16.Parse(Token.Remove(0, 5))));

            }
            else if (Token.StartsWith("0x"))
            {
                output.Add(Convert.ToByte(Token.Remove(0, 2), 16));
            }
            else
            {
                output.AddRange(System.Text.Encoding.UTF8.GetBytes(Token));
            }

            return output.ToArray();

        }

        static string[] getTokens(string s, char split = ' ', char quote = '"')
        {
            
            bool isinquotes = false;
            
            List<string> tokens = new List<string> { "" };

            for (int i = 0; i < s.Length; i++)
            {
                
                if (s[i] == quote && s[i - 1] != '\\')
                {

                    isinquotes = !isinquotes;

                }
                else if (s[i] == split && isinquotes == false)
                {

                    tokens.Add("");

                }
                else
                {

                    tokens[tokens.Count - 1] += s[i];

                }

            }

            return tokens.ToArray();

        }
        
    }
}