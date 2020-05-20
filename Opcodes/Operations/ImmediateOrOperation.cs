namespace Chroma_Invaders.Opcodes
{
    public class ImmediateOrOperation : Opcode
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
            parent.SetFlag(Flag.Parity, parent.Registers[Register.A] % 2 == 0);
            parent.SetFlag(Flag.Zero, parent.Registers[Register.A] == 0);
            parent.SetFlag(Flag.Sign, (parent.Registers[Register.A] & 128) > 0);
        }
    }
}
