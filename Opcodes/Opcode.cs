using System;
using System.Collections.Generic;
using System.Text;

namespace Chroma_Invaders.Opcodes
{
    public enum OperationTarget8 { A, F, B, C, D, E, H, L, M }
    public enum OperationTarget16 { PSW, B, D, H, M, SP, PC }

    public abstract class Opcode
    {
        public byte Code;
        public int Length = 1;
        public int Cycles = 4;

        protected Machine parent;

        public Opcode(Machine parent)
        {
            this.parent = parent;
        } 

        public abstract void Execute();
    }
}
