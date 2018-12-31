using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public class CPU
    {

        public int InstructionPoint;
        public bool isRunning = true;
        public byte ConditionalResult;

        internal KsILVM VM;

        private Memory Memory;
        private List<Instruction> Instructions;
        private List<int> ReturnPointer;

        private int CodePointer;

        public CPU(KsILVM VM, Memory Memory, int CodePointer = 4)
        {

            this.Memory = Memory;
            InstructionPoint = 0;

            byte[] code = this.Memory.GetDataPionter(CodePointer);

            ReturnPointer = new List<int>();

            Instructions = new List<Instruction>();
                 
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

        public byte[] Read(byte[] read)
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


        public byte[] getPart(byte[] data, int startindex, int endindex = -1)
        {

            if (endindex == -1)
            {
                endindex = data.Length;
            }

            byte[] output = new byte[endindex - startindex];

            Array.Copy(data, startindex, output, 0, endindex - startindex);

            return output;

            //List<byte> output = new List<byte>();

            //for (int i = 0; i < endindex; i++)
            //{

            //    output.Add(data[i]);

            //}

            //return output.ToArray();

        }

        public void ReloadCode()
        {



        }

        public void Tick()
        {

            if (!isRunning)
                return;

            if (InstructionPoint == Instructions.Count)
            {

                if (VM.cpu[0] == this)
                {

                    Memory.Set(Memory.PROGRAM_RUNNING, 0x00);

                }


                return;
            }

            Instruction instruction = Instructions[InstructionPoint];
            
            int a, b, point, pointer;
            byte[] data;

            switch (instruction.OPCode)
            {

                case OpCode.Interrupt:

                    a = BitConverter.ToInt16(getPart(instruction.data, 0, 1), 0);

                    data = getPart(instruction.data, 2);

                    foreach (Interrupt item in VM.Interrupts)
                    {

                        if (item.Code == a)
                        {

                            item.Run(data, this);
                            break;

                        }

                    }

                break;


                // Math

                case OpCode.Add:

                    a = BitConverter.ToInt32(getPart(instruction.data, 0, 3), 0);

                    b = BitConverter.ToInt32(getPart(instruction.data, 4, 7), 0);

                    Memory.SetData(b, BitConverter.GetBytes(a + b));

                break;

                case OpCode.Subtract:

                    a = BitConverter.ToInt32(getPart(instruction.data, 0, 3), 0);

                    b = BitConverter.ToInt32(getPart(instruction.data, 4, 7), 0);

                    Memory.SetData(b, BitConverter.GetBytes(a - b));

                break;

                case OpCode.Mutiply:

                    a = BitConverter.ToInt32(getPart(instruction.data, 0, 3), 0);

                    b = BitConverter.ToInt32(getPart(instruction.data, 4, 7), 0);

                    Memory.SetData(b, BitConverter.GetBytes(a * b));

                break;

                case OpCode.Divide:
                    
                    a = BitConverter.ToInt32(getPart(instruction.data, 0, 3), 0);

                    b = BitConverter.ToInt32(getPart(instruction.data, 4, 7), 0);

                    Memory.SetData(b, BitConverter.GetBytes(a / b));

                break;

                case OpCode.MathSpecial:

                    // {!} todo

                break;

                //Memory

                case OpCode.Move:

                    a = BitConverter.ToInt32(getPart(instruction.data, 0, 3), 0);

                    b = BitConverter.ToInt32(getPart(instruction.data, 4, 7), 0);

                    Memory.SetData(b, Memory.GetData(a));

                break;

                case OpCode.Clear:

                    a = BitConverter.ToInt32(getPart(instruction.data, 0, 3), 0);

                    Memory.Set(a, new byte[BitConverter.ToInt32(Memory.Get(a, 4), 0) + 4]);

                break;

                case OpCode.Store:

                    point = BitConverter.ToInt32(getPart(instruction.data, 0, 3), 0);

                    data = getPart(instruction.data, 4);

                    Memory.SetData(point, data);

                break;

                case OpCode.Store_Pointer:

                    pointer = BitConverter.ToInt32(getPart(instruction.data, 0, 3), 0);

                    point = BitConverter.ToInt32(getPart(instruction.data, 4, 7), 0);

                    data = getPart(instruction.data, 8);

                    Memory.SetDataPionter(point, data);

               break;
                                                         

                case OpCode.Goto:

                    a = BitConverter.ToInt32(instruction.data, 0);

                    InstructionPoint = a;
                    
                break;
                    
                case OpCode.Call:

                    point = BitConverter.ToInt32(Read(instruction.data), 0);

                    ReturnPointer.Insert(0, point);

                    InstructionPoint = point;

                break;

                case OpCode.Return:

                    point = ReturnPointer[0];
                    ReturnPointer.RemoveAt(0);
                    

                    InstructionPoint = (int)point;

                break;

            }

            InstructionPoint++;

        }


    }
}
