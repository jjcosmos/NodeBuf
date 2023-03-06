namespace NodeBuf.Core;

public struct Window
{
    public readonly int Width;
    public readonly int Height;
    public readonly Float4 ClearColor;

    public Window(int width, int height, Float4 clearColor)
    {
        Width = width;
        Height = height;
        ClearColor = clearColor;
    }

    public int Area()
    {
        return Width * Height;
    }
}