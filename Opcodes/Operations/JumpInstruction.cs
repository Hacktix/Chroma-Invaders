namespace Chroma_Invaders.Opcodes
{
    public class JumpOperation : Opcode // JMP, JC, JNC, JZ, JNZ, JM, JP, JPE, JPO
    {
        bool useCondition = false;
        Flag conditionFlag;
        bool conditionSet;

        public JumpOperation(Machine parent, byte opcode) : base(parent) {
            useCondition = opcode != 0b11000011;
            if (useCondition) conditionFlag = BitsToConditionFlag((byte)((opcode & 0b111000) >> 3));
            Cycles = 10;
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
            if(useCondition)
                if((((parent.Registers[Register.F] & (byte)conditionFlag) > 0) && !conditionSet) || (((parent.Registers[Register.F] & (byte)conditionFlag) == 0) && conditionSet))
                    return;
            parent.PC = (ushort)(parent.Memory[parent.PC + 1] + (parent.Memory[parent.PC + 2] << 8) - 3);
        }
    }
}
