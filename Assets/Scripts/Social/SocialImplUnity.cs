using UnityEngine;

namespace Ssg.Social
{
    public class SocialImplUnity : ISocialImpl
    {
		private UnityEngine.SocialPlatforms.ILeaderboard m_leaderboardTotalPixels;

        public bool IsLogEnabled { get; set; }

        public bool IsAuthenticated
        {
            get
            {
                if (IsLogEnabled)
                    Debug.Log("IsAuthenticated: " + UnityEngine.Social.localUser.authenticated.ToString());

				return UnityEngine.Social.localUser.authenticated;
            }
        }

        public string UserName
        {
            get
            {
                var localUser = UnityEngine.Social.localUser;
                if (localUser == null)
                    return null;

                return localUser.userName;
            }
        }

        public bool IsManualSignOutSupported
        {
            get
            {
#if UNITY_ANDROID
                return true;
#else
                return false;
#endif
            }
        }

        public void SignOut()
        {
            #if UNITY_ANDROID
                var gpgsSocial = UnityEngine.Social as GooglePlayGames.PlayGamesPlatform;
                if (gpgsSocial == null)
                    return;

                gpgsSocial.SignOut();
            #endif
        }

        public void Authenticate(System.Action<bool> callback)
        {
            if (IsLogEnabled)
                Debug.Log("Authenticate...");

            UnityEngine.Social.localUser.Authenticate((success) =>
            {
                if (IsLogEnabled)
                    Debug.Log("Authenticate result: " + success.ToString());

                if (callback != null)
                    callback(success);
            });
        }

        public void GetLocalUserScore(string leaderboardId, System.Action<Score> callback)
        {
            if (IsLogEnabled)
                Debug.Log("GetLocalUserScore...");

            if (m_leaderboardTotalPixels == null)
			{
				m_leaderboardTotalPixels = UnityEngine.Social.CreateLeaderboard();
				m_leaderboardTotalPixels.id = leaderboardId;      
			}

            m_leaderboardTotalPixels.LoadScores(result =>
			{
	    		bool success = m_leaderboardTotalPixels != null && m_leaderboardTotalPixels.localUserScore != null;
                if (IsLogEnabled)
                    Debug.Log("GetLocalUserScore result: " + success.ToString());
                Score score = null;
                if (success)
                {
                    score = new Score();
                    score.Value = m_leaderboardTotalPixels.localUserScore.value;
                    score.Rank = m_leaderboardTotalPixels.localUserScore.rank;

                    if (IsLogEnabled)
                    {
                        Debug.Log("GetLocalUserScore score.value = " + score.Value);
                        Debug.Log("GetLocalUserScore score.rank = " + score.Rank);
                    }
                }

		        if (callback != null)
		        	callback(score);
			});
        }

        public void ReportLocalUserScore(string leaderboardId, long score, System.Action<bool> callback)
        {
            if (IsLogEnabled)
                Debug.Log("ReportLocalUserScore...");

            UnityEngine.Social.ReportScore(score, leaderboardId, success =>
            {
                if (IsLogEnabled)
                    Debug.Log("ReportLocalUserScore result: " + success.ToString());

                if (callback != null)
                    callback(success);
			});
        }

        public void ShowLeaderboards()
        {
            UnityEngine.Social.ShowLeaderboardUI();
        }

        public void ReportAchievement(string achievementId)
        {
            var achievement = UnityEngine.Social.CreateAchievement();
            achievement.id = achievementId;
            achievement.percentCompleted = 100.0f;
            achievement.ReportProgress((bool success) => { });
        }
    }
}