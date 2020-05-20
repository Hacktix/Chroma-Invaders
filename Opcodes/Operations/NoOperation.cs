namespace Chroma_Invaders.Opcodes
{
    public class NoOperation : Opcode // NOP
    {
        public NoOperation(Machine parent, byte opcode) : base(parent) { }

        public override void Execute() { }
    }
}
