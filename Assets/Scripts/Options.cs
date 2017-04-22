using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options
{
    private bool m_isSoundEnabled;
    private bool m_isMusicEnabled;

    public bool IsSoundEnabled
    {
        get { return m_isSoundEnabled; }
    }

    public bool IsMusicEnabled
    {
        get { return m_isMusicEnabled; }
    }

    public void ToggleSound()
    {
        m_isSoundEnabled = !m_isSoundEnabled;

        Save();
    }

    public void ToggleMusic()
    {
        m_isMusicEnabled = !m_isMusicEnabled;

        Save();
    }

    public void Load()
    {

    }

    public void Save()
    {

    }
}
