﻿![Chroma Invaders](https://i.imgur.com/it2qTu5.png)

![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/Hacktix/Chroma-Invaders)
![GitHub last commit](https://img.shields.io/github/last-commit/Hacktix/Chroma-Invaders)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/Hacktix/Chroma-Invaders?label=latest%20release)
![GitHub release (latest by date including pre-releases)](https://img.shields.io/github/v/release/Hacktix/Chroma-Invaders?include_prereleases&label=latest%20release%20%28%2B%20pre-releases%29)

# What is this, exactly?
Chroma Invaders is an emulator of the classic Space Invaders arcade machine, based on the Chroma Framework. While there are multiple versions of the arcade machine, this is intended the original, using a black-and-white screen and an Intel 8080 processor running at 2MHz.

# Controls
| **Computer**            | **Space Invaders** |
|-------------------------|--------------------|
| Right Shift             | Insert Coin        |
| Enter                   | Start Game         |
| Arrow Keys (Left/Right) | Move Left/Right    |
| Space                   | Shoot              |

# Technical Details
This project is being developed (more or less) side-by-side with another Space Invaders emulator called [THICCADE - Space Invaders](https://github.com/Hacktix/THICCADE-Space-Invaders), which is written in C++ and attempts to implement JIT-Recompilation. (Yes, I am aware that this isn't necessary for emulating an Intel 8080)

Chroma Invaders is the second emulator project based on the Chroma Framework, right after my first [CHROMA-8](https://github.com/Hacktix/CHROMA-8). It uses built-in Chroma features for video output, while audio will be handled by my [ChromaSynth](https://github.com/Hacktix/ChromaSynth) library, which allows for audio synthesis at runtime.

# The ROMs
Yes, the project includes the original Space Invaders ROMs. These can be found in many, many repositories all over GitHub and other sites, too.

# Roadmap
* CPU Emulation ✓
* Input Handling ✓
* Colorized Screen ✓
* Sound << We are Here
* Two-Player Mode
* Controller Support

# Screenshots
![Splash Screen](https://i.imgur.com/Lwdq9LF.png)
![Gameplay](https://i.imgur.com/53n3Ffv.png)