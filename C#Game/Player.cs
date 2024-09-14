using System;
using System.Drawing;

public class Player
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Width { get; private set; } = 50;
    public int Height { get; private set; } = 50;
    private int speed = 10;
    private Direction moveDirection = Direction.None;

    public Player(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void Update()
    {
        if (moveDirection == Direction.Left)
        {
            X -= speed;
        }
        else if (moveDirection == Direction.Right)
        {
            X += speed;
        }

        // Ensure the player doesn't move out of screen bounds
        if (X < 0) X = 0;
        if (X + Width > Window.width) X = Window.width - Width;
    }

    public void Move(Direction direction)
    {
        moveDirection = direction;
    }

    public void Draw(Graphics g)
    {
        Pen pen = new Pen(Color.Black, 3);

        g.DrawEllipse(pen, X + Width / 4, Y, Width / 2, Width / 2); 

        g.DrawLine(pen, X + Width / 2, Y + Width / 2, X + Width / 2, Y + Height / 2); 

        g.DrawLine(pen, X + Width / 2, Y + Width, X, Y + Height / 2); 

        g.DrawLine(pen, X + Width / 2, Y + Width, X + Width, Y + Height / 2); 

    }
}
