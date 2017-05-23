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
        SceneManager.LoadScene("Levels");
    }

    private void HandleLeaderboardsPressed()
    {
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
        OptionsScene.Show(false);
    }
}
