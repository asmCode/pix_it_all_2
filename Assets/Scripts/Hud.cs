﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Hud : MonoBehaviour
{
    public Palette m_palette;
    public Image m_paletteColor;
    public TimeView m_timeView;

    public event System.Action PreviewPressed;
    public event System.Action PreviewReleased;
    public event System.Action PaletteClicked;
    public event System.Action PauseClicked;
    public event System.Action CheatFillColorsClicked;

    public void Init(Color[] paletteColors)
    {
        m_palette.Init(paletteColors);
    }

    public void SetTime(float time)
    {
        m_timeView.SetTime(time);
    }

    public void ShowPenalty()
    {
        m_timeView.BlinkRed();
    }

    public void ShowBonus()
    {
        m_timeView.BlinkGreen();
    }

    public void SetPaleteButtonColor(Color color)
    {
        m_paletteColor.color = color;
    }

    public void UiEventPreviewPressed()
    {
        if (PreviewPressed != null)
            PreviewPressed();
    }

    public void UiEventPreviewReleased()
    {
        if (PreviewReleased != null)
            PreviewReleased();
    }

    public void UiEventPaletteClicked()
    {
        if (PaletteClicked != null)
            PaletteClicked();
    }

    public void UiEventPauseClicked()
    {
        if (PauseClicked != null)
            PauseClicked();
    }

    public void UiEventCheatFillColorsClicked()
    {
        if (CheatFillColorsClicked != null)
            CheatFillColorsClicked();
    }
}
