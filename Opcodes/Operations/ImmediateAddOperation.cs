namespace Chroma_Invaders.Opcodes
{
    public class ImmediateAddOperation : Opcode
    {
        private OperationTarget8 source;
        private bool addCarry;

        public ImmediateAddOperation(Machine parent, byte opcode) : base(parent) {
            source = BitsToRegister((opcode & 0b111000) >> 3);
            addCarry = (opcode & 0b1000) > 0;
            Cycles = 7;
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
            byte addv = parent.Memory[parent.PC + 1];
            if (addCarry) addv += (byte)(parent.Registers[Register.F] & 1);
            parent.Registers[Register.A] += addv;

            // Set Flags
            parent.SetFlag(Flag.Carry, iv + addv > 0xFF);
            parent.SetFlag(Flag.AuxiliaryCarry, ((iv & 0xF) + (addv & 0xF)) > 0xF);
            parent.SetFlag(Flag.Parity, ((byte)(iv + addv)) % 2 == 0);
            parent.SetFlag(Flag.Zero, ((byte)(iv + addv)) == 0);
            parent.SetFlag(Flag.Sign, (((byte)(iv + addv)) & 128) > 0);
        }
    }
}
