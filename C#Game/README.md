# Your README for A2 HERE


https://github.com/user-attachments/assets/29c4d2de-f833-421d-affe-f6e2be2bc77c


This Dodge Game allows players to control a stickman(I tried to make it look like a stickman with two arms) using the arrow keys (or WASD) to dodge falling swords. The game gets progressively harder as swords fall faster, and the player's score increases for every sword they avoid. The game ends when the player is hit by a sword, and the final score is displayed.

The game consists of several files:
- **Game.cs**: Handles the game loop, updating the game state, player input, and drawing the game objects (player and obstacles).
- **Player.cs**: Manages the player character (stickman), including movement logic and drawing the character.
- **Obstacle.cs**: Represents the falling obstacles (swords), including their movement, collision detection with the player, and drawing.
- **Direction.cs**: Defines the possible directions for player movement (Left, Right, None).
