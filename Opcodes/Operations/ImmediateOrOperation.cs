namespace Chroma_Invaders.Opcodes
{
    public class ImmediateOrOperation : Opcode // ORI
    {
        public ImmediateOrOperation(Machine parent) : base(parent) {
            Cycles = 7;
            Length = 2;
        }

        public override void Execute()
        {
            parent.Registers[Register.A] |= parent.Memory[parent.PC + 1];

            // Set Flags
            parent.SetFlag(Flag.Carry, false);
            parent.SetFlag(Flag.Parity, Parity(parent.Registers[Register.A]));
            parent.SetFlag(Flag.Zero, parent.Registers[Register.A] == 0);
            parent.SetFlag(Flag.Sign, (parent.Registers[Register.A] & 128) > 0);
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
