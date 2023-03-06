namespace NodeBuf.Core;

public class Transform
{
    public Translation Translation;

    public void Translate(float x, float y, float z)
    {
        Translation = Translation.Translate(x, y, z);
    }
}