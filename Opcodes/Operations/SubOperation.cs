namespace Chroma_Invaders.Opcodes
{
    public class SubOperation : Opcode // SUB, SBB
    {
        private OperationTarget8 source;
        private bool subBorrow;

        public SubOperation(Machine parent, byte opcode) : base(parent) {
            source = BitsToRegister(opcode & 0b111);
            subBorrow = (opcode & 0b1000) > 0;
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
            byte iv = parent.Registers[Register.A];
            byte subv = source == OperationTarget8.M ? parent.Memory[(parent.Registers[Register.H] << 8) | (parent.Registers[Register.L])] : parent.Registers[(Register)source];
            if (subBorrow) subv += (byte)(parent.Registers[Register.F] & 1);
            parent.Registers[Register.A] -= subv;

            // Set Flags
            parent.SetFlag(Flag.Carry, subBorrow ? (iv + (byte)(~subv)) < 256 : iv - subv < 0);
            parent.SetFlag(Flag.AuxiliaryCarry, ((iv & 0xF) - (subv & 0xF)) < 0);
            parent.SetFlag(Flag.Parity, Parity((byte)(iv - subv)));
            parent.SetFlag(Flag.Zero, ((byte)(iv - subv)) == 0);
            parent.SetFlag(Flag.Sign, (((byte)(iv - subv)) & 128) > 0);
        }

        private bool Parity(byte value)
        {
            int cnt = 0;
            for (byte bitmap = 1; bitmap != 0; bitmap <<= 1)
                cnt += (value & bitmap) > 0 ? 1 : 0;
            return (cnt % 2) == 0;
        }
    }
}
