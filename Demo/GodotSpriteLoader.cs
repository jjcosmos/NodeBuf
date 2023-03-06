using Godot;
using NodeBuf.Core;

namespace NodeBuf;

public class GodotSpriteLoader : ISpriteLoader
{
    public ArraySprite Load(string path)
    {
        var loaded = GD.Load<Texture2D>(path);
        var image = loaded.GetImage();
        var width = image.GetWidth();
        var height = image.GetHeight();
        var colors = ColorsFromImage(image);
        
        return new ArraySprite(colors, width, height);
    }

    private static Color[] ColorsFromImage(Image image)
    {
        var width = image.GetWidth();
        var height = image.GetHeight();
        var colors = new Color[width * height];
        
        for(var y = 0; y < height; y ++)
        for (var x = 0; x < width; x ++)
        {
            var index = y * width + x;
            colors[index] = image.GetPixel(x, y);
        }

        return colors;
    }

    public static Color[] ColorsFromFloatBuf(float[] data)
    {
        const int channels = 4;
        var colors = new Color[data.Length / channels];
        for (var i = 0; i < data.Length; i+=channels)
        {
            colors[i / 3] = new Color(data[i], data[i + 1], data[i + 2], data[i + 3]);
        }

        return colors;
    }
}