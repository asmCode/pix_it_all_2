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

    private ImageData m_referenceImage;

    public GameplayController(
        Gameplay gameplay,
        Hud hud,
        PauseView pauseView,
        SummaryView summaryView,
        PenaltyView penaltyView,
        BonusView bonusView,
        Board board,
        BoardController boardInputController)
    {
        m_gameplay = gameplay;
        m_hud = hud;
        m_pauseView = pauseView;
        m_summaryView = summaryView;
        m_penaltyView = penaltyView;
        m_bonusView = bonusView;
        m_board = board;
        m_boardInputController = boardInputController;
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

        m_board.SetSize(imageProgress.Width, imageProgress.Height);
        m_board.SetReferenceImage(m_referenceImage.Texture);
        m_board.SetTiles(m_gameplay.ImageProgress.GetTiles());
        m_board.HidePreview();

        m_hud.m_palette.ColorClicked += HandleColorClicked;
        m_hud.m_palette.HidePalette();
        m_hud.m_palette.SetActiveColor(initialColor);

        m_hud.m_palette.PaletteShown += PaletteShown;
        m_hud.m_palette.PaletteClosed += PaletteClosed;

        m_pauseView.ResumeClicked += HandlePauseViewResumeClicked;
        m_pauseView.BackToMenuClicked += HandlePauseViewBackToMenuClicked;
        m_pauseView.Hide();

        m_summaryView.BackToMenuClicked += HandleBackToMenuClicked;
        m_summaryView.Hide();

        m_bonusController = new BonusController();
        m_bonusController.Init(m_gameplay, m_bonusView);
    }

    public void Update(float deltaTime)
    {
        if (IsGameRunning())
        {
            m_gameplay.AddSeconds(deltaTime);

            if (m_board.IsPreviewActive)
                m_gameplay.ApplyPreview(deltaTime);
        }

        m_hud.SetTime(m_gameplay.Time);
    }

    public void Cleanup()
    {
        m_boardInputController.BoardTileTapped -= HandleBoardTileTapped;
        m_hud.PreviewPressed -= HandlePreviewPressed;
        m_hud.PreviewReleased -= HandlePreviewReleased;
        m_hud.PaletteClicked -= HandlePaletteClicked;
        m_hud.PauseClicked -= HandlePauseClicked;
        m_pauseView.ResumeClicked -= HandlePauseViewBackToMenuClicked;
        m_pauseView.BackToMenuClicked -= HandlePauseViewResumeClicked;
    }

    private void SetBoardColor(int x, int y, Color color)
    {
        m_board.Image.SetPixel(x, y, color);
        m_board.Image.Apply();

        m_gameplay.ImageProgress.RevealTile(x, y);

        m_gameplay.NotifyTileRevealedWithSuccess();

        if (IsLevelCompleted())
            FinishLevel();
    }

    private void FinishLevel()
    {
        ShowSummary();

        m_gameplay.Complete();
    }

    private void SaveProgress()
    {
        m_gameplay.SaveProgress(m_board.Image);
    }

    private void HandleBoardTileTapped(int x, int y)
    {
        if (IsTileFilled(x, y))
            return;

        var activeColor = m_hud.m_palette.ActiveColor;
        var requiredColor = m_gameplay.GetReferenceColor(x, y);

        if (activeColor == requiredColor)
        {
            SetBoardColor(x, y, activeColor);
        }
        else
        {
            ApplyPenalty();
        }
    }

    private void ApplyPenalty()
    {
        int penaltySeconds = m_gameplay.ApplyPenalty();
        m_penaltyView.ShowPenalty(penaltySeconds);
        
        m_gameplay.NotifyTileRevealedWithSuccess();
    }

    private void ShowSummary()
    {
        int tilesCount = m_gameplay.ImageProgress.Width * m_gameplay.ImageProgress.Height;
        int colorsCount = m_referenceImage.Colors.Length;
        float time = m_gameplay.Time;
        bool record =
            m_gameplay.LevelProgress.IsCompleted &&
            m_gameplay.LevelProgress.BestTime > time;
        float timeFor3Stars = StarRatingCalc.RequiredTimeForStars(3, tilesCount, colorsCount);
        float timeFor2Stars = StarRatingCalc.RequiredTimeForStars(2, tilesCount, colorsCount);
        int starsCount = StarRatingCalc.GetStars(time, tilesCount, colorsCount);

        m_hud.gameObject.SetActive(false);
        m_summaryView.Show(starsCount, time, record, timeFor3Stars, timeFor2Stars);
    }

    private void Pause()
    {
        bool is_save_available = m_gameplay.ImageProgress.RevealedTiles > 0;

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
        return !IsPauseActive() && !IsSumaryActive();
    }

    private bool IsPauseActive()
    {
        return m_pauseView.IsActive;
    }

    private bool IsSumaryActive()
    {
        return m_summaryView.gameObject.activeSelf;
    }

    private void HandlePreviewPressed()
    {
        m_board.ShowPreview();
    }

    private void HandlePreviewReleased()
    {
        m_board.HidePreview();
    }

    private void HandlePaletteClicked()
    {
        if (m_hud.m_palette.IsPaletteVisible)
            m_hud.m_palette.HidePalette();
        else
            m_hud.m_palette.ShowPalette();
    }

    private void HandleColorClicked(Color color)
    {
        m_hud.m_palette.SetActiveColor(color);
        m_hud.m_palette.HidePalette();
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
        if (m_gameplay.ImageProgress.RevealedTiles > 0)
            SaveProgress();

        SceneManager.LoadScene("Levels");   
    }

    private void HandlePauseViewResumeClicked()
    {
        Resume();
    }

    private void HandleBackToMenuClicked()
    {
        SceneManager.LoadScene("Levels");
    }

    private void PaletteShown()
    {
    }

    private void PaletteClosed()
    {
    }
}
