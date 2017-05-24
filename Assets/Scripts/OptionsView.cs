using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsView : MonoBehaviour
{
    public Text m_labelVersion;
    public Text m_labelSound;
    public Text m_labelMusic;
    public Text m_labelGPGS;
    public GameObject m_buttonGPGS;
    public GameObject m_buttonRestorePurchases;

    public event System.Action SoundPressed;
    public event System.Action MusicPressed;
    public event System.Action RestorePurchasesPressed;
    public event System.Action BackPressed;
    public event System.Action GPGSPressed;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void SetSocial(bool manualSignInOut, string nickname)
    {
        m_buttonGPGS.gameObject.SetActive(manualSignInOut);
        if (!manualSignInOut)
            return;

        if (string.IsNullOrEmpty(nickname))
        {
            m_labelGPGS.text = "SIGN IN TO GPGS";
        }
        else
        {
            m_labelGPGS.text = "SIGN OUT " + nickname;
        }
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

    public void SetRestorePurchasesEnabled(bool enabled)
    {
        m_buttonRestorePurchases.SetActive(enabled);
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

    public void UiEvent_GPGSButtonPressed()
    {
        if (GPGSPressed != null)
            GPGSPressed();
    }
}
