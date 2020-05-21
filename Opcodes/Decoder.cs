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
            else if ((code & 0b11111000) == 0b10111000) return new CompareOperation(parent, code);
            else if ((code & 0b11100111) == 0b00000111) return new RotateOperation(parent, code);
            else if ((code & 0b11001011) == 0b11000001) return new StackOperation(parent, code);
            else if ((code & 0b11001111) == 0b00001001) return new DoubleAddOperation(parent, code);
            else if ((code & 0b11000111) == 0b00000011) return new DoubleIncDecOperation(parent, code);
            else if ((code & 0b11111111) == 0b11101011) return new ExchangeRegistersOperation(parent);
            else if ((code & 0b11111111) == 0b11100011) return new ExchangeStackOperation(parent);
            else if ((code & 0b11111111) == 0b11111001) return new LoadSPOperation(parent);
            else if ((code & 0b11111111) == 0b11101001) return new LoadPCOperation(parent);
            else if ((code & 0b11000111) == 0b00000110) return new ImmediateMoveOperation(parent, code);
            else if ((code & 0b11110111) == 0b11000110) return new ImmediateAddOperation(parent, code);
            else if ((code & 0b11110111) == 0b11010110) return new ImmediateSubOperation(parent, code);
            else if ((code & 0b11111111) == 0b11100110) return new ImmediateAndOperation(parent);
            else if ((code & 0b11111111) == 0b11101110) return new ImmediateXorOperation(parent);
            else if ((code & 0b11111111) == 0b11110110) return new ImmediateOrOperation(parent);
            else if ((code & 0b11111111) == 0b11111110) return new ImmediateCompareOperation(parent);
            else if ((code & 0b11110111) == 0b00110010) return new ImmediateAccMoveOperation(parent, code);
            else if ((code & 0b11110111) == 0b00100010) return new HLMoveOperation(parent, code);
            else if ((code & 0b11000110) == 0b11000010) return new JumpOperation(parent, code);
            else if ((code & 0b11000110) == 0b11000100) return new CallOperation(parent, code);
            else                                        return new NoOperation(parent, code);
        }
    }
}
