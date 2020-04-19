using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KsIL
{
    public class CPU
    {

        #region Registers

        public UInt64 PC;
        public struct Register
        {
            public Int64 A;
            public Int64 B;
            public Int64 X;
            public Int64 Y;

            public Int64 AX;

            public Int64 SP;
            public Int64 BP;
        }

        public Register Registers;

        public List<Register> RegisterStack = new List<Register>();

        public List<UInt64> CallStack = new List<UInt64>();

        #endregion

        public Dictionary<byte[], IInstruction> Instructions = new Dictionary<byte[], IInstruction>();

        public Bus GetBus;

        // current command buffer
        List<byte> buffer = new List<byte>();
        bool IsCommandEnd()
        {
            return buffer[^1] == 0xFF && buffer[^2] == 0x00 && buffer[^3] == 0xFF && buffer[^4] == 0x00;
        }
        public void Tick()
        {
            buffer.Clear();
            while (!IsCommandEnd())
            {
                buffer.Add(GetBus.ReadData(PC, 1)[0]);
                PC++;
            }

            Dictionary<byte[], IInstruction> lookup = new Dictionary<byte[], IInstruction>(Instructions);
            int i = 0;
            while (lookup.Count != 1)
            {
                lookup = lookup.Where(X => X.Key[i] == buffer[i]).ToDictionary(X=> X.Key, X => X.Value);

                i++;
            }
            lookup.First().Value.Run(this, buffer.Skip(i).ToArray());

        }

    }
}
