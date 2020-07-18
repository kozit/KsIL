using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace KsIL
{
    public class CPU
    {

        #region Registers

        public UInt64 PC;
        public List<UInt64> CallStack = new List<UInt64>();


        public Register Registers;
        public List<Register> RegisterStack = new List<Register>();

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
                buffer.Add(GetBus.Read(PC));
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
