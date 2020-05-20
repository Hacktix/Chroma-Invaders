namespace Chroma_Invaders.Opcodes
{
    public class ExchangeStackOperation : Opcode // XTHL
    {
        public ExchangeStackOperation(Machine parent) : base(parent) {
            Cycles = 18;
        }

        public override void Execute()
        {
            ushort sv = (ushort)(parent.Memory[parent.SP] + (parent.Memory[parent.SP + 1] << 8));
            parent.Memory[parent.SP] = parent.Registers[Register.L];
            parent.Memory[parent.SP + 1] = parent.Registers[Register.H];
            parent.WriteRegister16(OperationTarget16.H, sv);
        }
    }
}
