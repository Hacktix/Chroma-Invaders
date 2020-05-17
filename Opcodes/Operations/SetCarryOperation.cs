namespace Chroma_Invaders.Opcodes
{
    public class SetCarryOperation : Opcode
    {
        public SetCarryOperation(Machine parent) : base(parent) { }

        public override void Execute()
        {
            parent.Registers[Register.F] |= 1;
        }
    }
}
