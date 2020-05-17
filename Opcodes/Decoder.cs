﻿namespace Chroma_Invaders.Opcodes
{
    public class Decoder
    {
        public static Opcode DecodeOpcode(Machine parent, byte code)
        {
            if ((code & 0b11000111) == 0b00000100) return new IncrementOperation(parent, code);
            if ((code & 0b11000111) == 0b00000101) return new DecrementOperation(parent, code);
            if (code == 0x37)                      return new SetCarryOperation(parent);
            if (code == 0x3F)                      return new ComplementCarryOperation(parent);
            else return new NoOperation(parent, code);
        }
    }
}