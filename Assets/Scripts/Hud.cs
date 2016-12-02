using UnityEngine;
using System.Collections;

public class Hud : MonoBehaviour
{
    public event System.Action PreviewPressed;
    public event System.Action PreviewReleased;
    public event System.Action PaletteClicked;
    public event System.Action PauseClicked;

    public void UiEventPreviewPressed()
    {
        if (PreviewPressed != null)
            PreviewPressed();

        Debug.Log("UiEventPreviewPressed");
    }

    public void UiEventPreviewReleased()
    {
        if (PreviewReleased != null)
            PreviewReleased();

        Debug.Log("UiEventPreviewReleased");
    }

    public void UiEventPaletteClicked()
    {
        if (PaletteClicked != null)
            PaletteClicked();

        Debug.Log("UiEventPaletteClicked");
    }

    public void UiEventPauseClicked()
    {
        if (PauseClicked != null)
            PauseClicked();

        Debug.Log("UiEventPauseClicked");
    }
}
