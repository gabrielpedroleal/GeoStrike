🔻🔺 GeoStrike 🔺🔻

📝 Description

You are an circle space adventurer that is being attacked by triangles space pirates, the goal is to survive for four minutes until the enemies stop coming.

✨ Main Features

🔄 Gameplay Loop: Seamless transitions between main menu, gameplay, victory and game over screens.

⏳ Phased Horde System: The EnemySpawner manages difficulty in real time throgh programmable phases.

🛠️ UI/UX: Dynamic tutorial on "the ground" with fade-out system.

  - Biligual localization system (Brazilian Portuguese/English).
    
  - Custom crosshair cursor integrated into the engine.

🔊 Audio Managment: Centralized AudioManager using AudioMixer.

🎥 Smart Camera: Cinemachine with Confiner 2D to ensure the player's view never leaves the map boundaries.

🕹️ How to Play

1. You can download the repository and run the .exe file in the build folder or xxxxx.
   
2. WASD to move the character.
   
3. Use the cursor as the aim, and the left button to shoot.
   
4. Also press spacebar to dash and become invincible for a few frames.
   
   Survive until the timer reaches 00:00.

🛠️ Project Architecture
GameManager: Constrols the state, the survival timer and victory conditions.

PoolManager: Object pooling system for projectiles, collectibles and enemies.

EnemySpawner: Stage based system that scales difficulty according to the game timer.

AudioManager: Singleton system for triggering SFX and music.

------ Thanks for checking out my project! ------

Also huge thanks to all content creators who provided free assets that helped me bring this game to life! 🙌

Assets Credits: 

8-bit Hurt.aif by timgormly - https://freesound.org/s/170149/

Equipment Failure FX 1.wav by OptronTeamFilms - https://freesound.org/s/551174/

SFX_WooshShortDash8 by kx.audio - https://freesound.org/s/760002/

51 UI sound effects (buttons, switches and clicks) by Kenney.nl - www.kenney.nl

8Bit Music - 062022 by GWriterStudio - https://assetstore.unity.com/packages/audio/music/8bit-music-062022-225623#description

Shooting Sound by B.G.M - https://assetstore.unity.com/packages/audio/sound-fx/shooting-sound-177096#publisher

Free retro pixel font - GNF by Nytrock - https://assetstore.unity.com/packages/2d/fonts/free-retro-pixel-font-gnf-322855#description

Guns_gameassets by KayLousberg

Last but no least, thanks to Unity for all the free resources and support! 🌟
