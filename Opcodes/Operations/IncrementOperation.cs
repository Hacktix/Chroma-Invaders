namespace Chroma_Invaders.Opcodes
{
    public class IncrementOperation : Opcode // INR
    {
        private OperationTarget8 target;

        public IncrementOperation(Machine parent, byte opcode) : base(parent) {
            switch((opcode & 0b111000) >> 3)
            {
                case 0b000: target = OperationTarget8.B; break;
                case 0b001: target = OperationTarget8.C; break;
                case 0b010: target = OperationTarget8.D; break;
                case 0b011: target = OperationTarget8.E; break;
                case 0b100: target = OperationTarget8.H; break;
                case 0b101: target = OperationTarget8.L; break;
                case 0b110: target = OperationTarget8.M; break;
                case 0b111: target = OperationTarget8.A; break;
            }
            Cycles = target == OperationTarget8.M ? 10 : 5;
        }

        public override void Execute()
        {
            byte iv = (target == OperationTarget8.M) ? parent.Memory[(parent.Registers[Register.H] << 8) | (parent.Registers[Register.L])] : parent.Registers[(Register)target];
            if (target == OperationTarget8.M)
                parent.Memory[(parent.Registers[Register.H] << 8) | (parent.Registers[Register.L])]++;
            else
                parent.Registers[(Register)target]++;

            // Set Flags
            parent.SetFlag(Flag.AuxiliaryCarry, ((iv & 0xF) + 1) > 0xF);
            parent.SetFlag(Flag.Parity, Parity((byte)(iv + 1)));
            parent.SetFlag(Flag.Zero, ((byte)(iv + 1)) == 0);
            parent.SetFlag(Flag.Sign, (((byte)(iv + 1)) & 128) > 0);
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
