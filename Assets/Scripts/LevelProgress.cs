using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class LevelProgress
{
    private const int Version = 1;

    private LevelProgressData m_data;

    public string BundleId
    {
        get;
        private set;
    }

    public string ImageId
    {
        get;
        private set;
    }

    public bool IsCompleted
    {
        get { return m_data.BestTime != 0; }
    }

    public int BestTime
    {
        get { return m_data.BestTime; }
    }

    public bool IsInProgress
    {
        get { return !string.IsNullOrEmpty(m_data.ContinueImageData); }
    }

    public int ContinueTime
    {
        get { return m_data.ContinueTime; }
    }

    public static LevelProgress LoadOrCreate(string bundleId, string imageId)
    {
        if (string.IsNullOrEmpty(bundleId) ||
            string.IsNullOrEmpty(imageId))
            return null;

        var path = GetFilePath(bundleId, imageId);

        var data = JsonLoader.LoadFromFile<LevelProgressData>(path);
        if (data == null)
            data = new LevelProgressData();

        return Create(bundleId, imageId, data);
    }

    public static LevelProgress Create(string bundleId, string imageId, LevelProgressData data)
    {
        if (string.IsNullOrEmpty(bundleId) ||
            string.IsNullOrEmpty(imageId))
            return null;

        var levelProgress = new LevelProgress();
        levelProgress.BundleId = bundleId;
        levelProgress.ImageId = imageId;
        levelProgress.m_data = data;

        return levelProgress;
    }

    public void Save()
    {
        var path = GetFilePath(BundleId, ImageId);

        m_data.Version = Version;

        bool success = JsonLoader.SaveToFile(path, m_data);
        if (!success)
            Debug.LogFormat("Couldn't save progress for level: {0}", path);
    }

    public bool[] GetContinueImageData()
    {
        return null;
    }

    public void SaveProgress(int time, string imageMaskData)
    {
        m_data.ContinueTime = time;
        m_data.ContinueImageData = imageMaskData;
    }

    public void Complete(int time)
    {
        m_data.ContinueTime = 0;
        m_data.ContinueImageData = null;

        if (m_data.BestTime == 0 || m_data.BestTime > time)
            m_data.BestTime = time;
    }

    private LevelProgress()
    {

    }

    private static string GetFilePath(string bundleId, string imageId)
    {
        return string.Format("{0}/progress/{1}-{2}.progress",
            Application.persistentDataPath,
            bundleId,
            imageId);
    }
}
