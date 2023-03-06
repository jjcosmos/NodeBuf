using Godot;
using NodeBuf;
using NodeBuf.Core;
using Window = NodeBuf.Core.Window;

public partial class TexRect : TextureRect, IPixelWriter
{
	private Image _image;
	private ImageTexture _imTex;
	
	private int _width = 64;
	private int _height = 64;
	
	private Game _game;
	
	public override void _Ready()
	{
		_image = Image.Create(_width, _height, false, Image.Format.Rgba8);
		_imTex = ImageTexture.CreateFromImage(_image);
		Texture = _imTex;
		TextureFilter = TextureFilterEnum.Nearest;
		Engine.MaxFps = 30;

		var window = new Window(_width, _height, Colors.Blue);
		var loader = new GodotSpriteLoader();
		
		_game = new PongGame(window, this, loader);
		_game.Run();
	}

	public override void _Process(double delta)
	{
		_game.Tick(delta);
	}

	public void Write(Color[] pixels)
	{
		var area = _width * _height;
		for(var i = 0; i < area; i ++)
		{
			var y = i / _height;
			var x = i % _width;
			
			_image.SetPixel(x, y, pixels[i]);
		}
		_imTex.Update(_image);
	}
}
