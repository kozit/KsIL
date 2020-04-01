using System;
using System.Collections.Generic;

using System.Threading;

namespace KsIL
{
    public class CPU
    {

        public volatile uint InstructionPoint;
        public volatile uint ProgramAddress;
        public volatile bool isRunning = true;
        public byte ConditionalResult;

        public int ID;
        public int MadeBy;

        public Thread Thread;

        internal KsILVM VM;
        internal Memory Memory;

        private Instruction Instruction;
        private List<uint> ReturnPointer;
        private Dictionary<byte[], uint> Labels;
        public List<byte[]> Registers;

        uint CodePointer;

        public CPU(KsILVM VM, Memory Memory, uint CodePointer = 4)
        {
            this.Registers = new List<byte[]>() { 
                
                //bool
                new byte[1],
                new byte[1],
                
                // 
                new byte[8],
                new byte[8],
                new byte[8],
                new byte[8],
                new byte[8],
                new byte[8],
                
                //
                new byte[16],
                new byte[16],
            };
            Debugger.Log("loading CPU", "init", Debugger.DebugTypes.cpu);

            this.VM = VM;
            this.CodePointer = CodePointer;

            this.VM.cpu.Add(this);

            this.ID = VM.cpu.IndexOf(this);
            
            this.Memory = Memory;
            InstructionPoint = 0;
            
            ReturnPointer = new List<uint>();
            Instruction = new Instruction();
            Instruction.CPU = this;
            //LoadInstructions(CodePointer);
            ProgramAddress = CodePointer;
            Debugger.Log("starting CPU", "init", Debugger.DebugTypes.cpu);
            this.Thread = new Thread(new ThreadStart(this.Tick));
            this.Thread.Start();

        }

        public void Call(uint point)
        {

            Debugger.Log(point, "Call:" + InstructionPoint);

            ReturnPointer.Insert(0, InstructionPoint + 1);
            InstructionPoint = point;

        }

        public void Tick()
        {
            Debugger.Log("ID:" + this.ID, "CPU", Debugger.DebugTypes.cpu);
            if (!isRunning || Memory.Get(0x00) != 0xFF)
            {
                Thread.Sleep(100);
                return;
            }

            List<byte> instructionBuffer = new List<byte>();
            uint InstructionStartAddress = ProgramAddress;
            while (Memory.Get(ProgramAddress, 4) != new byte[]{ 0x00, 0xff, 0x00, 0xff})
            {

                instructionBuffer.Add(Memory.Get(ProgramAddress));
                ProgramAddress++;
            }

            this.Instruction.OPCode = (OpCode)instructionBuffer[0];
            instructionBuffer.RemoveAt(0);
            if ((int)this.Instruction.OPCode >= 0xFF)
            {
                instructionBuffer.RemoveAt(0);
            }

            this.Instruction.data = instructionBuffer.ToArray();

            Instruction instruction = this.Instruction;

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

                    a = BitConverter.ToUInt16(instruction.get(2), 0);

                    data = instruction.get(2);

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

                    UInt16 inter_code = instruction.getUInt16();

                    if (!VM.Interrupt_CallBack.ContainsKey(inter_code))
                    {
                        VM.Interrupt_CallBack.Add(inter_code, new List<(CPU, uint)>());
                    }

                    VM.Interrupt_CallBack[inter_code].Add((this, instruction.getUInt32()));
                    
                    break;

                // Math

                case OpCode.Add:

                    a = instruction.getInt32();

                    ub = instruction.getUInt32();

                    Debugger.Log("a:" + a + " b:" + ub, "Add:data");

                    Memory.SetData(ub, BitConverter.GetBytes(a + ub));

                    break;

                case OpCode.Subtract:

                    a = instruction.getInt32();

                    ub = instruction.getUInt32();

                    Debugger.Log("a:" + a + " b:" + ub, "Subtract:data");

                    Memory.SetData(ub, BitConverter.GetBytes(a - ub));

                    break;

                case OpCode.Mutiply:

                    a = instruction.getInt32();

                    ub = instruction.getUInt32();

                    Debugger.Log("a:" + a + " b:" + ub, "Mutiply:data");

                    Memory.SetData(ub, BitConverter.GetBytes(a * ub));

                    break;

                case OpCode.Divide:

                    a = instruction.getInt32();

                    ub = instruction.getUInt32();

                    Debugger.Log("a:" + a + " b:" + ub, "Divide:data");

                    Memory.SetData(ub, BitConverter.GetBytes(a / ub));

                    break;

                case OpCode.MathSpecial:

                    // {!} todo

                    break;
                //Memory

                case OpCode.Move:

                    ua = instruction.getUInt32();

                    ub = instruction.getUInt32();

                    Memory.SetData(ub, Memory.GetData(ua));

                    break;

                case OpCode.Clear:

                    Memory.ClearData(instruction.getUInt32());

                    break;

                case OpCode.Store:

                    point = instruction.getUInt32();

                    data = instruction.get();

                    Memory.SetData(point, data);

                    break;

                case OpCode.Store_Pointer:

                    pointer = instruction.getUInt32();

                    point = instruction.getUInt32();

                    data = instruction.get();

                    Memory.SetDataPionter(point, data);

                    break;


                case OpCode.Goto:

                    point = instruction.getUInt32();

                    Debugger.Log(point, "Goto");

                    InstructionPoint = point;

                    break;

                case OpCode.Call:

                    point = instruction.getUInt32();
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
