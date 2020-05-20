namespace Chroma_Invaders.Opcodes
{
    public class ImmediateMoveOperation : Opcode
    {
        private OperationTarget8 target;

        public ImmediateMoveOperation(Machine parent, byte opcode) : base(parent) {
            target = BitsToRegister((opcode & 0b111000) >> 3);
            Cycles = target == OperationTarget8.M ? 10 : 7;
            Length = 2;
        }

        private OperationTarget8 BitsToRegister(int bits)
        {
            switch(bits)
            {
                case 0b000: return OperationTarget8.B;
                case 0b001: return OperationTarget8.C;
                case 0b010: return OperationTarget8.D;
                case 0b011: return OperationTarget8.E;
                case 0b100: return OperationTarget8.H;
                case 0b101: return OperationTarget8.L;
                case 0b110: return OperationTarget8.M;
                case 0b111: return OperationTarget8.A;
                default: return OperationTarget8.A;
            }
        }

        public override void Execute()
        {
            byte srcVal = parent.Memory[parent.PC + 1];
            if (target == OperationTarget8.M)
                parent.Memory[(parent.Registers[Register.H] << 8) | (parent.Registers[Register.L])] = srcVal;
            else
                parent.Registers[(Register)target] = srcVal;
        }
    }
}
