namespace Chroma_Invaders
{
    public class Memory
    {
        private bool loadingRom = true;

        public void FinishLoading()
        {
            loadingRom = false;
        }

        private byte[] memory = new byte[0x4000];
        public byte this[int i]
        {
            get
            {
                i = i >= memory.Length ? 0x200 + (i % 0x400) : i;
                return memory[i];
            }
            set
            {
                i = i >= memory.Length ? 0x200 + (i % 0x400) : i;
                if (i < 0x2000 && !loadingRom) return;
                memory[i] = value;
            }
        }
    }
}
