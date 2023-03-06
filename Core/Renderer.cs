using Godot;

namespace NodeBuf.Core;

public class Renderer
{
    public Camera Camera;
    public Window Window => _window;
    
    private Color[] _buf;
    private IPixelWriter _writer;
    private Window _window;
    
    public Renderer(Window win, IPixelWriter writer)
    {
        _window = win;
        _buf = new Color[win.Area()];
        _writer = writer;
        Camera = new Camera(win.Width, win.Height);
    }

    public PixelBounds GetRenderBounds()
    {
        var minX = Camera.Translation.X;
        var maxX = Camera.Translation.X + Camera.Bounds.X;
        var minY = Camera.Translation.Y;
        var maxY = Camera.Translation.Y + Camera.Bounds.Y;
        return new PixelBounds((int)minX, (int)maxX, (int)minY, (int)maxY);
    }

    public void WritePixel(int worldPositionX, int worldPositionY, Color color)
    {
        var screenX = (int)(worldPositionX - Camera.Translation.X);
        var screenY = (int)(worldPositionY - Camera.Translation.Y);

        if (PositionInBounds(screenX, screenY))
        {
            var windowIndex = screenY * _window.Width + screenX;
            WriteBlend(windowIndex, color);
        }
    }
    

    public void WriteSprite(ArraySprite sprite, Translation t)
    {
        var screenX = t.X - Camera.Translation.X;
        var screenY = t.Y - Camera.Translation.Y;
        
        for(var y = 0; y < sprite.Height; y ++)
        {
            for(var x = 0; x < sprite.Width; x++)
            {
                var pixelPosX = (int)(screenX + x);
                var pixelPosY = (int)(screenY + y);

                if (PositionInBounds(pixelPosX, pixelPosY))
                {
                    var spriteIndex = y * sprite.Width + x;
                    var windowIndex = pixelPosY * _window.Height + pixelPosX;
                    WriteBlend(windowIndex, sprite.Data[spriteIndex]);
                }
            }
        }
    }

    private void WriteBlend(int index, Color newColor)
    {
        _buf[index] = _buf[index].Blend(newColor);
    }

    private bool PositionInBounds(int x, int y)
    {
        return (x >= 0 && x < _window.Width && y >= 0 && y < _window.Height);
    }

    public void Clear()
    {
        var area = _window.Area();
        for (var i = 0; i < area; i++)
        {
            _buf[i] = _window.ClearColor;
        }
    }

    public void WriteBuffer()
    {
        _writer.Write(_buf);
    }
}