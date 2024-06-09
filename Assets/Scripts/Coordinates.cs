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

public enum PipeShapes { Grass, LeftUp, LeftDown, RightUp, RightDown, Horizontal, Vertical }

public enum WhereIsYTarget {  Up , Down , Streight}

public enum Difficulty { Easy, Medium, Hard }