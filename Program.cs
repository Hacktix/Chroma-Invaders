using System;
using System.IO;

namespace Chroma_Invaders
{
    class Program
    {
        // private static readonly string[] ROM_NAMES = new string[] { "roms/invaders.h", "roms/invaders.g", "roms/invaders.f", "roms/invaders.e" };
        private static readonly string[] ROM_NAMES = new string[] { "roms/TST8080.COM" };

        static void Main(string[] args)
        {
            byte[][] roms = new byte[ROM_NAMES.Length][];
            for(int i = 0; i < ROM_NAMES.Length; i++)
            {
                if (File.Exists(ROM_NAMES[i]))
                    roms[i] = File.ReadAllBytes(ROM_NAMES[i]);
                else {
                    Console.WriteLine("Error: Missing ROM Part " + ROM_NAMES[i]);
                    return;
                }
            }
            new Emulator(roms).Run();
        }
    }
}
