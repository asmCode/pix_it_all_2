using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WellcomeController
{
    private WellcomeView m_view;

    public void Init(WellcomeView view)
    {
        m_view = view;

        m_view.PlayPressed += HandlePlayPressed;
        m_view.LeaderboardsPressed += HandleLeaderboardsPressed;
        m_view.OptionsPressed += HandleOptionsPressed;
    }

    private void HandlePlayPressed()
    {
        Fade.FadeIn(null, true, () =>
        {
            Pix.Game.GetInstance().ShowBundlesScene();
        });
    }

    private void HandleLeaderboardsPressed()
    {
        GameAnalyticsSDK.GameAnalytics.NewDesignEvent("button.wellcome.leaderboards");

        var social = Ssg.Social.Social.GetInstance();

        if (social.IsAuthenticated)
            social.ShowLeaderboards();
        else
        {
            social.Authenticate(success =>
            {
                // Debug.Log("****************************************** Authenticated: " + success.ToString());
                // if (success)
                social.ShowLeaderboards();
            });
        }
    }

    private void HandleOptionsPressed()
    {
        Fade.FadeIn(null, true, () =>
        {
            OptionsScene.Show(false, () =>
            {
                Fade.FadeOut(null, false, null);
            });
        });
    }

    public void HandleBackButton()
    {
        Application.Quit();
    }
}
