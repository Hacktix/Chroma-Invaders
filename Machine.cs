using Chroma_Invaders.Opcodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Chroma_Invaders
{
    public enum Register { A, F, B, C, D, E, H, L }
    public enum Flag { Carry = 1, Parity = 4, AuxiliaryCarry = 16, Zero = 64, Sign = 128 }

    public class Machine
    {
        public long EndTime = 0;
        private Stopwatch PerformanceTimer;
        private Stopwatch CycleTimer;

        public Memory Memory = new Memory();

        public Dictionary<Register, byte> Registers = new Dictionary<Register, byte>()
        { { Register.A, 0 }, { Register.F, 2 }, { Register.B, 0 }, { Register.C, 0 }, { Register.D, 0 }, { Register.E, 0 }, { Register.H, 0 }, { Register.L, 0 }, };

        private Dictionary<ushort, Opcode> InstructionCache = new Dictionary<ushort, Opcode>();

        public bool InterruptsDisabled = true;
        public bool Halted = false;

        public ushort PC = 0x0;
        public ushort SP = 0;

        public bool VBlank = false;

        public byte InputPort1 = 0b00001001;

        private int CycleCooldown = 0;
        private long Cycles = 0;
        private byte LastInterrupt = 0xCF;

        private ShiftHardware Shift = new ShiftHardware();

        public Machine() { }

        public Machine(byte[][] roms)
        {
            ushort loadPointer = 0x0;
            for (int i = 0; i < roms.Length; i++)
                for (int j = 0; j < roms[i].Length; j++, loadPointer++)
                    Memory[loadPointer] = roms[i][j];
            Memory.FinishLoading();

            PerformanceTimer = new Stopwatch();
            CycleTimer = new Stopwatch();
        }

        public void ExecuteCycles(int cycleLimit)
        {
            PerformanceTimer.Restart();
            int cycleCounter = cycleLimit;

            while(cycleCounter-- > 0)
            {
                CycleTimer.Start();

                if (Cycles++ >= 8333)
                {
                    Cycles -= 8333;
                    if(!InterruptsDisabled) VBlank = true;
                }

                if (CycleCooldown > 0)
                {
                    CycleCooldown--;
                    WaitForCycleFinish(CycleTimer);
                    continue;
                }

                if(Halted)
                {
                    CycleCooldown += 4;
                    WaitForCycleFinish(CycleTimer);
                    continue;
                }

                if(!InterruptsDisabled && VBlank)
                {
                    VBlank = false;
                    GenerateInterrupt(2);
                    WaitForCycleFinish(CycleTimer);
                    continue;
                }

                Opcode opcode;
                if (!InstructionCache.ContainsKey(PC))
                {
                    opcode = Decoder.DecodeOpcode(this, Memory[PC]);
                    InstructionCache.Add(PC, opcode);
                }
                else opcode = InstructionCache[PC];

                opcode.Execute();
                PC += (ushort)opcode.Length;
                CycleCooldown = opcode.Cycles - 1;

                WaitForCycleFinish(CycleTimer);
            }
            EndTime = PerformanceTimer.ElapsedTicks;
        }

        private void WaitForCycleFinish(Stopwatch timer)
        {
            while(timer.ElapsedTicks < (1.0/2000000.0) * TimeSpan.TicksPerSecond) { /* Wait... */ }
            timer.Reset();
        }

        public void DebugLog()
        {
            Console.WriteLine("==================== DEBUG LOG =====================");
            Console.WriteLine("A  : " + Registers[Register.A].ToString("X2"));
            Console.WriteLine("F  : " + Convert.ToString(Registers[Register.F], 2));
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
            Halted = false;
            InterruptsDisabled = true;
            LastInterrupt = (byte)(LastInterrupt == 0xCF ? 0xD7 : 0xCF);
            Opcode rst = new RestartOperation(this, LastInterrupt);
            rst.Execute();
            CycleCooldown = rst.Cycles - 1;
        }

        public byte ReadFromInput(byte inputNo)
        {
            // TODO: Emulate input devices
            switch(inputNo)
            {
                case 1: return InputPort1;
                case 2: return 0;
                case 3: return Shift.ReadResult();
            }
            return Registers[Register.A];
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
