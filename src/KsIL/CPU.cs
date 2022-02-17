using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace KsIL
{
    public class CPU
    {
        [ThreadStatic]
        public static CPU Current;
        public Thread mThread;
        private bool hasStarted = false;
        private UInt64 _Interrupt = 0;
        public CPU() {
            mThread = new Thread(new ThreadStart(Ticker));
            
        }

        #region Registers

        public void Start() {
            mThread.Start();
        }
        public UInt64 PC;
        public List<UInt64> CallStack = new List<UInt64>();

        public Register Registers;
        public List<Register> RegisterStack = new List<Register>();

        #endregion

        public Dictionary<byte[], IInstruction> Instructions = new Dictionary<byte[], IInstruction>();

        public Bus GetBus { get; internal set; }

        // current command buffer
        List<byte> buffer = new List<byte>();
        bool IsCommandEnd()
        {
            return buffer[^1] == 0xFF && buffer[^2] == 0x00 && buffer[^3] == 0xFF && buffer[^4] == 0x00;
        }

        void DoTick()
        {
            CPU.Current = this;
            if(_Interrupt != 0)
            {
                PC = _Interrupt;
                _Interrupt = 0;
            }
            buffer.Clear();
            while (!IsCommandEnd())
            {
                buffer.Add(GetBus.Read(PC));
                PC++;
            }

            IEnumerable<KeyValuePair<byte[], IInstruction>> lookup = Instructions
                            .Where(x=> x.Key.Length <= buffer.Count); // this will not likely speed things up
                            
            int i = 0;
            while (lookup.Count() > 1)
            {
                lookup = lookup.Where(X => X.Key[i] == buffer[i]);
                i++;
            }
            if(lookup.Count() == 1)
                lookup.First().Value.Run(buffer.Skip(i).ToArray());
            
        }

        void Ticker()
        {
            CPU.Current = this;
            while(true) {

                DoTick();
            }

        }

        public void Interrupt(UInt64 PC)
        {

        }

    }
}
