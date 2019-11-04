using System;
using System.Collections.Generic;

using System.Threading;

namespace KsIL
{
    public class CPU
    {

        public volatile uint InstructionPoint;
        public volatile bool isRunning = true;
        public byte ConditionalResult;

        public int ID;
        public int MadeBy;

        public Thread Thread;

        internal KsILVM VM;
        internal Memory Memory;

        private List<Instruction> Instructions;
        private List<uint> ReturnPointer;
        private Dictionary<byte[], uint> Labels;

        uint CodePointer;

        public CPU(KsILVM VM, Memory Memory, uint CodePointer = 4)
        {

            Debugger.Log("loading CPU", "init", 2);

            this.VM = VM;
            this.CodePointer = CodePointer;

            this.VM.cpu.Add(this);

            this.ID = VM.cpu.IndexOf(this);


            this.Memory = Memory;
            InstructionPoint = 0;
            

            ReturnPointer = new List<uint>();
            LoadInstructions(CodePointer);
            Debugger.Log("starting CPU", "init", 2);
            this.Thread = new Thread(new ThreadStart(this.Tick));
            this.Thread.Start();

        }

        public void ReloadInstructions()
        {

            LoadInstructions(CodePointer);

        }

        public void LoadInstructions(uint CodePointer)
        {

            Debugger.Log("loading instructions", "instructions", 2);

            byte[] code = this.Memory.GetDataPionter(this.CodePointer);

            Instructions = new List<Instruction>();
            Labels       = new Dictionary<byte[], uint>();

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
                Debugger.Log((OpCode)OpCode + "", "instruction:" + Instructions.Count, 2);
                Debugger.Log(data.ToArray(), "instruction:" + Instructions.Count, 2);

                if ((OpCode)OpCode == KsIL.OpCode.Label)
                {
                    Labels.Add(data.ToArray(), (uint)Instructions.Count);
                }
                               

            }

            Debugger.Log("loaded instructions", "instructions", 2);

        }

        public (byte[] Data, bool Offset) Read(byte[] read)
        {

            Debugger.Log(read, "Read:data");
            Debugger.Log(read[0], "Read:first");

            (byte[] Data, bool Offset) temp = this.read(read);

            Debugger.Log(temp.Data, "Read:data_out");
            Debugger.Log(temp.Offset, "Read:offset");


            return temp;

        }

        private (byte[] Data,  bool Offset) read(byte[] read)
        {

            

            switch (read[0]) {

                case 0xF1:
                    return (getPart(read, 1), true);

                case 0xFF:
                    return (Memory.GetDataPionter(BitConverter.ToUInt32(getPart(read, 1), 0)), true);

                case 0xFE:
                    return (Memory.Get(BitConverter.ToUInt32(getPart(read, 1), 0), 8), true);


            }

            return (read, false);

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

        public void Call(uint point)
        {

            Debugger.Log(point, "Call:" + InstructionPoint);

            ReturnPointer.Insert(0, InstructionPoint + 1);
            InstructionPoint = point;

        }

        public void Tick()
        {
            Debugger.Log("ID:" + this.ID, "CPU", 2);
            if (!isRunning || !VM.isRunning)
            {
                Thread.Sleep(100);
                return;
            }
                

            if (InstructionPoint == Instructions.Count)
            {

                if (VM.cpu[0] == this)
                {

                    Memory.Set(Memory.Pointer.PROGRAM_RUNNING, 0x00);

                    Debugger.Log("Main CPU has stoped", "Tick");

                }

                return;
            }

            Instruction instruction = Instructions[(int)InstructionPoint];

            Debugger.Log(InstructionPoint, "Instruction:point");

            Debugger.Log(instruction.OPCode.ToString(), "Instruction:OpCode");

            Debugger.Log(instruction.data, "Instruction:Data");

            int a, b;
            uint ua, ub;
            uint point, pointer;
            int offset = 0;
            (byte[] Data, bool Offset) ReadTemp;
            byte[] data;

            switch (instruction.OPCode)
            {

                case OpCode.Interrupt:

                    a = BitConverter.ToUInt16(getPart(instruction.data, 0, 2), 0);

                    data = getPart(instruction.data, 2);

                    Debugger.Log(a.ToString(), "Interrupt:ID");
                    Debugger.Log(data, "Interrupt:Data");

                    if (VM.Interrupts.ContainsKey((UInt16)a))
                    {
                        VM.Interrupts[(UInt16)a].Run(data, this);
                    }
                    else
                    {
                        if (VM.Interrupt_CallBack.ContainsKey((UInt16)a))
                        {

                            foreach ((CPU, uint) item in VM.Interrupt_CallBack[(UInt16)a])
                            {

                                item.Item1.Call(item.Item2);

                            }

                        }
                    }
                    
                    break;

                case OpCode.Interrupt_CallBack:
                    
                    //(CPU, uint) temp = Interrupt_CallBack[BitConverter.ToUInt16(getPart(instruction.data, 0, 2), 0)];

                    UInt16 inter_code = BitConverter.ToUInt16(getPart(instruction.data, 0, 2), 0);

                    if (!VM.Interrupt_CallBack.ContainsKey(inter_code))
                    {
                        VM.Interrupt_CallBack.Add(inter_code, new List<(CPU, uint)>());
                    }

                    VM.Interrupt_CallBack[inter_code].Add((this, BitConverter.ToUInt32(getPart(instruction.data, 3), 0)));
                    
                    break;

                // Math

                case OpCode.Add:

                    a = BitConverter.ToInt32(getPart(instruction.data, 0, 4), 0);

                    ub = BitConverter.ToUInt32(getPart(instruction.data, 4, 4), 0);

                    Debugger.Log("a:" + a + " b:" + ub, "Add:data");

                    Memory.SetData(ub, BitConverter.GetBytes(a + ub));

                    break;

                case OpCode.Subtract:

                    ReadTemp = Read(getPart(instruction.data, 0, 5));

                    if (ReadTemp.Offset)
                        offset++;

                    a = BitConverter.ToInt32(ReadTemp.Data, 0);

                    ub = BitConverter.ToUInt32(Read(getPart(instruction.data, 4 + offset, 5)).Data, 0);

                    Debugger.Log("a:" + a + " b:" + ub, "Subtract:data");

                    Memory.SetData(ub, BitConverter.GetBytes(a - ub));

                    break;

                case OpCode.Mutiply:
                    
                    ReadTemp = Read(getPart(instruction.data, 0, 5));

                    if (ReadTemp.Offset)
                        offset++;
                    
                    a = BitConverter.ToInt32(ReadTemp.Data, 0);

                    ub = BitConverter.ToUInt32(Read(getPart(instruction.data, 4 + offset, 5)).Data, 0);

                    Debugger.Log("a:" + a + " b:" + ub, "Mutiply:data");

                    Memory.SetData(ub, BitConverter.GetBytes(a * ub));

                    break;

                case OpCode.Divide:

                    ReadTemp = Read(getPart(instruction.data, 0, 5));

                    if (ReadTemp.Offset)
                        offset++;

                    a = BitConverter.ToInt32(ReadTemp.Data, 0);

                    ub = BitConverter.ToUInt32(Read(getPart(instruction.data, 4 + offset, 5)).Data, 0);

                    Debugger.Log("a:" + a + " b:" + ub, "Divide:data");

                    Memory.SetData(ub, BitConverter.GetBytes(a / ub));

                    break;

                case OpCode.MathSpecial:

                    // {!} todo

                    break;

                //Memory

                case OpCode.Move:

                    ReadTemp = Read(getPart(instruction.data, 0, 5));

                    if (ReadTemp.Offset)
                        offset++;

                    ua = BitConverter.ToUInt32(ReadTemp.Data, 0);

                    ub = BitConverter.ToUInt32(Read(getPart(instruction.data, 4 + offset, 5)).Data, 0);

                    Memory.SetData(ub, Memory.GetData(ua));

                    break;

                case OpCode.Clear:

                    Memory.ClearData(BitConverter.ToUInt32(getPart(instruction.data, 0, 4), 0));

                    break;

                case OpCode.Store:

                    point = BitConverter.ToUInt32(Read(getPart(instruction.data, 0, 5)).Data, 0);

                    data = getPart(instruction.data, 4);

                    Memory.SetData(point, data);

                    break;

                case OpCode.Store_Pointer:

                    pointer = BitConverter.ToUInt32(getPart(instruction.data, 0, 5), 0);

                    point = BitConverter.ToUInt32(getPart(instruction.data, 4 + offset, 5), 0);

                    data = getPart(instruction.data, 8);

                    Memory.SetDataPionter(point, data);

                    break;


                case OpCode.Goto:

                    point = BitConverter.ToUInt32(Read(instruction.data).Data, 0);

                    Debugger.Log(point, "Goto");

                    InstructionPoint = point;

                    break;

                case OpCode.Call:

                    point = BitConverter.ToUInt32(Read(instruction.data).Data, 0);
                    Call(point);


                    break;

                case OpCode.Return:

                    point = ReturnPointer[0];
                    ReturnPointer.RemoveAt(0);

                    Debugger.Log(point, "Return");

                    InstructionPoint = point;

                    break;

            }

            InstructionPoint++;

            Tick();

        }


    }
}
