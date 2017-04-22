using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsView : MonoBehaviour
{
    public Text m_labelVersion;
    public Text m_labelSound;
    public Text m_labelMusic;

    public event System.Action SoundPressed;
    public event System.Action MusicPressed;
    public event System.Action RestorePurchasesPressed;
    public event System.Action BackPressed;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void SetVersion(string version)
    {
        m_labelVersion.text = version;
    }

    public void SetSoundEnabled(bool enabled)
    {
        m_labelSound.text = "SOUND " + (enabled ? "ON" : "OFF");
    }

    public void SetMusicEnabled(bool enabled)
    {
        m_labelMusic.text = "MUSIC " + (enabled ? "ON" : "OFF");
    }

    public void UiEvent_BackButtonPreesed()
    {
        if (BackPressed != null)
            BackPressed();
    }

    public void UiEvent_SoundButtonPressed()
    {
        if (SoundPressed != null)
            SoundPressed();
    }

    public void UiEvent_MusicButtonPressed()
    {
        if (MusicPressed != null)
            MusicPressed();
    }

    public void UiEvent_RestorePurchasesButtonPressed()
    {
        if (RestorePurchasesPressed != null)
            RestorePurchasesPressed();
    }
}
