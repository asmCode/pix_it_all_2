using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsController
{
    private OptionsView m_view;

    public void Init(OptionsView view, bool duringLevel)
    {
        m_view = view;

        var options = Pix.Game.GetInstance().Options;

        m_view.BackPressed += HandleBackPressed;
        m_view.SoundPressed += HandleSoundPressed;
        m_view.MusicPressed += HandleMusicPressed;
        m_view.RestorePurchasesPressed += HandleRestorePurchasesPressed;
        m_view.GPGSPressed += HandleGPGSPressed;
        
        m_view.SetVersion(Application.version);
        m_view.SetSoundEnabled(options.IsSoundEnabled());
        m_view.SetMusicEnabled(options.IsMusicEnabled());

        bool restorePurchaseAvailable = !duringLevel && Pix.Game.GetInstance().Purchaser.IsRestoreAvailable();
        m_view.SetRestorePurchasesEnabled(restorePurchaseAvailable);

        UpdateSocial();
    }

    public void HandleBackButton()
    {
        Close();
    }

    private void HandleBackPressed()
    {
        Close();
    }

    private void HandleSoundPressed()
    {
        var options = Pix.Game.GetInstance().Options;

        options.ToggleSound();

        m_view.SetSoundEnabled(options.IsSoundEnabled());
    }

    private void HandleMusicPressed()
    {
        var options = Pix.Game.GetInstance().Options;

        options.ToggleMusic();

        m_view.SetMusicEnabled(options.IsMusicEnabled());
    }

    private void HandleRestorePurchasesPressed()
    {
        Pix.Game.GetInstance().Purchaser.RestorePurchases();
    }

    private void HandleGPGSPressed()
    {
        var social = Ssg.Social.Social.GetInstance();

        if (social.IsAuthenticated)
        {
            Ssg.Social.Social.GetInstance().SignOut();
            UpdateSocial();
        }
        else
        {
            social.Authenticate(success =>
            {
                UpdateSocial();
            });
        }
    }
    
    private void Close()
    {
        m_view.BackPressed -= HandleBackPressed;
        m_view.SoundPressed -= HandleSoundPressed;
        m_view.MusicPressed -= HandleMusicPressed;
        m_view.RestorePurchasesPressed -= HandleRestorePurchasesPressed;
        m_view.GPGSPressed -= HandleGPGSPressed;

        SceneManager.UnloadSceneAsync("Options");
    }

    private void UpdateSocial()
    {
        var social = Ssg.Social.Social.GetInstance();
        if (social.IsManualSignOutSupported)
        {
            if (social.IsAuthenticated)
                m_view.SetSocial(true, social.UserName);
            else
                m_view.SetSocial(true, null);
        }
        else
            m_view.SetSocial(false, null);
    }
}
