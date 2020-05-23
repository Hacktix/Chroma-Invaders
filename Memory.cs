using System;
using System.Collections.Generic;
using System.Text;

namespace Chroma_Invaders
{
    public class Memory
    {
        private byte[] memory = new byte[0x4000];
        public byte this[int i]
        {
            get
            {
                i = i >= memory.Length ? 0x2000 + (i % 0x400) : i;
                return memory[i];
            }
            set
            {
                i = i >= memory.Length ? 0x2000 + (i % 0x400) : i;
                memory[i] = value;
            }
        }
    }
}
