using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    public Color m_colorOff;
    public Color m_colorOn;
    public Image m_icon;

    private bool m_toggle;

    public bool Toggle
    {
        get
        {
            return m_toggle;
        }

        set
        {
            m_toggle = value;
            UpdateView();
        }
    }

    public void ToggleState()
    {
        m_toggle = !m_toggle;
        UpdateView();
    }

    private void UpdateView()
    {
        m_icon.color = m_toggle ? m_colorOn : m_colorOff;
    }

    private void Start()
    {
        UpdateView();
    }
}
