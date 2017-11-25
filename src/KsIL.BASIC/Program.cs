using System;
using System.Collections.Generic;
using System.Reflection;

namespace KsIL.BASIC
{
    class Program
    {
        static void Main(string[] args)
        {

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

                            output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[3])));

                        }

                    }
                }
                else if (Tokens[0] == "STR")
                {

                    output.Add(0x01);

                    output.AddRange(BitConverter.GetBytes(System.Text.Encoding.UTF8.GetBytes(Tokens[1]).Length));
                    output.AddRange(System.Text.Encoding.UTF8.GetBytes(Tokens[1]));
                 
                    
                    output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[2])));

                }
                else if (Tokens[0] == "DST")
                {

                    output.Add(0x02);

                    output.AddRange(BitConverter.GetBytes(System.Text.Encoding.UTF8.GetBytes(Tokens[1]).Length));
                    output.AddRange(System.Text.Encoding.UTF8.GetBytes(Tokens[1]));


                    output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[3])));

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

                    output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[1])));
                    
                }
                else if (Tokens[0] == "JIF")
                {

                    output.Add(0x13);

                    output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[1])));
                    
                }
                else if (Tokens[0] == "JMP")
                {

                    output.Add(0x20);

                    output.Add(byte.Parse(Tokens[1]));

                    output.AddRange(BitConverter.GetBytes(Int32.Parse(Tokens[2])));

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

                output.AddRange(new byte[] { 0x00, 0xFF, 0x00, 0xFF});

            }

            System.IO.File.Delete("out.KsIL");
            System.IO.File.WriteAllBytes("out.KsIL", output.ToArray());

        }


        static string[] getTokens(string s)
        {
            const char split = ' ';
            const char quote = '"';
            bool isinquotes = false;


            List<string> tokens = new List<string> { "" };

            foreach (char c in s)
            {

                if (c == quote)
                {
                    isinquotes = !isinquotes;
                }
                else if (c == split && isinquotes == false)
                {
                    tokens.Add("");
                }
                else
                {
                    tokens[tokens.Count - 1] += c;
                }

            }

            return tokens.ToArray();

        }


    }
}
