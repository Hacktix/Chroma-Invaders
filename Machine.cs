using Chroma_Invaders.Opcodes;
using System.Collections.Generic;

namespace Chroma_Invaders
{
    public enum Register { A, F, B, C, D, E, H, L }

    public class Machine
    {
        public byte[] Memory = new byte[0x4000];

        public Dictionary<Register, byte> Registers = new Dictionary<Register, byte>()
        { { Register.A, 0 }, { Register.F, 2 }, { Register.B, 0 }, { Register.C, 0 }, { Register.D, 0 }, { Register.E, 0 }, { Register.H, 0 }, { Register.L, 0 }, };

        public bool InterruptsDisabled = false;
        public bool Halted = false;

        public ushort PC = 0;
        public ushort SP = 0;

        private int CycleCooldown = 0;

        public Machine(byte[][] roms)
        {
            ushort loadPointer = 0;
            for (int i = 0; i < roms.Length; i++)
                for (int j = 0; j < roms[i].Length; j++, loadPointer++)
                    Memory[loadPointer] = roms[i][j];
        }

        public void ExecuteCycles(int cycleLimit)
        {
            int cycleCounter = cycleLimit;
            while(cycleCounter-- > 0)
            {
                if(CycleCooldown > 0)
                {
                    CycleCooldown--;
                    continue;
                }
                Opcode opcode = Decoder.DecodeOpcode(this, Memory[PC]);
                PC += (ushort)opcode.Length;
                opcode.Execute();

                CycleCooldown = opcode.Cycles - 1;
            }
        }
    }
}
