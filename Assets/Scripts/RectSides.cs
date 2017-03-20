using UnityEngine;

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

    public float CenterHori
    {
        get { return (Right + Left) / 2.0f; }
    }

    public float CenterVert
    {
        get { return (Top + Bottom) / 2.0f; }
    }

    public Vector2 Center
    {
        get { return new Vector2(CenterHori, CenterVert); }
    }

    public bool IsInsideHori(RectSides insideRect)
    {
        return
            Left <= insideRect.Left &&
            Right >= insideRect.Right;
    }

    public bool IsInsideVert(RectSides insideRect)
    {
        return
            Bottom <= insideRect.Bottom &&
            Top >= insideRect.Top;
    }

    public bool IsInside(RectSides insideRect)
    {
        return
            IsInsideHori(insideRect) &&
            IsInsideVert(insideRect);
    }
}