using Chroma;
using Chroma.Graphics;
using Chroma.Input.EventArgs;
using System;
using System.Collections.Generic;
using System.Numerics;
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
            Machine.ExecuteCycles(CYCLES_PER_UPDATE);

            // Performance Calculation
            double percent = (int)((CYCLES_PER_UPDATE * 0.5 / (Machine.EndTime - Machine.StartTime)) * 10000) / 100.0;
            Window.Properties.Title = "Chroma Invaders (" + Window.FPS + " FPS) [" + percent + "%]";
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            Machine.NextOp = true;
        }

        protected override void Draw(RenderContext context)
        {
            for(int col = 0; col < SCREEN_WIDTH; col++)
            {
                for(int row = SCREEN_HEIGHT / 8; row >= 0; row--)
                {
                    for(byte bitmap = 1, bit = 0; bitmap > 0; bitmap <<= 1, bit++)
                    {
                        int x = col * SCALE_FACTOR;
                        int y = (SCREEN_HEIGHT - (8 * row + bit)) * SCALE_FACTOR - SCALE_FACTOR;
                        if ((Machine.Memory[0x2400 + col * 0x20 + row] & bitmap) != 0)
                            context.Rectangle(ShapeMode.Fill, new Vector2(x, y), SCALE_FACTOR, SCALE_FACTOR, Color.White);
                    }
                }
            }
        }
    }
}
