﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Chroma_Invaders
{
    public enum Register { A, F, B, C, D, E, H, L }

    public class Machine
    {
        public byte[] Memory = new byte[0x4000];

        private Dictionary<Register, byte> Registers = new Dictionary<Register, byte>()
        { { Register.A, 0 }, { Register.F, 2 }, { Register.B, 0 }, { Register.C, 0 }, { Register.D, 0 }, { Register.E, 0 }, { Register.H, 0 }, { Register.L, 0 }, };

        private bool InterruptsDisabled = false;
        private bool Halted = false;

        private ushort PC = 0;
        private ushort SP = 0;

        public Machine(byte[][] roms)
        {
            ushort loadPointer = 0;
            for (int i = 0; i < roms.Length; i++)
                for (int j = 0; j < roms[i].Length; j++, loadPointer++)
                    Memory[loadPointer] = roms[i][j];
        }
    }
}
