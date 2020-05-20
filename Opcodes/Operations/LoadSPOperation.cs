namespace Chroma_Invaders.Opcodes
{
    public class LoadSPOperation : Opcode // SPHL
    {
        public LoadSPOperation(Machine parent) : base(parent) {
            Cycles = 5;
        }

        public override void Execute()
        {
            parent.SP = parent.ReadRegister16(OperationTarget16.H);
        }
    }
}
