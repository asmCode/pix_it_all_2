using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options
{
    private OptionsData m_data;

    public event System.Action SoundChanged;
    public event System.Action MusicChanged;

    public bool IsSoundEnabled()
    {
        Init();

        return m_data.IsSoundEnabled;
    }

    public bool IsMusicEnabled()
    {
        Init();

        return m_data.IsMusicEnabled;
    }

    public void ToggleSound()
    {
        Init();

        m_data.IsSoundEnabled = !m_data.IsSoundEnabled;

        Save();

        if (SoundChanged != null)
            SoundChanged();
    }

    public void ToggleMusic()
    {
        Init();

        m_data.IsMusicEnabled = !m_data.IsMusicEnabled;

        Save();

        if (MusicChanged != null)
            MusicChanged();
    }

    public void Load()
    {
        var filePath = GetFilePath();

        m_data = JsonLoader.LoadFromFile<OptionsData>(filePath);
    }

    public void Save()
    {
        var filePath = GetFilePath();

        JsonLoader.SaveToFile(filePath, m_data);
    }

    private void Init()
    {
        if (m_data != null)
            return;

        Load();

        if (m_data == null)
            m_data = new OptionsData();
    }

    private static string GetFilePath()
    {
        return string.Format("{0}/options.json", Application.persistentDataPath);
    }
}
