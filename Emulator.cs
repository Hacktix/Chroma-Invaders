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

        public static readonly int CYCLES_PER_UPDATE = 4000;
        public static readonly int UPDATE_FREQUENCY = 1000 / (2000000 / CYCLES_PER_UPDATE);

        private Machine Machine;

        public Emulator(byte[][] roms)
        {
            Machine = new Machine(roms);

            Window.GoWindowed((ushort)(SCREEN_WIDTH * SCALE_FACTOR), (ushort)(SCREEN_HEIGHT * SCALE_FACTOR));
        }

        protected override void FixedUpdate(float fixedDelta)
        {
            Window.Properties.Title = "Chroma Invaders (" + Window.FPS + " FPS)";
            Machine.ExecuteCycles(CYCLES_PER_UPDATE);
        }
    }
}
