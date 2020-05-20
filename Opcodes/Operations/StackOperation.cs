namespace Chroma_Invaders.Opcodes
{
    public class StackOperation : Opcode // PUSH, POP
    {
        private OperationTarget16 target;
        private bool push;

        public StackOperation(Machine parent, byte opcode) : base(parent) {
            target = BitsToRegisterPair((byte)((opcode & 0b110000) >> 4));
            push = (opcode & 0b100) > 0;
            Cycles = push ? 11 : 10;
        }

        private OperationTarget16 BitsToRegisterPair(byte bits)
        {
            switch(bits)
            {
                case 0b00: return OperationTarget16.B;
                case 0b01: return OperationTarget16.D;
                case 0b10: return OperationTarget16.H;
                case 0b11: return OperationTarget16.PSW;
                default: return OperationTarget16.PSW;
            }
        }

        public override void Execute()
        {
            if(push) {
                ushort val = parent.ReadRegister16(target);
                parent.SP -= 2;
                parent.Memory[parent.SP + 1] = (byte)((val & 0xFF00) >> 8);
                parent.Memory[parent.SP] = (byte)(val & 0xFF);
            } else {
                ushort val = (ushort)(parent.Memory[parent.SP] + (parent.Memory[parent.SP + 1] << 8));
                parent.WriteRegister16(target, val);
                parent.SP += 2;
            }
        }
    }
}
