namespace Chroma_Invaders.Opcodes
{
    public class DecimalAdjustOperation : Opcode
    {
        public DecimalAdjustOperation(Machine parent) : base(parent) { }

        public override void Execute()
        {
            if (((parent.Registers[Register.A]) & 0xF) > 9 || (parent.Registers[Register.F] & (byte)Flag.AuxiliaryCarry) != 0)
                parent.Registers[Register.A] += 6;
            if ((((parent.Registers[Register.A]) & 0xF0) >> 4) > 9 || (parent.Registers[Register.F] & (byte)Flag.Carry) != 0)
                parent.Registers[Register.A] += 6 << 4;
        }
    }
}
