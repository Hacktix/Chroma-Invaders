namespace Chroma_Invaders.Opcodes
{
    public class DoubleAddOperation : Opcode // DAD
    {
        private OperationTarget16 source;

        public DoubleAddOperation(Machine parent, byte opcode) : base(parent) {
            source = BitsToRegister((opcode & 0b110000) >> 4);
            Cycles = 10;
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
            ushort iv = parent.ReadRegister16(OperationTarget16.H);
            ushort addv = parent.ReadRegister16(source);
            parent.WriteRegister16(OperationTarget16.H, (ushort)(iv + addv));

            // Set Flags
            parent.SetFlag(Flag.Carry, iv + addv > 0xFFFF);
        }
    }
}
