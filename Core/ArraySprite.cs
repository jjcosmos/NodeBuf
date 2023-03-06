namespace NodeBuf.Core;

public struct ArraySprite
{
    public readonly Float4[] Data;
    public readonly int Width;
    public readonly int Height;

    public ArraySprite(Float4[] pixels, int width, int height)
    {
        Data = pixels;
        Width = width;
        Height = height;
    }
}