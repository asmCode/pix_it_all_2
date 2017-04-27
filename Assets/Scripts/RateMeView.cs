using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RateMeView : MonoBehaviour
{
    public GameObject m_buttonRestorePurchases;

    public event System.Action NowPressed;
    public event System.Action LaterPressed;
    public event System.Action NeverPressed;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void UiEvent_NowButtonPreesed()
    {
        if (NowPressed != null)
            NowPressed();
    }

    public void UiEvent_LaterButtonPressed()
    {
        if (LaterPressed != null)
            LaterPressed();
    }

    public void UiEvent_NeverButtonPressed()
    {
        if (NeverPressed != null)
            NeverPressed();
    }
}
