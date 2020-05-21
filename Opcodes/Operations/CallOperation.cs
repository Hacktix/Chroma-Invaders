namespace Chroma_Invaders.Opcodes
{
    public class CallOperation : Opcode // CALL, CC, CNC, CZ, CNZ, CM, CP, CPE, CPO
    {
        bool useCondition = false;
        Flag conditionFlag;
        bool conditionSet;

        public CallOperation(Machine parent, byte opcode) : base(parent) {
            useCondition = opcode != 0b11001101;
            if (useCondition) conditionFlag = BitsToConditionFlag((byte)((opcode & 0b111000) >> 3));
            Cycles = 17;
            Length = 3;
        }

        private Flag BitsToConditionFlag(byte bits)
        {
            switch(bits)
            {
                case 0b011: conditionSet = true; return Flag.Carry;
                case 0b010: conditionSet = false; return Flag.Carry;
                case 0b001: conditionSet = true; return Flag.Zero;
                case 0b000: conditionSet = false; return Flag.Zero;
                case 0b111: conditionSet = true; return Flag.Sign;
                case 0b110: conditionSet = false; return Flag.Sign;
                case 0b101: conditionSet = true; return Flag.Parity;
                case 0b100: conditionSet = false; return Flag.Parity;
                default: return Flag.Carry;
            }
        }

        public override void Execute()
        {
            if (useCondition)
            {
                if ((((parent.Registers[Register.F] & (byte)conditionFlag) > 0) && !conditionSet) || (((parent.Registers[Register.F] & (byte)conditionFlag) == 0) && conditionSet))
                {
                    Cycles = 11;
                    return;
                }
            }
            parent.SP -= 2;
            parent.Memory[parent.SP + 1] = (byte)((parent.PC & 0xFF00) >> 8);
            parent.Memory[parent.SP] = (byte)(parent.PC & 0xFF);
            parent.PC = (ushort)(parent.Memory[parent.PC + 1] + (parent.Memory[parent.PC + 2] << 8) - 3);
        }
    }
}
