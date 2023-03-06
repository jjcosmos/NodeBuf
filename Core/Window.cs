using Godot;

namespace NodeBuf.Core;

public struct Window
{
    public readonly int Width;
    public readonly int Height;
    public readonly Color ClearColor;

    public Window(int width, int height, Color clearColor)
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