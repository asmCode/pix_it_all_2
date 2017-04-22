﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Game : MonoBehaviourSingleton<Game, MonoBehaviourSingletonMeta>
{
    public ImageManager ImageManager
    {
        get;
        private set;
    }

    public PlayerProgress PlayerProgress
    {
        get;
        private set;
    }

    public Purchaser Purchaser
    {
        get;
        private set;
    }

    public Options Options
    {
        get;
        private set;
    }

    public void StartLevel(string bundleId, string imageId, bool continueLevel)
    {
        GameplayScene.m_selectedBundleId = bundleId;
        GameplayScene.m_selectedLevelId = imageId;
        GameplayScene.m_continueLevel = continueLevel;

        SceneManager.LoadScene("Gameplay");
    }

    protected override void Awake()
    {
        Application.targetFrameRate = 60;
        TouchProxy.Init();

        Options = new Options();
        Options.Load();

        ImageManager = new ImageManager();
        ImageManager.Init();
        ImageManager.LoadImages();

        PlayerProgress = new PlayerProgress();

        Purchaser = new Purchaser();
        Purchaser.PurchaseFinished += Purchaser_PurchaseFinished;
        Purchaser.InitializePurchasing();
    }

    private void Purchaser_PurchaseFinished(bool success, string productId)
    {
        if (!success)
            return;

        ImageManager.SetBundleAvailable(productId);
    }

    private void Update()
    {
        TouchProxy.Update();
	}
}
