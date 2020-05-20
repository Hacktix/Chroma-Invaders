using Chroma_Invaders.Opcodes;
using System;
using System.Collections.Generic;

namespace Chroma_Invaders
{
    public enum Register { A, F, B, C, D, E, H, L }
    public enum Flag { Carry = 1, Parity = 4, AuxiliaryCarry = 16, Zero = 64, Sign = 128 }

    public class Machine
    {
        public long StartTime = 0;
        public long EndTime = 0;

        public byte[] Memory = new byte[0x4000];

        public Dictionary<Register, byte> Registers = new Dictionary<Register, byte>()
        { { Register.A, 0 }, { Register.F, 2 }, { Register.B, 0 }, { Register.C, 0 }, { Register.D, 0 }, { Register.E, 0 }, { Register.H, 0 }, { Register.L, 0 }, };

        public bool InterruptsDisabled = false;
        public bool Halted = false;

        public ushort PC = 0;
        public ushort SP = 0;

        private int CycleCooldown = 0;

        public Machine() { }

        public Machine(byte[][] roms)
        {
            ushort loadPointer = 0;
            for (int i = 0; i < roms.Length; i++)
                for (int j = 0; j < roms[i].Length; j++, loadPointer++)
                    Memory[loadPointer] = roms[i][j];
        }

        public void ExecuteCycles(int cycleLimit)
        {
            StartTime = DateTime.Now.Ticks;
            int cycleCounter = cycleLimit;
            while(cycleCounter-- > 0)
            {
                if(CycleCooldown > 0)
                {
                    CycleCooldown--;
                    continue;
                }
                Opcode opcode = Decoder.DecodeOpcode(this, Memory[PC]);
                opcode.Execute();
                PC += (ushort)opcode.Length;

                CycleCooldown = opcode.Cycles - 1;
            }
            EndTime = DateTime.Now.Ticks;
        }

        public void WriteRegister16(OperationTarget16 regpair, ushort value)
        {
            switch(regpair)
            {
                case OperationTarget16.B:
                    Registers[Register.B] = (byte)((value & 0xFF00) >> 8);
                    Registers[Register.C] = (byte)(value & 0xFF);
                    break;
                case OperationTarget16.D:
                    Registers[Register.D] = (byte)((value & 0xFF00) >> 8);
                    Registers[Register.E] = (byte)(value & 0xFF);
                    break;
                case OperationTarget16.H:
                    Registers[Register.H] = (byte)((value & 0xFF00) >> 8);
                    Registers[Register.L] = (byte)(value & 0xFF);
                    break;
                case OperationTarget16.PSW:
                    Registers[Register.A] = (byte)((value & 0xFF00) >> 8);
                    Registers[Register.F] = (byte)(value & 0xFF);
                    break;
                case OperationTarget16.SP:
                    SP = value;
                    break;
                case OperationTarget16.PC:
                    PC = value;
                    break;
            }
        }

        public ushort ReadRegister16(OperationTarget16 regpair)
        {
            switch(regpair)
            {
                case OperationTarget16.B: return (ushort)((Registers[Register.B] << 8) + Registers[Register.C]);
                case OperationTarget16.D: return (ushort)((Registers[Register.D] << 8) + Registers[Register.E]);
                case OperationTarget16.H: return (ushort)((Registers[Register.H] << 8) + Registers[Register.L]);
                case OperationTarget16.PSW: return (ushort)((Registers[Register.A] << 8) + Registers[Register.F]);
                case OperationTarget16.SP: return SP;
                case OperationTarget16.PC: return PC;
            }
            return 0;
        }

        public void SetFlag(Flag flag, bool set)
        {
            if (set)
                Registers[Register.F] |= (byte)flag;
            else
                Registers[Register.F] &= (byte)~flag;
        }
    }
}
