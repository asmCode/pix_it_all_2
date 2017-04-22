using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsController
{
    private OptionsView m_view;

    public void Init(OptionsView view)
    {
        m_view = view;

        var options = Game.GetInstance().Options;

        m_view.BackPressed += HandleBackPressed;
        m_view.SoundPressed += HandleSoundPressed;
        m_view.MusicPressed += HandleMusicPressed;
        m_view.RestorePurchasesPressed += HandleRestorePurchasesPressed;
        
        m_view.SetVersion(Application.version);
        m_view.SetSoundEnabled(options.IsSoundEnabled);
        m_view.SetMusicEnabled(options.IsMusicEnabled);
    }

    private void HandleBackPressed()
    {
        Close();
    }

    private void HandleSoundPressed()
    {
        var options = Game.GetInstance().Options;

        options.ToggleSound();

        m_view.SetSoundEnabled(options.IsSoundEnabled);
    }

    private void HandleMusicPressed()
    {
        var options = Game.GetInstance().Options;

        options.ToggleMusic();

        m_view.SetMusicEnabled(options.IsMusicEnabled);
    }

    private void HandleRestorePurchasesPressed()
    {
    }

    private void Close()
    {
        m_view.BackPressed -= HandleBackPressed;
        m_view.SoundPressed -= HandleSoundPressed;
        m_view.MusicPressed -= HandleMusicPressed;
        m_view.RestorePurchasesPressed -= HandleRestorePurchasesPressed;

        SceneManager.UnloadSceneAsync("Options");
    }
}
