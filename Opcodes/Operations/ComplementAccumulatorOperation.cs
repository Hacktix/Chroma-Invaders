namespace Chroma_Invaders.Opcodes
{
    public class ComplementAccumulatorOperation : Opcode // CMA
    {
        public ComplementAccumulatorOperation(Machine parent) : base(parent) { }

        public override void Execute()
        {
            parent.Registers[Register.A] = (byte)~parent.Registers[Register.A];
        }
    }
}
