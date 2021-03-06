﻿using UnityEngine;
using UnityEngine.UI;

public class PauseView : MonoBehaviour
{
    private static readonly string SaveAndBackToMenuButtonTitle = "SAVE AND EXIT";
    private static readonly string BackToMenuButtonTitle = "EXIT";

    public Text m_backToMenuButton;

    public event System.Action ResumeClicked;
    public event System.Action BackToMenuClicked;
    public event System.Action OptionsClicked;

    public bool IsActive
    {
        get { return gameObject.activeSelf; }
    }

    public void Show(bool is_save_available)
    {
        m_backToMenuButton.text = is_save_available ? SaveAndBackToMenuButtonTitle : BackToMenuButtonTitle;

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void UiEvent_ResumeClicked()
    {
        if (ResumeClicked != null)
            ResumeClicked();
    }

    public void UiEvent_BackToMenuClicked()
    {
        if (BackToMenuClicked != null)
            BackToMenuClicked();
    }

    public void UiEvent_OptionsClicked()
    {
        if (OptionsClicked != null)
            OptionsClicked();
    }
}
