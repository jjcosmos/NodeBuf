using Godot;

namespace NodeBuf.Core;

public struct ArraySprite
{
    public readonly Color[] Data;
    public readonly int Width;
    public readonly int Height;

    public ArraySprite(Color[] pixels, int width, int height)
    {
        Data = pixels;
        Width = width;
        Height = height;
    }
}