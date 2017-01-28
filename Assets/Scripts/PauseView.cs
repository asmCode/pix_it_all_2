using UnityEngine;
using UnityEngine.UI;

public class PauseView : MonoBehaviour
{
    private static readonly string SaveAndBackToMenuButtonTitle = "SAVE AND BACK TO MENU";
    private static readonly string BackToMenuButtonTitle = "BACK TO MENU";

    public Text m_backToMenuButton;

    public event System.Action ResumeClicked;
    public event System.Action BackToMenuClicked;

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
}
