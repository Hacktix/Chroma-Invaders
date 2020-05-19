namespace Chroma_Invaders.Opcodes
{
    public class RotateOperation : Opcode
    {
        private bool left;
        private bool useCarry;

        public RotateOperation(Machine parent, byte opcode) : base(parent) {
            left = (opcode & 0b1000) == 0;
            useCarry = (opcode & 0b10000) > 0;
            Cycles = 4;
        }

        public override void Execute()
        {
            byte iv = parent.Registers[Register.A];
            if (left) parent.Registers[Register.A] <<= 1;
            else parent.Registers[Register.A] >>= 1;
            if (useCarry) parent.Registers[Register.A] += (byte)((parent.Registers[Register.F] & 1) << (left ? 0 : 7));
            parent.SetFlag(Flag.Carry, left ? (iv & 128) > 0 : (iv & 1) > 0);
        }
    }
}
