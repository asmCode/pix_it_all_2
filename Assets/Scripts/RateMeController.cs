using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RateMeController
{
    private RateMeView m_view;

    public static bool ShouldShowRateMe()
    {
        var persistent = Game.GetInstance().Persistent;

        var totalWins = persistent.GetTotalWins();
        var lastTimePresented = persistent.GetRateMeTimeWhenPresented();

        return
            !persistent.GetRateMeDismissed() &&
            totalWins > 0 &&
            totalWins % GameSettings.RateMeWinsCount == 0 &&
            (System.DateTime.Now - lastTimePresented).TotalDays >= GameSettings.RateMeDaysCount;
    }

    public void Init(RateMeView view)
    {
        m_view = view;

        var options = Game.GetInstance().Options;

        m_view.NowPressed += HandleNowPressed;
        m_view.LaterPressed += HandleLaterPressed;
        m_view.NeverPressed += HandleNeverPressed;
    }

    private void HandleNowPressed()
    {
        Application.OpenURL(GameSettings.RateMeUrl);

        DismissAndClose();
    }

    private void HandleLaterPressed()
    {
        Close();
    }

    private void HandleNeverPressed()
    {
        DismissAndClose();
    }

    private void DismissAndClose()
    {
        var persistent = Game.GetInstance().Persistent;
        persistent.SetRateMeDismissed(true);

        Close();
    }
    
    private void Close()
    {
        m_view.NowPressed -= HandleNowPressed;
        m_view.LaterPressed -= HandleLaterPressed;
        m_view.NeverPressed -= HandleNeverPressed;

        SceneManager.UnloadSceneAsync("RateMe");
    }
}
