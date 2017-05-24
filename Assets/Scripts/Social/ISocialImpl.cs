namespace Ssg.Social
{
    public interface ISocialImpl
    {
        bool IsLogEnabled { get; set; }
        bool IsAuthenticated { get; }
        string UserName { get; }
        bool IsManualSignOutSupported { get; }
        void SignOut();
        void Authenticate(System.Action<bool> callback);
        void GetLocalUserScore(string leaderboardId, System.Action<Score> callback);
        void ReportLocalUserScore(string leaderboardId, long score, System.Action<bool> callback);
        void ShowLeaderboards();
        void ReportAchievement(string achievementId);
    }
}
