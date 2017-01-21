using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress
{
    private Dictionary<string, LevelProgress> m_levelProgressesMap = new Dictionary<string, LevelProgress>();

    public LevelProgress GetLevelProgress(string bundleId, string imageId)
    {
        var id = MakeId(bundleId, imageId);

        LevelProgress levelProgress = null;
        if (!m_levelProgressesMap.TryGetValue(id, out levelProgress))
        {
            levelProgress = LevelProgress.LoadOrCreate(bundleId, imageId);

            if (levelProgress != null)
                m_levelProgressesMap.Add(id, levelProgress);
        }

        return levelProgress;
    }

    private string MakeId(string bundleId, string imageId)
    {
        return string.Format("{0}-{1}", bundleId, imageId);
    }
}
