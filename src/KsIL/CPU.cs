using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public class CPU
    {

        KsILVM VM;

        Memory Memory;

        List<Instruction> Instructions;

        int InstructionPoint;

        List<int> ReturnPointer;

        byte ConditionalResult;

        public CPU(KsILVM VM, Memory Memory, int CodePointer = 4)
        {

            this.Memory = Memory;
            InstructionPoint = 0;

            byte[] code = this.Memory.GetDataPionter(CodePointer);

            Instructions = new List<Instruction>();

            ReturnPointer = new List<int>();

            for (int i = 0; i < code.Length; i++)
            {

                byte OpCode = code[i];

                List<byte> data = new List<byte>();

                for (int offset = i + 1; offset < code.Length; offset++)
                {

                    if (code[offset] == 0x00 && code[offset + 1] == 0xFF && code[offset + 2] == 0x00 && code[offset + 3] == 0xFF)
                    {
                        break;
                    }

                    data.Add(code[offset]);

                }

                Instructions.Add(new Instruction() { OPCode = (OpCode)OpCode, data = data.ToArray() });
                               
            }


        }

        byte[] Read(byte[] read)
        {
            if (read[0] == 0xF1)
            {
                return getPart(read, 1);
            }

            if (read[0] == 0xFF)
            {
                return Memory.GetDataPionter(BitConverter.ToInt32(getPart(read, 1),0));
            }

            if (read[0] == 0xFE)
            {
                return Memory.Get(BitConverter.ToInt32(getPart(read, 1), 0),8);
            }

            return read;

        }

        byte[] getPart(byte[] data, int startindex, int endindex = -1)
        {

            if (endindex == -1)
            {
                endindex = data.Length;
            }

            List<byte> output = new List<byte>();

            for (int i = 0; i < endindex; i++)
            {

                output.Add(data[i]);

            }

            return output.ToArray();

        }

        public void Tick()
        {

            Instruction instruction = Instructions[InstructionPoint];
            
            int a, b, point;
            byte[] data;

            switch (instruction.OPCode)
            {

                case OpCode.Add:
                          
                    a = BitConverter.ToInt32(getPart(instruction.data, 0, 7), 0);

                    b = BitConverter.ToInt32(getPart(instruction.data, 8, 15), 0);

                    Memory.SetData(b, BitConverter.GetBytes(a + b));

                break;

                case OpCode.Subtract:
                    
                    a = BitConverter.ToInt32(getPart(instruction.data, 0, 7), 0);

                    b = BitConverter.ToInt32(getPart(instruction.data, 8, 15), 0);

                    Memory.SetData(b, BitConverter.GetBytes(a - b));

                break;

                case OpCode.Mutiply:
                    
                    a = BitConverter.ToInt32(getPart(instruction.data, 0, 7), 0);

                    b = BitConverter.ToInt32(getPart(instruction.data, 8, 15), 0);

                    Memory.SetData(b, BitConverter.GetBytes(a * b));

                break;

                case OpCode.Divide:
                    
                    a = BitConverter.ToInt32(getPart(instruction.data, 0, 7), 0);

                    b = BitConverter.ToInt32(getPart(instruction.data, 8, 15), 0);

                    Memory.SetData(b, BitConverter.GetBytes(a / b));

                break;

                case OpCode.MathSpecial:

                    // {!} todo

                break;

                case OpCode.Store:

                    point = BitConverter.ToInt32(getPart(instruction.data, 0, 7), 0);

                    data = getPart(instruction.data, 8);

                    Memory.SetData(point, data);

                    break;

                case OpCode.Return:

                    point = ReturnPointer[0];
                    ReturnPointer.RemoveAt(0);

                    InstructionPoint = (int)point;

                    break;

                case OpCode.Call:

                    point = BitConverter.ToInt32(Read(instruction.data), 0);

                    ReturnPointer.Insert(0, (int)point);

                    InstructionPoint = (int)point;

                    break;

            }

            InstructionPoint++;

        }


    }
}
