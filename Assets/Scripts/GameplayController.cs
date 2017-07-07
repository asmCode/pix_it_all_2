using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class GameplayController
{
    private Gameplay m_gameplay;
    private Hud m_hud;
    private PauseView m_pauseView;
    private SummaryView m_summaryView;
    private PenaltyView m_penaltyView;
    private Board m_board;
    private BoardController m_boardInputController;
    private BonusController m_bonusController;
    private BonusView m_bonusView;
    private LevelIntroView m_levelIntroView;
    private TutorialController m_tutorial;
    private TutorialView m_tutorialView;
    private SummaryController m_summaryController;

    private ImageData m_referenceImage;

    public Gameplay Gameplay
    {
        get { return m_gameplay; }
    }

    public Hud Hud
    {
        get { return m_hud; }
    }

    public BoardController BoardController
    {
        get { return m_boardInputController; }
    }

    public GameplayController(
        Gameplay gameplay,
        Hud hud,
        PauseView pauseView,
        SummaryView summaryView,
        PenaltyView penaltyView,
        BonusView bonusView,
        Board board,
        BoardController boardInputController,
        LevelIntroView levelIntroView,
        TutorialView tutorialView)
    {
        m_gameplay = gameplay;
        m_hud = hud;
        m_pauseView = pauseView;
        m_summaryView = summaryView;
        m_penaltyView = penaltyView;
        m_bonusView = bonusView;
        m_board = board;
        m_levelIntroView = levelIntroView;
        m_boardInputController = boardInputController;
        m_tutorialView = tutorialView;
    }

    public void SetupGameplay()
    {
        m_referenceImage = m_gameplay.ReferenceImage;
        var initialColor = m_referenceImage.Colors[0];
        var imageProgress = m_gameplay.ImageProgress;

        m_boardInputController.BoardTileTapped += HandleBoardTileTapped;

        m_hud.Init(m_referenceImage.Colors);
        m_hud.SetPaleteButtonColor(initialColor);
        m_hud.PreviewPressed += HandlePreviewPressed;
        m_hud.PreviewReleased += HandlePreviewReleased;
        m_hud.PaletteClicked += HandlePaletteClicked;
        m_hud.PauseClicked += HandlePauseClicked;
        m_hud.CheatFillColorsClicked += HandleCheatFillColorsClicked;

        m_board.PreviewEnded += HandleBoardPreviewEnded;
        m_board.SetSize(imageProgress.Width, imageProgress.Height);
        m_board.SetReferenceImage(m_referenceImage.Texture);
        m_board.SetTiles(m_gameplay.ImageProgress.GetTiles());
        m_board.HidePreview();

        m_hud.m_palette.ColorClicked += HandleColorClicked;
        m_hud.m_palette.SetActiveColor(initialColor);
        m_hud.m_tileProgress.SetMax(m_gameplay.ImageProgress.TotalTiles);
        m_hud.m_tileProgress.SetCurrent(m_gameplay.ImageProgress.RevealedTiles);

        m_hud.m_palette.PaletteShown += PaletteShown;
        m_hud.m_palette.PaletteClosed += PaletteClosed;

        m_pauseView.ResumeClicked += HandlePauseViewResumeClicked;
        m_pauseView.BackToMenuClicked += HandlePauseViewBackToMenuClicked;
        m_pauseView.OptionsClicked += HandlePauseViewOptionsClicked;
        m_pauseView.Hide();

        m_summaryView.BackToMenuClicked += HandleBackToMenuClicked;
        m_summaryView.Hide();

        m_summaryController = new SummaryController(m_board, m_summaryView, m_gameplay, m_boardInputController);

        m_bonusController = new BonusController();
        m_bonusController.Init(m_gameplay, m_bonusView, m_hud);

        m_tutorialView.Hide();
        if (m_gameplay.IsTutorialImage)
        {
            m_tutorial = new TutorialController();
            m_tutorial.Init(m_tutorialView, this);
        }           

        var imageViewData = LevelsScene.CreateImageViewData(m_referenceImage, m_gameplay.BundleId);
        m_boardInputController.PauseInput();
        m_board.SetScale(Vector2.zero, m_board.MinScale);
        m_board.SetLocalPosition(Vector2.zero);
        m_levelIntroView.Finished += HandleLevelIntroViewFinished;
        m_levelIntroView.Show(imageViewData);
    }

    public void HandleBackButton()
    {
        if (IsLevelIntroViewVisible())
        {
            // Do nothing
        }
        else if (RateMeScene.IsVisible())
        {
            var controller = RateMeScene.GetController();
            if (controller != null)
                controller.FireLater();
        }
        else if (IsSumaryActive())
        {
            HandleBackToMenuClicked();
        }
        else if (IsPauseActive())
        {
            HandlePauseViewBackToMenuClicked();
        }
        else
        {
            Pause();
        }
}   

    public void Update(float deltaTime)
    {
        if (m_tutorial != null)
            m_tutorial.Update();

        if (IsLevelIntroViewVisible())
        {
            m_hud.gameObject.SetActive(m_levelIntroView.m_hudVisible);
            if (m_levelIntroView.m_boardPreviewValue > 0.0f)
                m_board.ShowPreview();
            else
                m_board.HidePreview();
        }

        if (IsGameRunning())
        {
            // !IsLevelIntroViewVisible() means that preview is for free during the level intro.
            if (m_board.IsPreviewActive && !IsLevelIntroViewVisible())
            {
                m_gameplay.ApplyPreview(deltaTime);
                m_hud.ShowPenalty(m_gameplay.PreviewCost * m_gameplay.PreviewTime);
            }

            // Time is freezed when intro is visible
            if (!IsLevelIntroViewVisible())
                m_gameplay.AddSeconds(deltaTime);
        }

        if (IsSumaryActive())
        {
            m_summaryController.Update();
        }

        m_hud.SetTime(m_gameplay.Time);
    }

    public void Cleanup()
    {
        m_board.PreviewEnded -= HandleBoardPreviewEnded;
        m_boardInputController.BoardTileTapped -= HandleBoardTileTapped;
        m_hud.PreviewPressed -= HandlePreviewPressed;
        m_hud.PreviewReleased -= HandlePreviewReleased;
        m_hud.PaletteClicked -= HandlePaletteClicked;
        m_hud.PauseClicked -= HandlePauseClicked;
        m_pauseView.ResumeClicked -= HandlePauseViewBackToMenuClicked;
        m_pauseView.BackToMenuClicked -= HandlePauseViewResumeClicked;
        m_pauseView.OptionsClicked -= HandlePauseViewOptionsClicked;
        m_summaryView.BackToMenuClicked -= HandleBackToMenuClicked;
    }

    private bool IsLevelIntroViewVisible()
    {
        return m_levelIntroView.gameObject.activeSelf;
    }

    private void SetBoardColor(int x, int y, Color color)
    {
        // If it isn't the last pixel, reveal it with the animation. Last pixel is revealed immediately to
        // avoid collision with the steps animation.
        if (m_gameplay.ImageProgress.TotalTiles > m_gameplay.ImageProgress.RevealedTiles + 1)
            m_board.SetPixelWithAnimation(x, y, color);
        else
            m_board.SetPixel(x, y, color);

        m_board.PlayBoardSuccessEffect();

        m_gameplay.ImageProgress.RevealTile(x, y);
        m_gameplay.LevelProgress.AddStep(x, y);

        m_gameplay.NotifyTileRevealedWithSuccess();

        m_hud.m_tileProgress.SetCurrent(m_gameplay.ImageProgress.RevealedTiles);

        if (IsLevelCompleted())
            FinishLevel();
    }

    private void FinishLevel()
    {
        var persistent = Pix.Game.GetInstance().Persistent;
        persistent.SetTotalWins(persistent.GetTotalWins() + 1);

        SendProgressionFinishedEvent(m_gameplay.Time);

        ShowSummary();

        m_gameplay.Complete();
    }

    private void SaveProgress()
    {
        if (m_gameplay.ImageProgress.RevealedTiles > 0 &&
            m_gameplay.ImageProgress.RevealedTiles < m_gameplay.ImageProgress.TotalTiles &&
            !m_gameplay.IsTutorialImage)
            m_gameplay.SaveProgress(m_board.Image);
    }

    private void HandleBoardPreviewEnded()
    {
        m_gameplay.NotifyPreviewEnded();
    }

    public void HandleBoardTileTapped(int x, int y)
    {
        if (IsTileFilled(x, y))
            return;

        var activeColor = m_hud.m_palette.ActiveColor;
        var requiredColor = m_gameplay.GetReferenceColor(x, y);

        if (activeColor == requiredColor)
        {
            SetBoardColor(x, y, activeColor);

            // Don't play pixel sound if it was the last pixel. We want to avoid collision with the Victory sound.
            if (!IsLevelCompleted())
                AudioManager.GetInstance().PlayPixelSound();
        }
        else
        {
            AudioManager.GetInstance().SoundFail.Play();
            m_board.PlayBoardFailEffect();
            ApplyPenalty();
        }
    }

    private void ApplyPenalty()
    {
        int penaltySeconds = m_gameplay.ApplyPenalty();
        // Skip that for now
        // m_penaltyView.ShowPenalty(penaltySeconds);
        m_hud.ShowPenalty(penaltySeconds);

        m_gameplay.NotifyTileRevealedWithFailure();
    }

    private void ShowSummary()
    {
        m_boardInputController.PauseInput();
        int tilesCount = m_gameplay.ImageProgress.Width * m_gameplay.ImageProgress.Height;
        
        int colorsCount = m_referenceImage.Colors.Length;
        float time = m_gameplay.Time;
        bool record =
            !m_gameplay.LevelProgress.IsCompleted ||
            m_gameplay.LevelProgress.BestTime > time;
        float timeFor3Stars = StarRatingCalc.RequiredTimeForStars(3, tilesCount, colorsCount);
        float timeFor2Stars = StarRatingCalc.RequiredTimeForStars(2, tilesCount, colorsCount);
        int starsCount = StarRatingCalc.GetStars(time, tilesCount, colorsCount);

        var imageViewData = LevelsScene.CreateImageViewData(m_referenceImage, m_gameplay.BundleId);
        if (imageViewData == null)
            return;

        m_hud.gameObject.SetActive(false);
        // m_summaryView.Show(imageViewData.ImageData.Name, starsCount, time, record, imageViewData.LevelProgress.BestTime, timeFor3Stars, timeFor2Stars);
        m_summaryController.Show(
            imageViewData.ImageData.Name, 
            starsCount,
            time,
            record,
            imageViewData.LevelProgress.BestTime,
            timeFor3Stars,
            timeFor2Stars,
            m_gameplay.LevelProgress.GetSteps());

        AudioManager.GetInstance().SoundVictory.Play();
    }

    private void Pause()
    {
        bool is_save_available =
            m_gameplay.ImageProgress.RevealedTiles > 0 &&
            !m_gameplay.IsTutorialImage;

        m_pauseView.Show(is_save_available);
    }

    private void Resume()
    {
        m_pauseView.gameObject.SetActive(false);
    }

    private bool IsTileFilled(int x, int y)
    {
        return m_gameplay.ImageProgress.IsRevealed(x, y);
    }

    private bool IsLevelCompleted()
    {
        return m_gameplay.ImageProgress.IsCompleted;
    }

    private void CheatSetAllButOnePixels()
    {
        bool skipped = false;

        var imgPrg = m_gameplay.ImageProgress;

        for (int y = 0; y < imgPrg.Height; y++)
        {
            for (int x = 0; x < imgPrg.Width; x++)
            {
                if (!imgPrg.IsRevealed(x, y))
                {
                    if (skipped)
                    {
                        var requiredColor = m_gameplay.GetReferenceColor(x, y);

                        SetBoardColor(x, y, requiredColor);
                    }
                    else
                        skipped = true;
                }
            }
        }
    }

    private bool IsGameRunning()
    {
        return !IsPauseActive() && !IsSumaryActive() && !IsLevelIntroViewVisible();
    }

    private bool IsPauseActive()
    {
        return m_pauseView.IsActive;
    }

    private bool IsSumaryActive()
    {
        return m_summaryController.IsActive;
    }

    public void HandlePreviewPressed()
    {
        AudioManager.GetInstance().SoundPreview.Play();
        m_board.ShowPreview();
        m_gameplay.NotifyPreviewStarted();
    }

    public void HandlePreviewReleased()
    {
        AudioManager.GetInstance().SoundPreview.Stop();
        m_board.HidePreview();
    }

    private void HandlePaletteClicked()
    {
        TogglePalette();
    }

    public void TogglePalette()
    {
        if (m_hud.m_palette.IsPaletteVisible)
            HidePalette();
        else
            ShowPalette();
    }

    public void ShowPalette()
    {
        m_hud.m_palette.ShowPalette();
    }

    public void HidePalette()
    {
        m_hud.m_palette.HidePalette();
    }

    private void HandleColorClicked(Color color)
    {
        m_hud.m_palette.SetActiveColor(color);
        m_hud.m_palette.HidePalette();
        m_hud.SetPaleteButtonColor(color);
    }

    public void SetColor(int index)
    {
        var color = m_hud.m_palette.GetColor(index);
        m_hud.m_palette.SetActiveColor(color);
        m_hud.SetPaleteButtonColor(color);
    }

    private void HandlePauseClicked()
    {
        Pause();
    }

    private void HandleCheatFillColorsClicked()
    {
        CheatSetAllButOnePixels();
    }

    private void HandlePauseViewBackToMenuClicked()
    {
        GameAnalyticsSDK.GameAnalytics.NewDesignEvent("button.gameplay.pause.back");

        SaveProgress();

        ShowLevelsSceneWithFade(m_gameplay.BundleId, m_gameplay.ImageId, null);
    }

    private void HandlePauseViewOptionsClicked()
    {
        OptionsScene.Show(true, () =>
        {
            Fade.FadeOut(null, false, null);
        });
    }

    private void HandlePauseViewResumeClicked()
    {
        Resume();
    }

    public void NotifyOnApplicationPause(bool paused)
    {
        if (paused && !IsSumaryActive())
        {
            SaveProgress();
            Pause();
        }
    }

    private void GoToLevels()
    {
        if (RateMeController.ShouldShowRateMe())
        {
            // TODO: after closing popup, go to Levels automatically
            RateMeScene.Show();
            return;
        }

        ShowLevelsSceneWithFade(m_gameplay.BundleId, m_gameplay.ImageId, m_gameplay.ImageId);
    }

    private void ShowLevelsSceneWithFade(string bundleId, string imageIdToCenter, string imageIdToComplete)
    {
        Fade.FadeIn(null, true, () =>
        {
            Pix.Game.GetInstance().ShowLevelsScene(bundleId, imageIdToCenter, imageIdToComplete);
        }); 
    }

    private void HandleBackToMenuClicked()
    {
        GoToLevels();
    }

    private void PaletteShown()
    {
    }

    private void PaletteClosed()
    {
    }

    private bool IsInProgress()
    {
        var imageViewData = LevelsScene.CreateImageViewData(m_referenceImage, m_gameplay.BundleId);
        if (imageViewData == null || imageViewData.LevelProgress == null)
            return false;

        return imageViewData.LevelProgress.IsInProgress;
    }

    private void HandleLevelIntroViewFinished()
    {
        m_levelIntroView.Close();
        m_hud.gameObject.SetActive(true);
        m_boardInputController.ResumeInput();

        if (m_gameplay.IsTutorialImage)
            m_tutorialView.Show();

        if (!IsInProgress())
            SendProgressionStartEvent();
        else
            GameAnalyticsSDK.GameAnalytics.NewDesignEvent("gameplay.intro.continue");
    }

    private void SendProgressionStartEvent()
    {
        GameAnalyticsSDK.GameAnalytics.NewProgressionEvent(
            GameAnalyticsSDK.GAProgressionStatus.Start,
            m_gameplay.BundleId,
            m_gameplay.ImageId);
    }

    private void SendProgressionFinishedEvent(float time)
    {
        GameAnalyticsSDK.GameAnalytics.NewProgressionEvent(
            GameAnalyticsSDK.GAProgressionStatus.Complete,
            m_gameplay.BundleId,
            m_gameplay.ImageId,
            (int)(time));
    }
}
