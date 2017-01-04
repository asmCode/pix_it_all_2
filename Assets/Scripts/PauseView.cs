using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseView : MonoBehaviour
{
    public event System.Action ResumeClicked;
    public event System.Action BackToMenuClicked;

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
