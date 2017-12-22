using System;
using System.Collections.Generic;

namespace KsIL.BASIC
{
    class Program
    {

        static Dictionary<string, int> Labels = new Dictionary<string, int>();

        static int i = 1; 

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

            Console.WriteLine(Lines[0]);

            if (Lines[0].Trim() == "8bit")
            {
                Console.WriteLine("8bit mode");
                output.Add(0x00);
            }
            else if (Lines[0].Trim() == "16bit")
            {
                Console.WriteLine("16bit mode");
                output.Add(0x01);
            }
            else if (Lines[0].Trim() == "32bit")
            {
                Console.WriteLine("32bit mode");
                output.Add(0x02);
            }
            else if (Lines[0].Trim() == "64bit")
            {
                Console.WriteLine("64bit mode");
                output.Add(0x03);
            }

            Console.WriteLine(output[0]);

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

                    output.AddRange(BitConverter.GetBytes(Int16.Parse(Tokens[1])));

                    if (Int16.Parse(Tokens[1]) == (Int16) 1)
                    {

                        if (byte.Parse(Tokens[2]) != 0x03)
                        {

                            output.AddRange(IsPointer(Tokens[3]));

                        }

                    }
                    else if (Int16.Parse(Tokens[1]) == (Int16) 2)
                    {
                    } 

                }
                else if (Tokens[0] == "STR")
                {

                    output.Add(0x01);

                    

                    byte[] content = IsPointer(Tokens[1]);

                    output.AddRange(BitConverter.GetBytes(content.Length));
                    output.AddRange(content);
                 
                    
                    output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[2])));

                }
                else if (Tokens[0] == "DST")
                {

                    output.Add(0x02);

                    output.AddRange(BitConverter.GetBytes(System.Text.Encoding.UTF8.GetBytes(Tokens[1]).Length));
                    output.AddRange(System.Text.Encoding.UTF8.GetBytes(Tokens[1]));


                    output.AddRange(IsPointer(Tokens[2]));

                }
                else if (Tokens[0] == "RDI")
                {

                    output.Add(0x03);

                    output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[1])));
                    
                    output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[2])));

                }
                else if (Tokens[0] == "DRI")
                {

                    output.Add(0x04);

                    output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[1])));

                    output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[2])));

                }
                else if (Tokens[0] == "FLL")
                {

                    output.Add(0x05);

                    output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[1])));

                    output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[2])));

                    output.Add(byte.Parse(Tokens[3]));

                }
                else if (Tokens[0] == "CLR")
                {

                    output.Add(0x06);

                    output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[1])));
                    
                }
                else if (Tokens[0] == "TEQ")
                {

                    output.Add(0x10);

                    output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[1])));

                    output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[2])));

                }
                else if (Tokens[0] == "TGT")
                {

                    output.Add(0x11);

                    output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[1])));

                    output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[2])));

                }
                else if (Tokens[0] == "JIT")
                {

                    output.Add(0x12);
                    if (Tokens[1].StartsWith(":"))
                    {

                        output.AddRange(BitConverter.GetBytes(Labels[Tokens[1].Remove(0, 1)]));

                    }
                    else
                    {

                        output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[1])));

                    }

                }
                else if (Tokens[0] == "JIF")
                {

                    output.Add(0x13);
                    if (Tokens[1].StartsWith(":"))
                    {

                        output.AddRange(BitConverter.GetBytes(Labels[Tokens[1].Remove(0, 1)]));

                    }
                    else
                    {

                        output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[1])));

                    }

                }
                else if (Tokens[0] == "JMP")
                {

                    output.Add(0x20);

                    output.Add(byte.Parse(Tokens[1]));

                    if (Tokens[2].StartsWith(":"))
                    {

                        output.AddRange(BitConverter.GetBytes(Labels[Tokens[2].Remove(0, 1)]));

                    }
                    else
                    {

                        output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[2])));

                    }

                }
                else if (Tokens[0] == "RTN")
                {

                    output.Add(0x21);
                    
                }
                else if (Tokens[0] == "ADD")
                {

                    output.Add(0x30);

                    output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[1])));

                    output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[2])));

                }
                else if (Tokens[0] == "SUB")
                {

                    output.Add(0x31);

                    output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[1])));

                    output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[2])));

                }
                else if (Tokens[0] == "MUL")
                {

                    output.Add(0x32);

                    output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[1])));

                    output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[2])));

                }
                else if (Tokens[0] == "DIV")
                {

                    output.Add(0x33);

                    output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[1])));

                    output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[2])));

                }

                output.AddRange(new byte[] { 0x00, 0x00, 0xFF, 0x00, 0xFF});

            }

            System.IO.File.WriteAllBytes(Output + ".KsIL", output.ToArray());

        }

        static string[] Pre(string[] input)
        {
            List<string> output = new List<string>();

            for (int i = 1; i < input.Length; i++)
            {

                if (input[i].StartsWith(':'))
                {

                    Labels.Add(input[i].Remove(0,1) , output.Count - 1);

                }
                else
                {

                    output.Add(input[i]);

                }

            }

                return output.ToArray();

        }

        static byte[] IsPointer(string input)
        {

            List<byte> output = new List<byte>();

            if (input[0] == '^')
            {

                output.Add(0xFF);

                output.AddRange(BitConverter.GetBytes(Int32.Parse(input.Remove(0, 1))));

            }
            else if (input[0] == '%')
            {

                output.Add(0xFE);

                output.AddRange(BitConverter.GetBytes(Int32.Parse(input.Remove(0, 1))));

            }
            else if (input[0] == '#')
            {

                if (input[1] == '+')
                {

                    string temp = input.Remove(1);
                    int t = int.Parse(temp) - 2;

                    t += i;

                    output.AddRange(BitConverter.GetBytes(t));

                }
                else if (input[1] == '-')
                {

                    string temp = input.Remove(1);
                    int t = int.Parse(temp) - 2;

                    t -= i;

                    output.AddRange(BitConverter.GetBytes(t));


                }
                else
                {

                    output.AddRange(BitConverter.GetBytes(i));

                }

            }
            else
            {

                if ((byte) input[0] == 0xFF)
                {

                    output.Add(0xF1);

                }
                else if ((byte) input[0] == 0xFE)
                {

                    output.Add(0xF1);

                }

                output.AddRange(System.Text.Encoding.UTF8.GetBytes(input));

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