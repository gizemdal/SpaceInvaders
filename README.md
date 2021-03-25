# Space Invaders: Deconstructed

<img src="imgs/readme.gif" width = 700>

**Name:** Gizem Dal
  - [Portfolio](https://www.gizemdal.com/), [Linkedin](https://www.linkedin.com/in/gizemdal/)
**Where to Play:** A MacOS executable is included under Mac_Exe

*I do not own the game logo or the sound effects used in this game project.*

### Player Goals & Objectives

This is a Unity 3D game project that is based on the arcade game [Space Invaders](https://www.youtube.com/watch?v=MU4psw3ccUI). The goal of Space Invaders is to kill the attacking aliens without getting hit, by controlling your spaceship with left and right keys and shooting at them.

Main Menu Scene | Game Over Scene
:---: | :---:
<img src="imgs/mainscreen.png" width = 300> | <img src="imgs/gameover.png" width = 300>

### Gameplay Rules

When the game is started, the user is presented with a scene that contains a batch of aliens, 4 dark blue shields and the player’s green spaceship. The current score is contained on the bottom left of the screen and the remaining lives of the player are shown on top of it. Using the left and right arrow keys, the player can move the ship along the 2-dimentional x-axis one unit at a time. The player will lose a life every time their ship gets hit by an alien attack. There are 3 ways to avoid getting hit by an alien:
- Dodging the attack by moving
- Hiding behind a shield
- Killing the attack with a bullet – this is the hardest one to achieve due to precision

The player must kill as many aliens as possible to earn points. If all the aliens on the screen are killed, a new batch of aliens will be spawned. The number of points the player can earn by killing an alien depend on the alien type. Points are awarded to the player using the following heuristic:
- 10 points per killing a magenta alien; located closest to the ship
- 20 points per killing a yellow alien; located in the middle
- 30 points per killing a cyan alien; located farthest from the ship
- Unknown number of points per destroying a spaceship

The player can also gain additional points by pushing the alien and spaceship debris off the platform.

### Challenges & Conflict

- As the number of remaining aliens decreases, they start moving faster thus it becomes harder to kill them.
- The aliens that are far from the ship will start getting closer as more aliens are killed. 
- The protecting shields lose volume each time they take a hit (including the player’s bullets!).
- The ship bullet reloading takes 0.5 seconds, thus the player cannot shoot bullets consecutively.

### Resources

Besides the shields, the player can be awarded 3 types of rewards during the gameplay. The condition for reward generation is killing 10 aliens/spaceships in a row without missing a bullet. The types of rewards include:
- **Extra Life:** This reward has 11% chance of getting spawned. If collected, it gives an extra life to the player.
- **Faster Bullet:** This reward has 44% chance of getting spawned. If collected, it speeds up the player bullet by 2x for 10 game seconds.
- **Faster Bullet Reload:** This reward has 44% chance of getting spawned. If collected, it speeds up the player bullet reloading by 10x for 10 game seconds.


Extra Life | Faster Bullet | Faster Bullet Reload
:---: | :---: | :---: 
<img src="imgs/power0.gif" width = 300> | <img src="imgs/power1.gif" width = 300> | <img src="imgs/power2.gif" width = 300>
