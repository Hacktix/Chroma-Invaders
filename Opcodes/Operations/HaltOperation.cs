namespace Chroma_Invaders.Opcodes
{
    public class HaltOperation : Opcode // HLT
    {
        public HaltOperation(Machine parent) : base(parent) {
            Cycles = 7;
        }

        public override void Execute() {
            parent.Halted = true;
        }
    }
}
