namespace Chroma_Invaders.Opcodes
{
    public class DoubleIncDecOperation : Opcode // INX, DCX
    {
        private OperationTarget16 target;
        private bool inc;

        public DoubleIncDecOperation(Machine parent, byte opcode) : base(parent) {
            target = BitsToRegister((opcode & 0b110000) >> 4);
            inc = (opcode & 0b1000) == 0;
            Cycles = 5;
        }

        private OperationTarget16 BitsToRegister(int bits)
        {
            switch (bits)
            {
                case 0b00: return OperationTarget16.B;
                case 0b01: return OperationTarget16.D;
                case 0b10: return OperationTarget16.H;
                case 0b11: return OperationTarget16.SP;
                default: return OperationTarget16.SP;
            }
        }

        public override void Execute()
        {
            if (inc)
                parent.WriteRegister16(target, (ushort)(parent.ReadRegister16(target) + 1));
            else
                parent.WriteRegister16(target, (ushort)(parent.ReadRegister16(target) - 1));
        }
    }
}
