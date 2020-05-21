namespace Chroma_Invaders.Opcodes
{
    public class HLMoveOperation : Opcode // SHLD, LHLD
    {
        private bool store;

        public HLMoveOperation(Machine parent, byte opcode) : base(parent) {
            store = (opcode & 0b1000) == 0;
            Length = 3;
        }

        public override void Execute()
        {
            ushort memaddr = (ushort)(parent.Memory[parent.PC + 1] + (parent.Memory[parent.PC + 2] << 8));
            if (store)
            {
                parent.Memory[memaddr] = parent.Registers[Register.L];
                parent.Memory[memaddr+1] = parent.Registers[Register.H];
            }
            else
            {
                parent.Registers[Register.L] = parent.Memory[memaddr];
                parent.Registers[Register.H] = parent.Memory[memaddr+1];
            }
                
        }
    }
}
