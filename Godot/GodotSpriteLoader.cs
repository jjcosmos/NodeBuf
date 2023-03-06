using Godot;
using NodeBuf.Core;

namespace NodeBuf.Godot;

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

    private static Float4[] ColorsFromImage(Image image)
    {
        var width = image.GetWidth();
        var height = image.GetHeight();
        var colors = new Float4[width * height];
        
        for(var y = 0; y < height; y ++)
        for (var x = 0; x < width; x ++)
        {
            var index = y * width + x;
            var gColor = image.GetPixel(x, y);
            colors[index] = new Float4(gColor.R, gColor.G, gColor.B, gColor.A);
        }

        return colors;
    }
}