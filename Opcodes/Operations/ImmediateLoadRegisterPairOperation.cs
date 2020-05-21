namespace Chroma_Invaders.Opcodes
{
    public class ImmediateLoadRegisterPairOperation : Opcode // LXI
    {
        OperationTarget16 target;

        public ImmediateLoadRegisterPairOperation(Machine parent, byte opcode) : base(parent) {
            target = BitsToRegisterPair((byte)((opcode & 0b110000) >> 4));
            Cycles = 10;
            Length = 3;
        }

        private OperationTarget16 BitsToRegisterPair(byte bits)
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
            parent.WriteRegister16(target, (ushort)(parent.Memory[parent.PC + 1] + (parent.Memory[parent.PC + 2] << 8)));
        }
    }
}
