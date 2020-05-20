namespace Chroma_Invaders.Opcodes
{
    public class ImmediateAccMoveOperation : Opcode
    {
        private bool store;

        public ImmediateAccMoveOperation(Machine parent, byte opcode) : base(parent) {
            store = (opcode & 0b1000) == 0;
        }

        public override void Execute()
        {
            ushort memaddr = (ushort)(parent.Memory[parent.PC + 1] + (parent.Memory[parent.PC + 2] << 8));
            if (store)
                parent.Memory[memaddr] = parent.Registers[Register.A];
            else
                parent.Registers[Register.A] = parent.Memory[memaddr];
        }
    }
}
