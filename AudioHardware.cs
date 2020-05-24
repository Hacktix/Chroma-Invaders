using System;
using System.Collections.Generic;
using System.Text;

namespace Chroma_Invaders
{
    public class AudioHardware
    {
        private byte AudioPort3 = 0;
        private byte AudioPort5 = 0;

        public AudioHardware() { 
        }

        public void HandleInput(byte input, byte port)
        {
            switch(port)
            {
                case 3:
                    AudioPort3 = input;
                    break;
                case 5:
                    AudioPort5 = input;
                    break;
            }
            PlaySounds();
        }

        private void PlaySounds()
        {
            // UFO Sound
            if (IsSet(AudioPort3, 0)) Emulator.Sounds[7].Play();
            else Emulator.Sounds[7].Stop();

            // Shot Sound
            if (IsSet(AudioPort3, 1))
            {
                Unset(ref AudioPort3, 1);
                Emulator.Sounds[6].PlayOneShot();
            }

            // Player Hit Sound
            if (IsSet(AudioPort3, 2) && Emulator.Sounds[0].Status != Chroma.Audio.PlaybackStatus.Playing)
            {
                Unset(ref AudioPort3, 2);
                Emulator.Sounds[0].Play();
            }

            // Invader Hit Sound
            if (IsSet(AudioPort3, 3) && Emulator.Sounds[5].Status != Chroma.Audio.PlaybackStatus.Playing)
            {
                Unset(ref AudioPort3, 3);
                Emulator.Sounds[5].Play();
            }

            // Invader Walk Sounds
            if (IsSet(AudioPort5, 0))
            {
                Unset(ref AudioPort5, 0);
                Emulator.Sounds[1].PlayOneShot();
            }
            if (IsSet(AudioPort5, 1))
            {
                Unset(ref AudioPort5, 1);
                Emulator.Sounds[2].PlayOneShot();
            }
            if (IsSet(AudioPort5, 2))
            {
                Unset(ref AudioPort5, 2);
                Emulator.Sounds[3].PlayOneShot();
            }
            if (IsSet(AudioPort5, 3))
            {
                Unset(ref AudioPort5, 3);
                Emulator.Sounds[4].PlayOneShot();
            }
        }

        private bool IsSet(byte input, byte bit) {
            return (input & (1 << bit)) > 0;
        }

        private void Unset(ref byte input, byte bit)
        {
            input &= (byte)(~(1 << bit));
        }
    }
}
