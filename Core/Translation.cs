namespace NodeBuf.Core;

public readonly struct Translation
{
    public readonly float X;
    public readonly float Y;
    public readonly float Z;

    public Translation(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Translation Translate(float x, float y, float z)
    {
        return new Translation(X + x, Y + y, Z + z);
    }
}