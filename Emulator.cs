using Chroma;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chroma_Invaders
{
    public class Emulator : Game
    {
        public static readonly int SCREEN_WIDTH = 224;
        public static readonly int SCREEN_HEIGHT = 256;
        public static readonly int SCALE_FACTOR = 3;

        private Machine machine;

        public Emulator(byte[][] roms)
        {
            machine = new Machine(roms);

            Window.GoWindowed((ushort)(SCREEN_WIDTH * SCALE_FACTOR), (ushort)(SCREEN_HEIGHT * SCALE_FACTOR));
        }
    }
}
