namespace Chroma_Invaders.Opcodes
{
    public class RotateOperation : Opcode // RLC, RAL, RRC, RAR
    {
        private bool left;
        private bool useCarry;

        public RotateOperation(Machine parent, byte opcode) : base(parent) {
            left = (opcode & 0b1000) == 0;
            useCarry = (opcode & 0b10000) > 0;
            Cycles = 4;
        }

        public override void Execute()
        {
            if(left)
            {
                if(useCarry)
                {
                    byte carryAdd = (byte)(parent.Registers[Register.F] & 1);
                    parent.SetFlag(Flag.Carry, (parent.Registers[Register.A] & 128) > 0);
                    parent.Registers[Register.A] = (byte)((parent.Registers[Register.A] << 1) + carryAdd);
                } else {
                    byte rotateAdd = (byte)((parent.Registers[Register.A] & 128) >> 7);
                    parent.SetFlag(Flag.Carry, (parent.Registers[Register.A] & 128) > 0);
                    parent.Registers[Register.A] = (byte)((parent.Registers[Register.A] << 1) + rotateAdd);
                }
            } else {
                if (useCarry)
                {
                    byte carryAdd = (byte)((parent.Registers[Register.F] & 1) << 7);
                    parent.SetFlag(Flag.Carry, (parent.Registers[Register.A] & 1) > 0);
                    parent.Registers[Register.A] = (byte)((parent.Registers[Register.A] >> 1) + carryAdd);
                }
                else
                {
                    byte rotateAdd = (byte)(parent.Registers[Register.A] & 1);
                    parent.SetFlag(Flag.Carry, (parent.Registers[Register.A] & 1) > 0);
                    parent.Registers[Register.A] = (byte)((parent.Registers[Register.A] >> 1) + rotateAdd);
                }
            }
        }
    }
}
