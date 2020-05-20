namespace Chroma_Invaders.Opcodes
{
    public class ImmediateAddOperation : Opcode // ADI, ACI
    {
        private bool addCarry;

        public ImmediateAddOperation(Machine parent, byte opcode) : base(parent) {
            addCarry = (opcode & 0b1000) > 0;
            Cycles = 7;
            Length = 2;
        }

        public override void Execute()
        {
            byte iv = parent.Registers[Register.A];
            byte addv = parent.Memory[parent.PC + 1];
            if (addCarry) addv += (byte)(parent.Registers[Register.F] & 1);
            parent.Registers[Register.A] += addv;

            // Set Flags
            parent.SetFlag(Flag.Carry, iv + addv > 0xFF);
            parent.SetFlag(Flag.AuxiliaryCarry, ((iv & 0xF) + (addv & 0xF)) > 0xF);
            parent.SetFlag(Flag.Parity, ((byte)(iv + addv)) % 2 == 0);
            parent.SetFlag(Flag.Zero, ((byte)(iv + addv)) == 0);
            parent.SetFlag(Flag.Sign, (((byte)(iv + addv)) & 128) > 0);
        }
    }
}
