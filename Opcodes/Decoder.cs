namespace Chroma_Invaders.Opcodes
{
    public class Decoder
    {
        public static Opcode DecodeOpcode(Machine parent, byte code)
        {
            if ((code & 0b11000111) == 0b00000100)      return new IncrementOperation(parent, code);
            else if ((code & 0b11000111) == 0b00000101) return new DecrementOperation(parent, code);
            else if (code == 0x37)                      return new SetCarryOperation(parent);
            else if (code == 0x3F)                      return new ComplementCarryOperation(parent);
            else if (code == 0x27)                      return new DecimalAdjustOperation(parent);
            // TODO: Add HALT Operation
            else if ((code & 0b11000000) == 0b01000000) return new MoveOperation(parent, code);
            else if ((code & 0b11100111) == 0b00000010) return new AccMoveOperation(parent, code);
            else if ((code & 0b11110000) == 0b10000000) return new AddOperation(parent, code);
            else if ((code & 0b11110000) == 0b10010000) return new SubOperation(parent, code);
            else if ((code & 0b11111000) == 0b10100000) return new AndOperation(parent, code);
            else if ((code & 0b11111000) == 0b10101000) return new XorOperation(parent, code);
            else if ((code & 0b11111000) == 0b10110000) return new OrOperation(parent, code);
            else                                        return new NoOperation(parent, code);
        }
    }
}
