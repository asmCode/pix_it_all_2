﻿using UnityEngine;
using System.Collections;
using System;

public class Gameplay
{
    public event System.Action TileRevealedWithSuccess;

    private static readonly int PenaltyTime = 10;
    private static readonly float PreviewTimeCostPerSecond = 7.0f;

    private int m_successInRow;

    public int SuccessInRow
    {
        get { return m_successInRow; }
    }

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

    public ImageData ReferenceImage
    {
        get;
        private set;
    }

    public ImageProgress ImageProgress
    {
        get;
        private set;
    }

    public LevelProgress LevelProgress
    {
        get;
        private set;
    }

    public float Time
    {
        get;
        private set;
    }

    public void Init(string bundleId, string imageId, bool continueLevel)
    {
        BundleId = bundleId;
        ImageId = imageId;

        ReferenceImage = Pix.Game.GetInstance().ImageManager.GetImageById(bundleId, imageId);

        ImageProgress = new ImageProgress();
        ImageProgress.Init(ReferenceImage.Texture.width, ReferenceImage.Texture.height);

        InitLevelProgress();

        if (continueLevel)
            InitContinue();
    }

    public void AddSeconds(float deltaTime)
    {
        Time += deltaTime;

        if (Time < 0.0f)
            Time = 0.0f;
    }

    public Color GetReferenceColor(int x, int y)
    {
        return ReferenceImage.Texture.GetPixel(x, y);
    }

    public void Complete()
    {
        if (LevelProgress == null)
            return;

        LevelProgress.Complete(Time);
        LevelProgress.Save();

        Pix.Game.GetInstance().ReportScores();
    }

    public void SaveProgress(Texture2D image)
    {
        if (LevelProgress == null)
            return;

        var tiles = ImageProgress.GetTiles();
        if (tiles == null)
            return;

        LevelProgress.SaveProgress(Time, tiles);
        LevelProgress.Save();

        Pix.Game.GetInstance().ReportScores();
    }

    private void InitLevelProgress()
    {
        LevelProgress = null;

        var playerProgress = Pix.Game.GetInstance().PlayerProgress;

        LevelProgress = playerProgress.GetLevelProgress(BundleId, ImageId);
        if (LevelProgress == null)
            return;
    }

    private void InitContinue()
    {
        if (!LevelProgress.IsInProgress)
            return;

        var tiles = LevelProgress.GetContinueImageData();
        if (tiles == null)
            return;

        ImageProgress.SetTiles(tiles);

        Time = LevelProgress.ContinueTime;
    }

    public int ApplyPenalty()
    {
        AddSeconds(PenaltyTime);

        return PenaltyTime;
    }

    public void ApplyBonus(float seconds)
    {
        AddSeconds(seconds);
    }

    public void ApplyPreview(float deltaTime)
    {
        Time += PreviewTimeCostPerSecond * deltaTime;
    }

    public void NotifyTileRevealedWithSuccess()
    {
        m_successInRow++;

        Pix.Game.GetInstance().Persistent.AddToTotalPixelsRevealed(1);

        if (TileRevealedWithSuccess != null)
            TileRevealedWithSuccess();
    }

    public void NotifyTileRevealedWithFailure()
    {
        m_successInRow = 0;
    }
}
