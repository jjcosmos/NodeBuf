namespace NodeBuf.Core;

public readonly struct Float4
{
    public readonly float X, Y, Z, W;

    public Float4(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }
    
    public Float4 Blend(Float4 over)
    {
        var n = 1f - over.W;
        var resultW = W * n + over.W;
        var x = Blend(X, over.X, W, over.W, n, resultW);
        var y = Blend(Y, over.Y, W, over.W, n, resultW);
        var z = Blend(Z, over.Z, W, over.W, n, resultW);
        return new Float4(x, y, z, resultW);
    }

    private float Blend(float thisValue, float thatVal, float thisBlend, float thatBlend, float n, float resultW)
    {
        return (thisValue * thisBlend * n + thatVal * thatBlend) / resultW;
    }
}