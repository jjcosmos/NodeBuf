namespace NodeBuf.Demo;

public readonly struct CollisionBounds
{
    public readonly int Width;
    public readonly int Height;
    public readonly int PxOffsetX;
    public readonly int PxOffsetY;

    public CollisionBounds(int width, int height, int pxOffsetX, int pxOffsetY)
    {
        Width = width;
        Height = height;
        PxOffsetX = pxOffsetX;
        PxOffsetY = pxOffsetY;
    }
}