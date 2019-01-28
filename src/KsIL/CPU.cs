using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL
{
    public class CPU
    {

        public uint InstructionPoint;
        public bool isRunning = true;
        public byte ConditionalResult;

        public int MadeBy;

        internal KsILVM VM;
        internal Memory Memory;

        private List<Instruction> Instructions;
        private List<uint> ReturnPointer;

        uint CodePointer;

        public CPU(KsILVM VM, Memory Memory, uint CodePointer = 4)
        {

            Debugger.Log("loading CPU", "", 2);
            
            this.VM = VM;
            this.CodePointer = CodePointer;

            this.Memory = Memory;
            InstructionPoint = 0;

            byte[] code = this.Memory.GetData(this.CodePointer);

            ReturnPointer = new List<uint>();

            Instructions = new List<Instruction>();
                 
            for (int i = 0; i < code.Length; i++)
            {

                byte OpCode = code[i];

                List<byte> data = new List<byte>();

                for (int offset = i + 1; offset < code.Length; offset++)
                {

                    if (code[offset] == 0x00 && code[offset + 1] == 0xFF && code[offset + 2] == 0x00 && code[offset + 3] == 0xFF)
                    {
                        i = offset + 3;
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
                return Memory.GetDataPionter(BitConverter.ToUInt32(getPart(read, 1),0));
            }

            if (read[0] == 0xFE)
            {
                return Memory.Get(BitConverter.ToUInt32(getPart(read, 1), 0),8);
            }

            return read;

        }
        
        public byte[] getPart(byte[] data, int startindex, int Length = -1)
        {

            if (Length == -1)
            {
                Length = data.Length - startindex;
            }

            byte[] output = new byte[Length];

            Debugger.Log(data, "getPart:data", 2);
            Debugger.Log(startindex, "getPart:startindex", 2);
            Debugger.Log(Length, "getPart:length", 2);

            Array.Copy(data, startindex, output, 0, Length);

            Debugger.Log(output, "getPart:output", 2);

            return output;

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

                    Debugger.Log("Main CPU has stoped", "Tick");

                }
                
                return;
            }

            Instruction instruction = Instructions[(int)InstructionPoint];

            Debugger.Log(InstructionPoint, "Instruction:point");

            Debugger.Log(instruction.OPCode.ToString(), "Instruction:OpCode");

            Debugger.Log(instruction.data, "Instruction:Data");

            int  a,  b;
            uint ua, ub;
            uint point, pointer;
            byte[] data;
            
            switch (instruction.OPCode)
            {

                case OpCode.Interrupt:

                    a = BitConverter.ToUInt16(getPart(instruction.data, 0, 2), 0);

                    data = getPart(instruction.data, 2);

                    Debugger.Log(a.ToString(), "Interrupt:ID");

                    VM.Interrupts[(UInt16)a].Run(data, this);
                    Debugger.Log(data, "Interrupt:Data");


                break;
                    
                case OpCode.Interrupt_CallBack:



                break;

                // Math

                case OpCode.Add:

                    a = BitConverter.ToInt32(getPart(instruction.data, 0, 4), 0);

                    ub = BitConverter.ToUInt32(getPart(instruction.data, 4, 4), 0);

                    Debugger.Log("a:" + a + " b:" + ub, "Add:data");

                    Memory.SetData(ub, BitConverter.GetBytes(a + ub));

                break;

                case OpCode.Subtract:

                    a = BitConverter.ToInt32(getPart(instruction.data, 0, 4), 0);

                    ub = BitConverter.ToUInt32(getPart(instruction.data, 4, 4), 0);

                    Debugger.Log("a:" + a + " b:" + ub, "Subtract:data");

                    Memory.SetData(ub, BitConverter.GetBytes(a - ub));

                break;

                case OpCode.Mutiply:

                    a = BitConverter.ToInt32(getPart(instruction.data, 0, 4), 0);

                    ub = BitConverter.ToUInt32(getPart(instruction.data, 4, 4), 0);

                    Debugger.Log("a:" + a + " b:" + ub, "Mutiply:data");

                    Memory.SetData(ub, BitConverter.GetBytes(a * ub));

                break;

                case OpCode.Divide:
                    
                    a = BitConverter.ToInt32(getPart(instruction.data, 0, 4), 0);

                    ub = BitConverter.ToUInt32(getPart(instruction.data, 4, 4), 0);

                    Debugger.Log("a:" + a + " b:" + ub, "Divide:data");

                    Memory.SetData(ub, BitConverter.GetBytes(a / ub));

                break;

                case OpCode.MathSpecial:

                    // {!} todo

                break;

                //Memory

                case OpCode.Move:

                    ua = BitConverter.ToUInt32(getPart(instruction.data, 0, 4), 0);

                    ub = BitConverter.ToUInt32(getPart(instruction.data, 4, 4), 0);

                    Memory.SetData(ub, Memory.GetData(ua));

                break;

                case OpCode.Clear:

                    Memory.ClearData(BitConverter.ToUInt32(getPart(instruction.data, 0, 4), 0));

                break;

                case OpCode.Store:

                    point = BitConverter.ToUInt32(getPart(instruction.data, 0, 4), 0);

                    data = getPart(instruction.data, 4);

                    Memory.SetData(point, data);

                break;

                case OpCode.Store_Pointer:

                    pointer = BitConverter.ToUInt32(getPart(instruction.data, 0, 4), 0);

                    point = BitConverter.ToUInt32(getPart(instruction.data, 4, 4), 0);

                    data = getPart(instruction.data, 8);

                    Memory.SetDataPionter(point, data);

               break;
                                                         

                case OpCode.Goto:

                    point = BitConverter.ToUInt32(instruction.data, 0);

                    Debugger.Log(point, "Goto");

                    InstructionPoint = point;
                    
                break;
                    
                case OpCode.Call:

                    point = BitConverter.ToUInt32(Read(instruction.data), 0);

                    Debugger.Log(point, "Call");

                    ReturnPointer.Insert(0, InstructionPoint + 1);

                    InstructionPoint = point;

                break;

                case OpCode.Return:

                    point = ReturnPointer[0];
                    ReturnPointer.RemoveAt(0);

                    Debugger.Log(point, "Return");

                    InstructionPoint = point;

                break;

            }

            InstructionPoint++;

        }


    }
}
