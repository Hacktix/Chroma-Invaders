using Chroma;
using Chroma.Audio;
using Chroma.Graphics;
using Chroma.Graphics.Accelerated;
using Chroma.Input.EventArgs;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Chroma_Invaders
{
    public class Emulator : Game
    {
        public static readonly int SCREEN_WIDTH = 224;
        public static readonly int SCREEN_HEIGHT = 256;
        public static readonly int SCALE_FACTOR = 3;

        public static readonly int CYCLES_PER_UPDATE = 60000;
        public static readonly int UPDATE_FREQUENCY = 1000 / (2000000 / CYCLES_PER_UPDATE);

        private static readonly int PERFORMANCE_BUFFER_LENGTH = 100;

        private Machine Machine;
        public static List<Sound> Sounds = new List<Sound>();

        private bool UseColor = true;
        private List<double> PerformanceBuffer = new List<double>();

        private bool UseShader = false;
        private PixelShader ArcadeShader;
        private RenderTarget Frame;

        public Emulator(byte[][] roms)
        {
            Machine = new Machine(roms);
            Window.GoWindowed((ushort)(SCREEN_WIDTH * SCALE_FACTOR), (ushort)(SCREEN_HEIGHT * SCALE_FACTOR));
            ArcadeShader = new PixelShader("shader.frag");
            Frame = new RenderTarget((ushort)(SCREEN_WIDTH * SCALE_FACTOR), (ushort)(SCREEN_HEIGHT * SCALE_FACTOR));
        }

        protected override void LoadContent()
        {

            Sounds.Add(Content.Load<Sound>("explosion.wav"));
            Sounds.Add(Content.Load<Sound>("fastinvader1.wav"));
            Sounds.Add(Content.Load<Sound>("fastinvader2.wav"));
            Sounds.Add(Content.Load<Sound>("fastinvader3.wav"));
            Sounds.Add(Content.Load<Sound>("fastinvader4.wav"));
            Sounds.Add(Content.Load<Sound>("invaderkilled.wav"));
            Sounds.Add(Content.Load<Sound>("shoot.wav"));
            Sounds.Add(Content.Load<Sound>("ufo_highpitch.wav"));
            Sounds[7].LoopCount = int.MaxValue;
        }

        protected override void FixedUpdate(float fixedDelta)
        {
            Machine.ExecuteCycles(CYCLES_PER_UPDATE);

            // Performance Calculation
            double percent = (CYCLES_PER_UPDATE * (1.0 / 2000000.0) * TimeSpan.TicksPerSecond) / Machine.EndTime;
            PerformanceBuffer.Add(percent);
            if (PerformanceBuffer.Count > PERFORMANCE_BUFFER_LENGTH) PerformanceBuffer.RemoveAt(0);

            UpdateWindowTitle();
        }

        private void UpdateWindowTitle()
        {
            double percent = 0;
            foreach (double value in PerformanceBuffer) percent += value;
            percent /= PerformanceBuffer.Count;
            percent = ((int)(percent * 10000)) / 100.0;
            Window.Properties.Title = "Chroma Invaders (" + Window.FPS + " FPS) [" + percent + "%]";
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Chroma.Input.KeyCode.Return:
                    Machine.InputPort1 |= 0b100;
                    break;
                case Chroma.Input.KeyCode.RightShift:
                    Machine.InputPort1 &= 0b11111110;
                    break;
                case Chroma.Input.KeyCode.Space:
                    Machine.InputPort1 |= 0b10000;
                    break;
                case Chroma.Input.KeyCode.Left:
                    Machine.InputPort1 |= 0b100000;
                    break;
                case Chroma.Input.KeyCode.Right:
                    Machine.InputPort1 |= 0b1000000;
                    break;
                case Chroma.Input.KeyCode.LeftControl:
                    Machine.InputPort1 |= 0b10;
                    break;
                case Chroma.Input.KeyCode.A:
                    Machine.InputPort2 |= 0b100000;
                    break;
                case Chroma.Input.KeyCode.D:
                    Machine.InputPort2 |= 0b1000000;
                    break;
                case Chroma.Input.KeyCode.W:
                    Machine.InputPort2 |= 0b10000;
                    break;
                case Chroma.Input.KeyCode.C:
                    UseColor = !UseColor;
                    break;
                case Chroma.Input.KeyCode.F1:
                    UseShader = !UseShader;
                    break;
            }
        }

        protected override void KeyReleased(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Chroma.Input.KeyCode.Return:
                    Machine.InputPort1 &= 0b11111011;
                    break;
                case Chroma.Input.KeyCode.RightShift:
                    Machine.InputPort1 |= 0b1;
                    break;
                case Chroma.Input.KeyCode.Space:
                    Machine.InputPort1 &= 0b11101111;
                    break;
                case Chroma.Input.KeyCode.Left:
                    Machine.InputPort1 &= 0b11011111;
                    break;
                case Chroma.Input.KeyCode.Right:
                    Machine.InputPort1 &= 0b10111111;
                    break;
                case Chroma.Input.KeyCode.LeftControl:
                    Machine.InputPort1 &= 0b11111101;
                    break;
                case Chroma.Input.KeyCode.A:
                    Machine.InputPort2 &= 0b11011111;
                    break;
                case Chroma.Input.KeyCode.D:
                    Machine.InputPort2 &= 0b10111111;
                    break;
                case Chroma.Input.KeyCode.W:
                    Machine.InputPort2 &= 0b11101111;
                    break;
            }
        }

        protected override void Draw(RenderContext context)
        {
            context.RenderTo(Frame, () =>
            {
                context.Clear(Color.Black);
                for (int col = 0; col < SCREEN_WIDTH; col++)
                {
                    for (int row = SCREEN_HEIGHT / 8; row >= 0; row--)
                    {
                        for (byte bitmap = 1, bit = 0; bitmap > 0; bitmap <<= 1, bit++)
                        {
                            int x = col * SCALE_FACTOR;
                            int y = (SCREEN_HEIGHT - (8 * row + bit)) * SCALE_FACTOR - SCALE_FACTOR;
                            if ((Machine.Memory[0x2400 + col * 0x20 + row] & bitmap) != 0)
                            {
                                Color pxColor = Color.White;
                                if (UseColor)
                                {
                                    if (y / SCALE_FACTOR > 32 && y / SCALE_FACTOR < 49) pxColor = Color.Red;
                                    else if (y / SCALE_FACTOR >= 192 && y / SCALE_FACTOR <= 239) pxColor = Color.LimeGreen;
                                    else if (y / SCALE_FACTOR > 239 && (x / SCALE_FACTOR >= 26 && x / SCALE_FACTOR <= 54)) pxColor = Color.LimeGreen;
                                }
                                context.Rectangle(ShapeMode.Fill, new Vector2(x, y), SCALE_FACTOR, SCALE_FACTOR, pxColor);
                            }
                        }
                    }
                }
            });

            if(UseShader)
            {
                ArcadeShader.Activate();
                ArcadeShader.SetUniform("CRT_CURVE_AMNTx", .2f);
                ArcadeShader.SetUniform("CRT_CURVE_AMNTy", .2f);
            }

            context.DrawTexture(Frame, Vector2.Zero, Vector2.One, Vector2.Zero, 0f);
            Shader.Deactivate();
        }
    }
}
