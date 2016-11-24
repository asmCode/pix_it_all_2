public class RectSides
{
    public float Left { get; set; }
    public float Right { get; set; }
    public float Top { get; set; }
    public float Bottom { get; set; }

    public float Width
    {
        get { return Right - Left; }
    }

    public float Height
    {
        get { return Top - Bottom; }
    }

    public float CenterVert
    {
        get { return (Right + Left) / 2.0f; }
    }

    public float CenterHori
    {
        get { return (Top + Bottom) / 2.0f; }
    }
}