namespace NodeBuf.Core;

public class Game
{
    protected ISpriteLoader SpriteLoader;
    protected Renderer Renderer;
    protected double Time;
    protected bool Running { get; private set; }
    
    public Game(Window window, IPixelWriter writer, ISpriteLoader spriteLoader)
    {
        SpriteLoader = spriteLoader;
        Renderer = new Renderer(window, writer);
    }

    public void Run()
    {
        Running = true;
        OnStart();
    }
    
    public void Tick(double delta)
    {
        if (!Running)
            return;
        
        Time += delta;
        Renderer.Clear();
        OnTick(delta);
        Renderer.WriteBuffer();
    }

    protected virtual void OnStart()
    {
        
    }
    
    protected virtual void OnTick(double delta)
    {
        
    }
}