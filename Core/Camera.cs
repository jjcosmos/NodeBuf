namespace NodeBuf.Core;

public class Camera : Transform
{
    public Int2 Bounds;

    public Camera(int boundsX, int boundsY)
    {
        Bounds = new Int2(boundsX, boundsY);
    }
}