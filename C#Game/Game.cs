using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

public class Game
{
    private Player player; // Player object
    private List<Obstacle> obstacles; // List of obstacles
    private Random rand = new Random();
    private bool gameOver = false; // Flag for game over
    private int score = 0; // Game score
    private int obstacleSpawnTimer = 0; // Timer for spawning obstacles
    private int obstacleSpawnRate = 10; // Spawn a new obstacle every 10 frames

    public void Setup()
    {
        player = new Player(300, 450); // Initialize player
        obstacles = new List<Obstacle>(); // Initialize obstacle list
    }

    public void Update(float dt)
    {
        if (!gameOver)
        {
            player.Update();

            // Spawn a new obstacle at a regular interval
            obstacleSpawnTimer++;
            if (obstacleSpawnTimer >= obstacleSpawnRate)
            {
                obstacleSpawnTimer = 0;
                int xPosition = rand.Next(0, Window.width - 50); // Random x position for obstacle
                obstacles.Add(new Obstacle(xPosition, 0)); // Spawn obstacle at the top
            }

            // Update all obstacles
            for (int i = obstacles.Count - 1; i >= 0; i--)
            {
                obstacles[i].Update();
                if (obstacles[i].IsOffScreen(Window.height))
                {
                    obstacles.RemoveAt(i);
                    score++; // Increase score for every avoided obstacle
                }
                else if (obstacles[i].IsCollidingWith(player))
                {
                    gameOver = true;
                    MessageBox.Show($"Game Over! Score: {score}");
                    System.Windows.Forms.Application.Exit();
                }
            }
        }
    }

    public void Draw(Graphics g)
    {
        if (!gameOver)
        {
            player.Draw(g);

            // Draw all obstacles
            foreach (var obstacle in obstacles)
            {
                obstacle.Draw(g);
            }

            // Display the score
            g.DrawString($"Score: {score}", new Font("Arial", 16), Brushes.Black, 10, 10);
        }
    }

    public void KeyDown(KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Left)
        {
            player.Move(Direction.Left);
        }
        else if (e.KeyCode == Keys.Right)
        {
            player.Move(Direction.Right);
        }
    }

    public void MouseClick(MouseEventArgs e)
    {
        // You can add other mouse click functionalities if needed
    }
}
