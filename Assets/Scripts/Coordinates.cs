public struct Coordinates
{
    public int X;
    public int Y;

    public Coordinates(int X, int Y)
    {
        this.X = X;
        this.Y = Y;
    }
}

public enum PipeDirection { Left, Right, Up, Down }

public enum WhereIsYTarget {  Up , Down , Streight}

public enum Difficulty { Easy, Medium, Hard }