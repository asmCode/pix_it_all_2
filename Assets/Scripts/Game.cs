﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Pix
{
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

        public Persistent Persistent
        {
            get;
            private set;
        }

        public void Init()
        {
            Ssg.Social.Social.GetInstance().Authenticated += HandleSocialAuthenticated;
        }

        public void StartLevel(string bundleId, string imageId, bool continueLevel)
        {
            GameplayScene.m_selectedBundleId = bundleId;
            GameplayScene.m_selectedLevelId = imageId;
            GameplayScene.m_continueLevel = continueLevel;

            SceneManager.LoadScene("Gameplay");
        }

        public void ShowBundlesScene()
        {
            LevelsScene.m_bundleIdToSelect = null;
            SceneManager.LoadScene("Levels");
        }

        public void ShowLevelsScene(string bundleId, string imageIdToCenter, string imageIdToComplete)
        {
            LevelsScene.m_bundleIdToSelect = bundleId;
            LevelsScene.m_imageIdToCenter = imageIdToCenter;
            LevelsScene.m_imageIdToComplete = imageIdToComplete;
            SceneManager.LoadScene("Levels");
        }

        protected override void Awake()
        {
            Application.targetFrameRate = 60;
            TouchProxy.Init();

            Options = new Options();
            Options.Load();

            Persistent = new Persistent();
            Persistent.Load();

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

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.S))
            {
                Application.CaptureScreenshot(string.Format("pix_it_all_{0}.png", System.DateTime.Now.Ticks));
            }
#endif
        }

        public void ReportScores()
        {
            var social =  Ssg.Social.Social.GetInstance();

            if (!social.IsAuthenticated)
                return;

            social.ReportLocalUserScore(SocialIds.LeaderboardTotalPixels, Persistent.GetTotalPixelsRevealed(), null);
        }

        private void HandleSocialAuthenticated(bool success)
        {
            ReportScores();
        }
    }
}