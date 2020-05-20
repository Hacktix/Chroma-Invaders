namespace Chroma_Invaders.Opcodes
{
    public class ExchangeRegistersOperation : Opcode // XCHG
    {
        public ExchangeRegistersOperation(Machine parent) : base(parent) {
            Cycles = 5;
        }

        public override void Execute()
        {
            ushort hl = parent.ReadRegister16(OperationTarget16.H);
            parent.WriteRegister16(OperationTarget16.H, parent.ReadRegister16(OperationTarget16.D));
            parent.WriteRegister16(OperationTarget16.D, hl);
        }
    }
}
