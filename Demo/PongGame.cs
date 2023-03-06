using System;
using System.Collections.Generic;
using Godot;
using NodeBuf.Core;
using Translation = NodeBuf.Core.Translation;
using Window = NodeBuf.Core.Window;

namespace NodeBuf.Demo;

public class PongGame : Game
{
    private ArraySprite _playerSprite;
    private ArraySprite _ballSprite;

    private ArraySprite _player0WinSprite;
    private ArraySprite _player1WinSprite;
    
    private List<WorldSprite> _worldSprites;
    
    private WorldSprite _player0;
    private WorldSprite _player1;
    private WorldSprite _ball;

    private Random _random;
    private bool _showingWin;
    private float _winTimer;
    private WorldSprite _winText;

    private Int2[] _trail;

    public PongGame(Window window, IPixelWriter writer, ISpriteLoader spriteLoader) : base(window, writer, spriteLoader)
    {
    }

    protected override void OnStart()
    {
        var window = Renderer.Window;

        _random = new Random();
        _trail = new Int2[20];
        
        _playerSprite = SpriteLoader.Load("res://Demo/Sprites/Player.png");
        _ballSprite = SpriteLoader.Load("res://Demo/Sprites/Ball.png");
        
        _player0WinSprite = SpriteLoader.Load("res://Demo/Sprites/P1Goal.png");
        _player1WinSprite = SpriteLoader.Load("res://Demo/Sprites/P2Goal.png");
        
        _worldSprites = new List<WorldSprite>();

        var playerCollisionBounds = new CollisionBounds(2, 8, 3, 0);
        
        _player0 = new WorldSprite()
        {
            Sprite = _playerSprite, 
            Translation = new Translation(window.Width - _playerSprite.Width, window.Height / 2f, 0),
            Collision = playerCollisionBounds,
            Velocity = new Velocity(),
        };
        
        _player1 = new WorldSprite()
        {
            Sprite = _playerSprite, 
            Translation = new Translation(0, window.Height / 2f, 0),
            Collision = playerCollisionBounds,
            Velocity = new Velocity(),
        };
        
        _ball = new WorldSprite()
        {
            Sprite = _ballSprite, 
            Translation = new Translation(window.Height / 2f, window.Height / 2f, 0),
            Collision = new CollisionBounds(2, 2, 3, 3),
            Velocity = new Velocity(),
        };
        
        ResetMap();
        
        _worldSprites.Add(_player0);
        _worldSprites.Add(_player1);
        _worldSprites.Add(_ball);
    }

    private const string UP = "ui_up";
    private const string DOWN = "ui_down";

    protected override void OnTick(double delta)
    {
        // If the camera moves, do it before drawing.
        // Sprites are written directly to the pixel buffer relative to the camera.

        if (_showingWin)
        {
            _winTimer -= (float)delta;
            if (_winTimer <= 0)
            {
                _showingWin = false;
                HideWin();
            }
        }

        if (!_showingWin)
        {
            SetPlayerVelocity();
            MoveAi(delta);
            MoveSprites(delta);
            MakeTrail();
            ClampPlayers();
            CollideBall();
        }

        // Now write sprites/fx to the screen.
        RenderTrail();
        WriteSprites();
    }

    private void ShowWin(bool isLocalPlayer)
    {
        _winTimer = 2f;
        _showingWin = true;

        var window = Renderer.Window;
        
        _winText = new WorldSprite()
        {
            Sprite = isLocalPlayer ? _player0WinSprite : _player1WinSprite,
            Translation = new Translation(window.Width / 2f - _player0WinSprite.Width / 2f, window.Height / 3f, 0),
            Collision = new CollisionBounds(0, 0, 0, 0),
            Velocity = new Velocity()
        };
        
        _worldSprites.Add(_winText);
    }

    private void MakeTrail()
    {
        for(var i = _trail.Length - 1; i > 0; i --)
        {
            _trail[i] = _trail[i - 1];
        }

        _trail[0] = _ball.Center();
    }

    private void RenderTrail()
    {
        for (var i = 0; i < _trail.Length; i++)
        {
            var alpha = 1f - (float)i / _trail.Length;
            Renderer.WritePixel(_trail[i].X, _trail[i].Y, new Float4(1f, 1f, 1f, alpha));
        }
    }

    private void HideWin()
    {
        _worldSprites.Remove(_winText);
    }
    

    private void MoveSprites(double delta)
    {
        foreach (var worldSprite in _worldSprites)
        {
            worldSprite.Translate(worldSprite.Velocity.X * (float)delta, worldSprite.Velocity.Y * (float)delta, 0);
        }
    }

    private void ClampPlayers()
    {
        var bounds = Renderer.GetRenderBounds();
        ClampToRenderBounds(_player0, bounds);
        ClampToRenderBounds(_player1, bounds);
    }

    private void MoveAi(double delta)
    {
        var deltaY = _ball.Translation.Y - _player1.Translation.Y;
        _player1.Translate(0f, deltaY * (float)delta * 3f, 0f);
    }

    private void SetPlayerVelocity()
    {
        var axisInput = Input.GetAxis(DOWN, UP) * 50f;
        _player0.Velocity.Y = -axisInput;
    }

    private void CollideBall()
    {
        var overlappedP0 = DidOverlap(_player0.Translation, _player0.Collision, _ball.Translation, _ball.Collision);
        var overlappedP1 = DidOverlap(_player1.Translation, _player1.Collision, _ball.Translation, _ball.Collision);

        if (_ball.Velocity.X > 0 && overlappedP0)
        {
            _ball.Velocity.X *= -1;
        }
        
        if (_ball.Velocity.X < 0 && overlappedP1)
        {
            _ball.Velocity.X *= -1;
        }

        var renderBounds = Renderer.GetRenderBounds();
        var ballBounds = GetWorldBounds(_ball.Translation, _ball.Collision);
        
        if (_ball.Velocity.Y > 0 && ballBounds.TopLeft.Y  >= renderBounds.MaxY)
        {
            _ball.Velocity.Y *= -1;
        }
        
        if (_ball.Velocity.Y < 0 && ballBounds.BottomRight.Y <= renderBounds.MinY)
        {
            _ball.Velocity.Y *= -1;
        }

        if (ballBounds.TopLeft.X < renderBounds.MinX)
        {
            ShowWin(true);
            ResetMap();
        }

        if (ballBounds.BottomRight.X > renderBounds.MaxX)
        {
            ShowWin(false);
            ResetMap();
        }
    }

    private void ResetMap()
    {
        var launchSpeed = 30f;
        var maxY = launchSpeed / 1.2f;
        var window = Renderer.Window;
        
        _ball.Translation = new Translation(window.Width / 2f - _ball.Sprite.Width / 2f, window.Height / 2f - _ball.Sprite.Height / 2f, 0);
        _ball.Velocity = new Velocity(RandSign() * launchSpeed, RandRange(maxY / 2f, maxY) * RandSign());

        _player0.Translation = new Translation(window.Width - _playerSprite.Width, window.Height / 2f, 0);
        _player1.Translation = new Translation(0, window.Height / 2f, 0);

        for (var i = 0; i < _trail.Length; i++)
        {
            _trail[i] = _ball.Center();
        }
    }

    private int RandSign()
    {
        return _random.NextDouble() > 0.5 ? 1 : -1;
    }

    private float RandRange(float min, float max)
    {
        var lerp = (float)_random.NextDouble();
        return Lerp(min, max, lerp);
    }
    
    private static float Lerp(float a, float b, float f)
    {
        return a * (1f - f) + (b * f);
    }

    private void WriteSprites()
    {
        foreach (var wSpr in _worldSprites)
        {
            Renderer.WriteSprite(wSpr.Sprite, wSpr.Translation);
        }
    }

    private static void ClampToRenderBounds(WorldSprite wSpr, PixelBounds bounds)
    {
        var clampedX = Math.Clamp(wSpr.Translation.X, bounds.MinX, bounds.MaxX - wSpr.Sprite.Width);
        var clampedY = Math.Clamp(wSpr.Translation.Y, bounds.MinY, bounds.MaxY - wSpr.Sprite.Height);

        wSpr.Translation = new Translation(clampedX, clampedY, wSpr.Translation.Z);
    }

    private static bool DidOverlap(Translation tFormA, CollisionBounds boundsA, Translation tFormB, CollisionBounds boundsB)
    {
        var bounds1 = GetWorldBounds(tFormA, boundsA);
        var bounds2 = GetWorldBounds(tFormB, boundsB);
        
        if (bounds1.TopLeft.X == bounds1.BottomRight.X || bounds1.TopLeft.Y == bounds1.BottomRight.Y || bounds2.BottomRight.X == bounds2.TopLeft.X || bounds2.TopLeft.Y == bounds2.BottomRight.Y)
            return false;

        if (bounds1.TopLeft.X > bounds2.BottomRight.X || bounds2.TopLeft.X > bounds1.BottomRight.X)
            return false;

        if (bounds1.BottomRight.Y > bounds2.TopLeft.Y || bounds2.BottomRight.Y > bounds1.TopLeft.Y)
            return false;

        return true;
    }

    private static Bounds GetWorldBounds(Translation tForm, CollisionBounds bounds)
    {
        var minX = tForm.X + bounds.PxOffsetX;
        var maxX = tForm.X + bounds.PxOffsetX + bounds.Width;

        var minY = tForm.Y + bounds.PxOffsetY;
        var maxY = tForm.Y + bounds.PxOffsetY + bounds.Height;

        var topLeft = new Int2(minX, maxY);
        var bottomRight = new Int2(maxX, minY);
        
        return new Bounds(){TopLeft = topLeft, BottomRight = bottomRight};
    }
}