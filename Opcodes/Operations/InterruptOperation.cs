namespace Chroma_Invaders.Opcodes
{
    public class InterruptOperation : Opcode // EI, DI
    {
        private bool enable;

        public InterruptOperation(Machine parent, byte opcode) : base(parent) {
            enable = (opcode & 0b1000) > 0;
        }

        public override void Execute() {
            parent.InterruptsDisabled = !enable;
        }
    }
}
