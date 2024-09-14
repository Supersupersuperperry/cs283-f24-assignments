using System;
using System.Drawing;

public class Obstacle
{
    public int X { get; private set; }
    public int Y { get; private set; }
    private int speed = 15; // Obstacle falling speed
    public int Width { get; private set; } = 50;
    public int Height { get; private set; } = 50;

    public Obstacle(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void Update()
    {
        Y += speed; // Move the obstacle downwards
    }

    public bool IsOffScreen(int screenHeight)
    {
        return Y > screenHeight;
    }

    public bool IsCollidingWith(Player player)
    {
        return X < player.X + player.Width && X + Width > player.X &&
               Y < player.Y + player.Height && Y + Height > player.Y;
    }

    // Draw a simple sword using Graphics
    public void Draw(Graphics g)
    {
        Brush swordBrush = new SolidBrush(Color.Gray);
        Pen pen = new Pen(Color.Black, 2);

        g.FillRectangle(swordBrush, X + Width / 4, Y, Width / 2, Height * 0.6f); 

        g.DrawLine(pen, X, Y + (int)(Height * 0.6), X + Width, Y + (int)(Height * 0.6)); 

        g.FillRectangle(swordBrush, X + Width / 3, Y + (int)(Height * 0.6), Width / 3, Height * 0.15f); 

        Point[] trianglePoints = {
            new Point(X + Width / 2, Y + Height), 
            new Point(X, Y + (int)(Height * 0.75)),
            new Point(X + Width, Y + (int)(Height * 0.75)) 
        };
        g.FillPolygon(swordBrush, trianglePoints); 
    }

    public void IncreaseSpeed()
    {
        speed += 1; 
    }
}
