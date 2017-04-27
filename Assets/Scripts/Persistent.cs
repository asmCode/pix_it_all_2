using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistent
{
    private PersistentData m_data;

    public bool GetRateMeDismissed()
    {
        Init();

        return m_data.RateMeDismissed;
    }

    public void SetRateMeDismissed(bool value)
    {
        Init();

        m_data.RateMeDismissed = value;

        Save();
    }

    public System.DateTime GetRateMeTimeWhenPresented()
    {
        Init();

        return new System.DateTime(m_data.RateMeTimeWhenPresentedTimestamp);
    }

    public void SetRateMeTimeWhenPresented(System.DateTime value)
    {
        Init();

        m_data.RateMeTimeWhenPresentedTimestamp = value.Ticks;

        Save();
    }

    public int GetTotalWins()
    {
        Init();

        return m_data.TotalWins;
    }

    public void SetTotalWins(int value)
    {
        Init();

        m_data.TotalWins = value;

        Save();
    }

    public void Load()
    {
        var filePath = GetFilePath();

        m_data = JsonLoader.LoadFromFile<PersistentData>(filePath);
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
            m_data = new PersistentData();
    }

    private static string GetFilePath()
    {
        return string.Format("{0}/persistent.json", Application.persistentDataPath);
    }
}
