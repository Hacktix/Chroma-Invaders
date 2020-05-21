namespace Chroma_Invaders.Opcodes
{
    public class RestartOperation : Opcode // RST
    {
        ushort memaddr;

        public RestartOperation(Machine parent, byte opcode) : base(parent) {
            memaddr = (ushort)(opcode & 0b111000);
            Cycles = 11;
        }

        public override void Execute()
        {
            parent.SP -= 2;
            parent.Memory[parent.SP + 1] = (byte)((parent.PC & 0xFF00) >> 8);
            parent.Memory[parent.SP] = (byte)(parent.PC & 0xFF);
            parent.PC = (ushort)(memaddr - 1);
        }
    }
}
