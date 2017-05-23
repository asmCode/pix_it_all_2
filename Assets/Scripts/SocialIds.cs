public class SocialIds
{
#if UNITY_ANDROID
    public static readonly string LeaderboardTotalPixels = GPGSIds.leaderboard_total_pixels;
#else
    public static readonly string LeaderboardTotalPixels = "pixitallfree.leaderboard.totalpixels";
#endif
}
