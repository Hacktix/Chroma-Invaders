namespace Chroma_Invaders.Opcodes
{
    public class AccMoveOperation : Opcode
    {
        private OperationTarget16 target;
        private bool store;

        public AccMoveOperation(Machine parent, byte opcode) : base(parent) {
            target = (opcode & 0b10000) > 0 ? OperationTarget16.D : OperationTarget16.B;
            store = (opcode & 0b1000) == 0;
        }

        public override void Execute()
        {
            if (store)
                parent.Memory[parent.ReadRegister16(target)] = parent.Registers[Register.A];
            else
                parent.Registers[Register.A] = parent.Memory[parent.ReadRegister16(target)];
        }
    }
}
