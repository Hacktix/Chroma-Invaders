namespace Chroma_Invaders.Opcodes
{
    public class ComplementCarryOperation : Opcode
    {
        public ComplementCarryOperation(Machine parent) : base(parent) { }

        public override void Execute()
        {
            parent.Registers[Register.F] ^= 1;
        }
    }
}
