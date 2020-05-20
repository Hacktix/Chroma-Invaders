namespace Chroma_Invaders.Opcodes
{
    public class OrOperation : Opcode // ORA
    {
        private OperationTarget8 source;

        public OrOperation(Machine parent, byte opcode) : base(parent) {
            source = BitsToRegister(opcode & 0b111);
            Cycles = source == OperationTarget8.M ? 7 : 4;
        }

        private OperationTarget8 BitsToRegister(int bits)
        {
            switch (bits)
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
            parent.Registers[Register.A] |= source == OperationTarget8.M ? parent.Memory[(parent.Registers[Register.H] << 8) | (parent.Registers[Register.L])] : parent.Registers[(Register)source];

            // Set Flags
            parent.SetFlag(Flag.Carry, false);
            parent.SetFlag(Flag.Parity, parent.Registers[Register.A] % 2 == 0);
            parent.SetFlag(Flag.Zero, parent.Registers[Register.A] == 0);
            parent.SetFlag(Flag.Sign, (parent.Registers[Register.A] & 128) > 0);
        }
    }
}
