namespace Chroma_Invaders.Opcodes
{
    public class MoveOperation : Opcode // MOV
    {
        private OperationTarget8 source;
        private OperationTarget8 target;

        public MoveOperation(Machine parent, byte opcode) : base(parent) {
            source = BitsToRegister(opcode & 0b111);
            target = BitsToRegister((opcode & 0b111000) >> 3);
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
            byte srcVal = (source == OperationTarget8.M) ? parent.Memory[(parent.Registers[Register.H] << 8) | (parent.Registers[Register.L])] : parent.Registers[(Register)source];
            if (target == OperationTarget8.M)
                parent.Memory[(parent.Registers[Register.H] << 8) | (parent.Registers[Register.L])] = srcVal;
            else
                parent.Registers[(Register)target] = srcVal;
        }
    }
}
