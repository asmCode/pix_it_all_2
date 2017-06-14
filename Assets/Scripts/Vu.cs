using UnityEngine;

public class Vu
{
    public static Vector3 AddX(Vector3 v, float x)
    {
        var result = v;
        result.x += x;
        return result;
    }

    public static Vector3 AddY(Vector3 v, float y)
    {
        var result = v;
        result.y += y;
        return result;
    }

    public static Vector3 SetY(Vector3 v, float y)
    {
        var result = v;
        result.y = y;
        return result;
    }

    public static Vector2 XY(Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }
}
