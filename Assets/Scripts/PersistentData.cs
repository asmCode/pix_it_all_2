using System;

[Serializable]
public class PersistentData
{
    public bool SkipSocial;
    public int TotalPixelsRevealed;
    public bool RateMeDismissed;
    public long RateMeTimeWhenPresentedTimestamp;
    public int TotalWins;
    public bool FirstRun = true;
}
