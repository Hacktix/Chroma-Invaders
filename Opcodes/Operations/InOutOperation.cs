namespace Chroma_Invaders.Opcodes
{
    public class InOutOperation : Opcode // IN, OUT
    {
        private bool read;

        public InOutOperation(Machine parent, byte opcode) : base(parent) {
            read = (opcode & 0b1000) > 0;
            Cycles = 10;
            Length = 2;
        }

        public override void Execute() {
            if (read)
                parent.Registers[Register.A] = parent.ReadFromInput(parent.Memory[parent.PC + 1]);
            else
                parent.WriteToOutput(parent.Memory[parent.PC + 1], parent.Registers[Register.A]);
        }
    }
}
