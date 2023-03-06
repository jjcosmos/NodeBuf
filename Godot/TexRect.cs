using Godot;
using NodeBuf.Core;
using NodeBuf.Demo;
using Window = NodeBuf.Core.Window;

namespace NodeBuf.Godot;

public partial class TexRect : TextureRect, IPixelWriter
{
	private Image _image;
	private ImageTexture _imTex;
	
	private readonly int _width = 64;
	private readonly int _height = 64;
	
	private Game _game;
	
	public override void _Ready()
	{
		_image = Image.Create(_width, _height, false, Image.Format.Rgba8);
		_imTex = ImageTexture.CreateFromImage(_image);
		Texture = _imTex;
		TextureFilter = TextureFilterEnum.Nearest;
		Engine.MaxFps = 30;

		var window = new Window(_width, _height, new Float4(0, 0, 1f, 1f));
		var loader = new GodotSpriteLoader();
		
		_game = new PongGame(window, this, loader);
		_game.Run();
	}

	public override void _Process(double delta)
	{
		_game.Tick(delta);
	}

	public void Write(Float4[] pixels)
	{
		var counter = 0;
		for (var y = 0; y < _height; y++)
		{
			for (var x = 0; x < _width; x++)
			{
				var color = pixels[counter];
				_image.SetPixel(x, y, new Color(color.X, color.Y, color.Z, color.W));
				counter++;
			}
		}

		_imTex.Update(_image);
	}
}