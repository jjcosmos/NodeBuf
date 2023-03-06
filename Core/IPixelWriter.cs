using Godot;

namespace NodeBuf.Core;

public interface IPixelWriter
{
    public void Write(Color[] pixels);
}