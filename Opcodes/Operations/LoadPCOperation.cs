namespace Chroma_Invaders.Opcodes
{
    public class LoadPCOperation : Opcode // PCHL
    {
        public LoadPCOperation(Machine parent) : base(parent) {
            Cycles = 5;
        }

        public override void Execute()
        {
            parent.PC = parent.ReadRegister16(OperationTarget16.H);
        }
    }
}
