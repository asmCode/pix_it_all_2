using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RateMeController
{
    private RateMeView m_view;

    public void Init(RateMeView view)
    {
        m_view = view;

        var options = Game.GetInstance().Options;

        // m_view.BackPressed += HandleBackPressed;
        // m_view.SoundPressed += HandleSoundPressed;
        // m_view.MusicPressed += HandleMusicPressed;
    }

    private void HandleBackPressed()
    {
        Close();
    }

    private void HandleSoundPressed()
    {
        var options = Game.GetInstance().Options;

        options.ToggleSound();
    }

    private void HandleMusicPressed()
    {
        var options = Game.GetInstance().Options;

        options.ToggleMusic();
    }

    private void HandleRestorePurchasesPressed()
    {
    }
    
    private void Close()
    {
        // m_view.BackPressed -= HandleBackPressed;
        // m_view.SoundPressed -= HandleSoundPressed;
        // m_view.MusicPressed -= HandleMusicPressed;

        SceneManager.UnloadSceneAsync("RateMe");
    }
}
