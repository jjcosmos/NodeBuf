namespace NodeBuf.Core;

public struct PixelBounds
{
    public readonly int MinX;
    public readonly int MaxX;
    public readonly int MinY;
    public readonly int MaxY;
    
    public PixelBounds(int minX, int maxX, int minY, int maxY)
    {
        MinX = minX;
        MaxX = maxX;
        MinY = minY;
        MaxY = maxY;
    }
}