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

        public Memory Memory = new Memory();

        public Dictionary<Register, byte> Registers = new Dictionary<Register, byte>()
        { { Register.A, 0 }, { Register.F, 2 }, { Register.B, 0 }, { Register.C, 0 }, { Register.D, 0 }, { Register.E, 0 }, { Register.H, 0 }, { Register.L, 0 }, };

        public bool InterruptsDisabled = false;
        public bool Halted = false;

        public ushort PC = 0;
        public ushort SP = 0;

        public bool NextOp = true;
        public bool HitBreakpoint = false;
        public ushort BreakpointAddr = 0xFFFF;

        private int CycleCooldown = 0;
        private long LastVBLANK = 0;
        private ShiftHardware Shift = new ShiftHardware();

        public Machine() { }

        public Machine(byte[][] roms)
        {
            ushort loadPointer = 0;
            for (int i = 0; i < roms.Length; i++)
                for (int j = 0; j < roms[i].Length; j++, loadPointer++)
                    Memory[loadPointer] = roms[i][j];
            LastVBLANK = DateTime.Now.Ticks;
        }

        public void ExecuteCycles(int cycleLimit)
        {
            StartTime = DateTime.Now.Ticks;
            int cycleCounter = cycleLimit;

            if (!NextOp) return;

            while(cycleCounter-- > 0)
            {
                if(CycleCooldown > 0)
                {
                    CycleCooldown--;
                    continue;
                }

                /*
                if(!InterruptsDisabled && (DateTime.Now.Ticks - LastVBLANK >= (1.0/60.0) * TimeSpan.TicksPerSecond))
                {
                    LastVBLANK = DateTime.Now.Ticks;
                    GenerateInterrupt(2);
                    continue;
                }
                */

                if (PC == BreakpointAddr) HitBreakpoint = true;

                if (HitBreakpoint)
                {
                    Console.WriteLine("====================================================");
                    Console.WriteLine("# EXECUTING 0x" + Memory[PC].ToString("X2"));
                }

                Opcode opcode = Decoder.DecodeOpcode(this, Memory[PC]);

                if (HitBreakpoint)
                {
                    if (opcode.Length > 1)
                    {
                        for (int i = 1; i < opcode.Length; i++)
                            Console.WriteLine("# OPERAND " + i + ": 0x" + Memory[PC + i].ToString("X2"));
                    }
                }

                opcode.Execute();
                PC += (ushort)opcode.Length;
                CycleCooldown = opcode.Cycles - 1;

                if (HitBreakpoint)
                {
                    NextOp = false;
                    DebugLog();
                    break;
                }
            }
            EndTime = DateTime.Now.Ticks;
        }

        public void DebugLog()
        {
            Console.WriteLine("==================== DEBUG LOG =====================");
            Console.WriteLine("A  : " + Registers[Register.A].ToString("X2"));
            Console.WriteLine("F  : " + Convert.ToString(Registers[Register.A], 2));
            Console.WriteLine("B  : " + Registers[Register.B].ToString("X2"));
            Console.WriteLine("C  : " + Registers[Register.C].ToString("X2"));
            Console.WriteLine("D  : " + Registers[Register.D].ToString("X2"));
            Console.WriteLine("E  : " + Registers[Register.E].ToString("X2"));
            Console.WriteLine("H  : " + Registers[Register.H].ToString("X2"));
            Console.WriteLine("L  : " + Registers[Register.L].ToString("X2"));
            Console.WriteLine("SP : " + SP.ToString("X4"));
            Console.WriteLine("PC : " + PC.ToString("X4"));
        }

        public void GenerateInterrupt(int number)
        {
            SP -= 2;
            Memory[SP + 1] = (byte)((PC & 0xFF00) >> 8);
            Memory[SP] = (byte)(PC & 0xFF);
            PC = (ushort)(8 * number);
        }

        public byte ReadFromInput(byte inputNo)
        {
            // TODO: Emulate input devices
            switch(inputNo)
            {
                case 3: return Shift.ReadResult();
            }
            return 0;
        }

        public void WriteToOutput(byte outputNo, byte outval)
        {
            // TODO: Emulate output devices
            switch(outputNo)
            {
                case 2:
                    Shift.ShiftAmount = (byte)(outval & 0b111);
                    break;
                case 4:
                    Shift.ShiftValue(outval);
                    break;
            }
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
