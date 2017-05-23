namespace Ssg.Social
{
    public class Social : MonoBehaviourSingleton<Social, MonoBehaviourSingletonMeta>
    {
        private ISocialImpl m_impl;

        public event System.Action<bool> Authenticated;

        public bool LogEnabled
        {
            get { return m_impl.IsLogEnabled; }
            set { m_impl.IsLogEnabled = value; }
        }

        public bool IsAuthenticated
        {
            get
            {
                if (m_impl != null)
                    return m_impl.IsAuthenticated;
                else
                    return false;
            }
        }

        public void Authenticate(System.Action<bool> callback)
        {
            if (m_impl != null)
                m_impl.Authenticate(success =>
                {
                    if (Authenticated != null)
                        Authenticated(success);

                    if (callback != null)
                        callback(success);
                });
        }

        public void GetLocalUserScore(string leaderboardId, System.Action<Score> callback)
        {
            if (m_impl != null)
                m_impl.GetLocalUserScore(leaderboardId, callback);
        }

        public void ReportLocalUserScore(string leaderboardId, long score, System.Action<bool> callback)
        {
            if (m_impl != null)
                m_impl.ReportLocalUserScore(leaderboardId, score, callback);
        }

        public void ShowLeaderboards()
        {
            if (m_impl != null)
                m_impl.ShowLeaderboards();
        }

        public void ReportAchievement(string achievementId)
        {
            if (m_impl != null)
                m_impl.ReportAchievement(achievementId);
        }

        protected override void Awake()
        {
            CreateImplInstance();
        }

        private void CreateImplInstance()
        {
#if UNITY_EDITOR
            m_impl = new SocialImplEditor();
#elif UNITY_IOS
            m_impl = new SocialImplUnity();
#elif UNITY_ANDROID
            GooglePlayGames.PlayGamesPlatform.DebugLogEnabled = GGHeroGame.Debug;
            GooglePlayGames.PlayGamesPlatform.Activate();
            m_impl = new SocialImplUnity();
#endif
        }
    }
}
