namespace Chroma_Invaders.Opcodes
{
    public class ImmediateSubOperation : Opcode // SUI, SBI
    {
        private bool subBorrow;

        public ImmediateSubOperation(Machine parent, byte opcode) : base(parent) {
            subBorrow = (opcode & 0b1000) > 0;
            Cycles = 7;
            Length = 2;
        }

        public override void Execute()
        {
            byte iv = parent.Registers[Register.A];
            byte subv = parent.Memory[parent.PC + 1];
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
