using System;
using System.Collections.Generic;
using System.Text;

namespace Chroma_Invaders
{
    public class ShiftHardware
    {
        public byte ShiftAmount = 0;

        public ushort ShiftRegister = 0;

        public void ShiftValue(byte value)
        {
            ShiftRegister >>= 8;
            ShiftRegister += (ushort)(value << 8);
        }

        public byte ReadResult()
        {
            return (byte)(((ShiftRegister << ShiftAmount) & 0xFF00) >> 8);
        }
    }
}
