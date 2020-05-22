using Chroma_Invaders.Opcodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

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

        private Dictionary<ushort, Opcode> InstructionCache = new Dictionary<ushort, Opcode>();

        public bool InterruptsDisabled = true;
        public bool Halted = false;

        public ushort PC = 0x0;
        public ushort SP = 0;

        public bool VBlank = false;

        public bool NextOp = true;
        public bool HitBreakpoint = false;
        public ushort BreakpointAddr = 0xFFFF;

        public byte InputPort1 = 0b00001001;

        private byte LastInterrupt = 0xCF;

        private int CycleCooldown = 0;
        private ShiftHardware Shift = new ShiftHardware();

        public long CycleCount = 0;

        private double Timer = 0.0;

        private FileStream LogFile;
        private StreamWriter LogWriter;

        public Machine() { }

        public Machine(byte[][] roms)
        {
            //if (File.Exists("log.txt")) File.Delete("log.txt");
            //LogFile = File.OpenWrite("log.txt");
            //LogWriter = new StreamWriter(LogFile);
            //LogWriter.AutoFlush = true;
            ushort loadPointer = 0x0;
            for (int i = 0; i < roms.Length; i++)
                for (int j = 0; j < roms[i].Length; j++, loadPointer++)
                    Memory[loadPointer] = roms[i][j];
            /*Memory[0x00] = 0xD3;
            Memory[0x01] = 0x00;
            Memory[0x05] = 0xDB;
            Memory[0x06] = 0x00;
            Memory[0x07] = 0xC9;*/
        }

        public void ExecuteCycles(int cycleLimit)
        {
            StartTime = DateTime.Now.Ticks;
            int cycleCounter = cycleLimit;

            if (!NextOp) return;

            while(cycleCounter-- > 0)
            {
                Timer += 1.0 / 200000.0;
                if (Timer > (1.0 / 60.0))
                {
                    Timer -= (1.0 / 60.0);
                    if(!InterruptsDisabled) VBlank = true;
                }

                if (CycleCooldown > 0)
                {
                    CycleCooldown--;
                    CycleCount++;
                    continue;
                }

                // DebugLog();

                if(!InterruptsDisabled && VBlank)
                {
                    VBlank = false;
                    GenerateInterrupt(2);
                    continue;
                }


                if (PC == BreakpointAddr) HitBreakpoint = true;

                if (HitBreakpoint)
                {
                    Console.WriteLine("====================================================");
                    Console.WriteLine("# EXECUTING 0x" + Memory[PC].ToString("X2") + " FROM 0x" + PC.ToString("X4"));
                }

                Opcode opcode;
                if (!InstructionCache.ContainsKey(PC))
                {
                    opcode = Decoder.DecodeOpcode(this, Memory[PC]);
                    InstructionCache.Add(PC, opcode);
                }
                else opcode = InstructionCache[PC];
                    

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
                CycleCount++;

                if (HitBreakpoint)
                {
                    DebugLog();
                    NextOp = false;
                    break;
                }
            }
            EndTime = DateTime.Now.Ticks;
        }

        public void DebugLog()
        {
            /*LogWriter.Write("PC: " + PC.ToString("X4") +
                ", AF: " + ReadRegister16(OperationTarget16.PSW).ToString("X4") +
                ", BC: " + ReadRegister16(OperationTarget16.B).ToString("X4") +
                ", DE: " + ReadRegister16(OperationTarget16.D).ToString("X4") +
                ", HL: " + ReadRegister16(OperationTarget16.H).ToString("X4") +
                ", SP: " + SP.ToString("X4") +
                ", CYC: " + CycleCount + "\t(" +
                Memory[PC].ToString("X2") + " " +
                Memory[PC + 1].ToString("X2") + " " +
                Memory[PC + 2].ToString("X2") + " " +
                Memory[PC + 3].ToString("X2") + ")\n");
            */
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
            if(HitBreakpoint)
            {
                Console.WriteLine("====================================================");
                Console.WriteLine(" ### INTERRUPT " + number + " ###");
                Console.WriteLine("====================================================");
            }
            InterruptsDisabled = true;
            LastInterrupt = (byte)(LastInterrupt == 0xCF ? 0xD7 : 0xCF);
            new RestartOperation(this, LastInterrupt).Execute();
        }

        public byte ReadFromInput(byte inputNo)
        {
            // if (Registers[Register.C] == 2) Console.Write(Convert.ToChar(Registers[Register.E]));

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
            // Environment.Exit(0);

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
