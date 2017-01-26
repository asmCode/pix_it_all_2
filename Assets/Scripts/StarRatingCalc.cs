public class StarRatingCalc
{
    private static readonly float TimePerTile2Stars = 2.0f;
    private static readonly float TimePerTile3Stars = 1.5f;

    public static int GetStars(float time, int tilesCount, int colorCount)
    {
        if (time <= RequiredTimeForStars(3, tilesCount, colorCount))
            return 3;

        if (time <= RequiredTimeForStars(2, tilesCount, colorCount))
            return 2;

        return 1;
    }

    public static float RequiredTimeForStars(int stars, int tilesCount, int colorsCount)
    {
        if (stars <= 1)
            return float.PositiveInfinity;

        if (stars == 2)
            return tilesCount * TimePerTile2Stars;

        if (stars >= 3)
            return tilesCount * TimePerTile3Stars;

        return 0;
    }
}
