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

            string[] Lines = Pre(System.IO.File.ReadAllLines(args[0]));
  
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

                if (Tokens[0] == "INT")
                {

                    output.Add(0x00);

                }
                else if (Tokens[0] == "STR")
                {

                    output.Add(0x01);

                }
                else if (Tokens[0] == "DST")
                {

                    output.Add(0x02);

                }
                else if (Tokens[0] == "RDI")
                {

                    output.Add(0x03);

                }
                else if (Tokens[0] == "DRI")
                {

                    output.Add(0x04);

                }
                else if (Tokens[0] == "FLL")
                {

                    output.Add(0x05);

                }
                else if (Tokens[0] == "CLR")
                {

                    output.Add(0x06);

                }
                else if (Tokens[0] == "TEQ")
                {

                    output.Add(0x10);

                }
                else if (Tokens[0] == "TGT")
                {

                    output.Add(0x11);

                }
                else if (Tokens[0] == "JIT")
                {

                    output.Add(0x12);

                }
                else if (Tokens[0] == "JIF")
                {

                    output.Add(0x13);

                }
                else if (Tokens[0] == "JMP")
                {

                    output.Add(0x20);

                }
                else if (Tokens[0] == "RTN")
                {

                    output.Add(0x21);

                }
                else if (Tokens[0] == "ADD")
                {

                    output.Add(0x30);

                }
                else if (Tokens[0] == "SUB")
                {

                    output.Add(0x31);

                }
                else if (Tokens[0] == "MUL")
                {

                    output.Add(0x32);

                }
                else if (Tokens[0] == "DIV")
                {

                    output.Add(0x33);

                }
                else if (Tokens[0] == "RAW")
                {

                }

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

            if (Token[0] == '^')
            {

                output.Add(0xFF);

                output.AddRange(BitConverter.GetBytes(Int32.Parse(Token.Remove(0, 1))));

            }
            else if (Token[0] == '%')
            {

                output.Add(0xFE);

                output.AddRange(BitConverter.GetBytes(Int32.Parse(Token.Remove(0, 1))));

            }
            else if (Token[0] == '#')
            {

                if (Token[1] == '+')
                {

                    string temp = Token.Remove(1);
                    int t = int.Parse(temp) - 2;

                    t += LineCount;

                    output.AddRange(MakeSafe(BitConverter.GetBytes(t)));

                }
                else if (Token[1] == '-')
                {

                    string temp = Token.Remove(1);
                    int t = int.Parse(temp) - 2;

                    t -= LineCount;

                    output.AddRange(MakeSafe(BitConverter.GetBytes(t)));


                }
                else
                {

                    output.AddRange(MakeSafe(BitConverter.GetBytes(LineCount)));

                }

            }
            else if (Token.StartsWith("i:"))
            {

                output.AddRange(MakeSafe(MakeInt(Token.Remove(0, 2))));

            }
            else if (Token.StartsWith("i16:"))
            {

                output.AddRange(MakeSafe(MakeInt(Token.Remove(0, 4),1)));

            }
            else
            {
                output.AddRange(MakeSafe(System.Text.Encoding.UTF8.GetBytes(Token)));
            }

            return output.ToArray();

        }

        static string[] Pre(string[] input)
        {
            List<string> output = new List<string>();
            return input;
            for (int i = 1; i < input.Length; i++)
            {

                if (input[i].StartsWith(":"))
                {

                    Labels.Add(input[i].Remove(0, 1), output.Count - 1);

                }
                else
                {

                    output.Add(input[i]);

                }

            }

            return output.ToArray();

        }

        static byte[] MakeByteArray(string input)
        {

            List<byte> output = new List<byte>();

            for (int offset = 0; offset < input.Length; offset++)
            {
                
                output.Add(Convert.ToByte(input.Substring(offset, 2), 16));

                offset++;

            }

            return output.ToArray();
            
        }

        static byte[] MakeInt(string t, int BitMode = 2)
        {

            List<byte> output = new List<byte>();
            if (BitMode == 0)
            {

                output.Add(byte.Parse(t));

            }
            else if (BitMode == 1)
            {

                output.AddRange(BitConverter.GetBytes(Int16.Parse(t)));

            }
            else if (BitMode == 2)
            {

                output.AddRange(BitConverter.GetBytes(Int32.Parse(t)));

            }
            else if (BitMode == 3)
            {

                output.AddRange(BitConverter.GetBytes(Int64.Parse(t)));

            }

            return output.ToArray();

        }

        static byte[] MakeUInt(string t, int BitMode = 2)
        {

            List<byte> output = new List<byte>();
            if (BitMode == 0)
            {

                output.Add(byte.Parse(t));

            }
            else if (BitMode == 1)
            {

                output.AddRange(BitConverter.GetBytes(UInt16.Parse(t)));

            }
            else if (BitMode == 2)
            {

                output.AddRange(BitConverter.GetBytes(UInt32.Parse(t)));

            }
            else if (BitMode == 3)
            {

                output.AddRange(BitConverter.GetBytes(UInt64.Parse(t)));

            }

            return output.ToArray();

        }

        static byte[] MakeSafe(byte[] input)
        {

            List<byte> output = new List<byte>();

            if (input[0] == 0xFF)
            {

                output.Add(0xF1);

            }
            else if (input[0] == 0xFE)
            {

                output.Add(0xF1);

            }

            output.AddRange(input);

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