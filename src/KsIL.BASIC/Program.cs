using System;
using System.Collections.Generic;

namespace KsIL.BASIC
{
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
  
            for (int i = 1; i < Lines.Length; i++)
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

                for (int t = 1; t < Tokens.Length; t++)
                    output.AddRange(TokenDecode(Tokens[t]));

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