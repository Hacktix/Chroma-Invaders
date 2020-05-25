![Chroma Invaders](https://i.imgur.com/it2qTu5.png)

![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/Hacktix/Chroma-Invaders)
![GitHub last commit](https://img.shields.io/github/last-commit/Hacktix/Chroma-Invaders)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/Hacktix/Chroma-Invaders?label=latest%20release)
![GitHub release (latest by date including pre-releases)](https://img.shields.io/github/v/release/Hacktix/Chroma-Invaders?include_prereleases&label=latest%20release%20%28%2B%20pre-releases%29)

# What is this, exactly?
Chroma Invaders is an emulator of the classic Space Invaders arcade machine, based on the Chroma Framework. While there are multiple versions of the arcade machine, this is intended the original, using a black-and-white screen and an Intel 8080 processor running at 2MHz.

# Controls

| **Computer**            | **Controller**            | **Space Invaders**   |
|-------------------------|---------------------------|----------------------|
| Right Shift             | Right Stick (Press)       | Insert Coin          |
| Enter                   | Start                     | Start Game (1P)      |
| Right CTRL              | Select                    | Start Game (2P)      |
| Arrow Keys (Left/Right) | Left Stick (Controller 1) | Move Left/Right (P1) |
| Space                   | A (Controller 1)          | Shoot (P1)           |
| A / D                   | Left Stick (Controller 2) | Move Left/Right (P2) |
| W                       | A (Controller 2)          | Shoot (P2)           |
| C                       | Left Trigger              | Toggle Color         |
| F1                      | Right Trigger             | Toggle CRT-Mode      |

**Note for Controllers:** *Multiplayer still works with only one controller.* However, the controller needs to be passed between players, as it controls both Player 1 and Player 2.

**Another Note for Multi-Controller Play:** Starting the Game, toggling Color and CRT Mode as well as inserting coins can only be done by Controller 1.

# Technical Details
This project is being developed (more or less) side-by-side with another Space Invaders emulator called [THICCADE - Space Invaders](https://github.com/Hacktix/THICCADE-Space-Invaders), which is written in C++ and attempts to implement JIT-Recompilation. (Yes, I am aware that this isn't necessary for emulating an Intel 8080)

Chroma Invaders is the second emulator project based on the Chroma Framework, right after my first [CHROMA-8](https://github.com/Hacktix/CHROMA-8). It uses built-in Chroma features for both video and audio output.

# The ROMs
Yes, the project includes the original Space Invaders ROMs. These can be found in many, many repositories all over GitHub and other sites, too.

# Roadmap
* CPU Emulation ✓
* Input Handling ✓
* Colorized Screen ✓
* Sound ✓
* Two-Player Mode ✓
* Controller Support ✓

All the features above are a requirement before the first non-pre-release version 1.0 releases. Afterwards, the following updates are planned:

* Game Settings Configuration << We are Here
* "For-Fun" Features:
  - TILT Message
  - CPU Watchdog
* Netplay (Online Multiplayer)
* Improved Audio System

# Screenshots
### Now including authentic CRT-Mode!
![CRT Mode](https://i.imgur.com/2cHX2Zy.png)
### Entirely toggle-able.
![Splash Screen](https://i.imgur.com/Lwdq9LF.png)
![Gameplay](https://i.imgur.com/53n3Ffv.png)