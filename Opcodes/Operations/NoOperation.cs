﻿namespace Chroma_Invaders.Opcodes
{
    public class NoOperation : Opcode
    {
        public NoOperation(Machine parent, byte opcode) : base(parent) { }

        public override void Execute() { }
    }
}