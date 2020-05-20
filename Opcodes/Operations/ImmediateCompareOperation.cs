namespace Chroma_Invaders.Opcodes
{
    public class ImmediateCompareOperation : Opcode // CPI
    {
        public ImmediateCompareOperation(Machine parent) : base(parent) {
            Cycles = 7;
            Length = 2;
        }

        public override void Execute()
        {
            byte iv = parent.Registers[Register.A];
            byte subv = parent.Memory[parent.PC + 1];

            // Set Flags
            parent.SetFlag(Flag.Carry, iv - subv < 0);
            parent.SetFlag(Flag.AuxiliaryCarry, ((iv & 0xF) - (subv & 0xF)) < 0);
            parent.SetFlag(Flag.Parity, ((byte)(iv - subv)) % 2 == 0);
            parent.SetFlag(Flag.Zero, ((byte)(iv - subv)) == 0);
            parent.SetFlag(Flag.Sign, (((byte)(iv - subv)) & 128) > 0);
        }
    }
}
