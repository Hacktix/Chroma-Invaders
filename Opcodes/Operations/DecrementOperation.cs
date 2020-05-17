using System;
using System.Collections.Generic;
using System.Text;

namespace Chroma_Invaders.Opcodes
{
    public class DecrementOperation : Opcode
    {
        private OperationTarget8 target;

        public DecrementOperation(Machine parent, byte opcode) : base(parent) {
            switch((opcode & 0b111000) >> 3)
            {
                case 0b000: target = OperationTarget8.B; break;
                case 0b001: target = OperationTarget8.C; break;
                case 0b010: target = OperationTarget8.D; break;
                case 0b011: target = OperationTarget8.E; break;
                case 0b100: target = OperationTarget8.H; break;
                case 0b101: target = OperationTarget8.L; break;
                case 0b110: target = OperationTarget8.M; break;
                case 0b111: target = OperationTarget8.A; break;
            }
            Cycles = target == OperationTarget8.M ? 10 : 5;
        }

        public override void Execute()
        {
            if (target == OperationTarget8.M)
                parent.Memory[(parent.Registers[Register.H] << 8) | (parent.Registers[Register.L])]--;
            else
                parent.Registers[(Register)target]--;
        }
    }
}
