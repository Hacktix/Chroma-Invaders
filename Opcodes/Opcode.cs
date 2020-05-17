using System;
using System.Collections.Generic;
using System.Text;

namespace Chroma_Invaders.Opcodes
{
    public abstract class Opcode
    {
        public byte Code;
        public byte Length = 1;
        public byte Cycles = 4;

        protected byte[] Parameters;

        public void LoadParameters(byte[] parameters)
        {
            Parameters = new byte[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
                Parameters[i] = parameters[i];
        } 

        public abstract void Execute();
    }
}
