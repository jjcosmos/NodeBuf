namespace NodeBuf.Core;

public struct Int2
{
    public int X;
    public int Y;

    public Int2(int x, int y)
    {
        X = x;
        Y = y;
    }
        
    public Int2(float x, float y)
    {
        X = (int)x;
        Y = (int)y;
    }

    public override string ToString()
    {
        return $"({X},{Y})";
    }
}