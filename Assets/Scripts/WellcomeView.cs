using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WellcomeView : MonoBehaviour
{
    public event System.Action PlayPressed;
    public event System.Action LeaderboardsPressed;
    public event System.Action OptionsPressed;

    public void UiEvent_PlayButtonPreesed()
    {
        if (PlayPressed != null)
            PlayPressed();
    }

    public void UiEvent_LeaderboarsButtonPressed()
    {
        if (LeaderboardsPressed != null)
            LeaderboardsPressed();
    }

    public void UiEvent_OptionsButtonPressed()
    {
        if (OptionsPressed != null)
            OptionsPressed();
    }
}
