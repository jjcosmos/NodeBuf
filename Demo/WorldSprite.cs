using NodeBuf.Core;

namespace NodeBuf.Demo;
public class WorldSprite : Transform
{
    public ArraySprite Sprite;
    public CollisionBounds Collision;
    public Velocity Velocity;

    public Int2 Center()
    {
        return new Int2(Translation.X + Sprite.Width / 2f, Translation.Y + Sprite.Height / 2f);
    }
}