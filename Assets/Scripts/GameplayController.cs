using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameplayController
{
    private Gameplay m_gameplay;
    private Hud m_hud;
    private PauseView m_pauseView;
    private SummaryView m_summaryView;
    private Board m_board;
    private BoardController m_boardInputController;

    private ImageData m_image;
    private int m_tilesLeft;

    public GameplayController(
        Gameplay gameplay,
        Hud hud,
        PauseView pauseView,
        SummaryView summaryView,
        Board board,
        BoardController boardInputController)
    {
        m_gameplay = gameplay;
        m_hud = hud;
        m_pauseView = pauseView;
        m_summaryView = summaryView;
        m_board = board;
        m_boardInputController = boardInputController;
    }

    public void SetupGameplay()
    {
        m_image = m_gameplay.Image;
        var initialColor = m_image.Colors[0];
        m_tilesLeft = m_image.Texture.width * m_image.Texture.height;

        m_boardInputController.BoardTileTapped += HandleBoardTileTapped;

        m_hud.Init(m_image.Colors);
        m_hud.SetPaleteButtonColor(initialColor);
        m_hud.PreviewPressed += HandlePreviewPressed;
        m_hud.PreviewReleased += HandlePreviewReleased;
        m_hud.PaletteClicked += HandlePaletteClicked;
        m_hud.PauseClicked += HandlePauseClicked;
        m_hud.CheatFillColorsClicked += HandleCheatFillColorsClicked;

        m_board.SetSize(m_image.Texture.width, m_image.Texture.height);
        m_board.SetReferenceImage(m_image.Texture);
        m_board.HidePreview();

        m_hud.m_palette.ColorClicked += HandleColorClicked;
        m_hud.m_palette.HidePalette();
        m_hud.m_palette.SetActiveColor(initialColor);

        m_pauseView.ResumeClicked += HandlePauseViewBackToMenuClicked;
        m_pauseView.BackToMenuClicked += HandlePauseViewResumeClicked;
        m_pauseView.gameObject.SetActive(false);

        m_summaryView.NextLevelClicked += HandleNextLevelClicked;
        m_summaryView.RetryClicked += HandleRetryClicked;
        m_summaryView.BackToMenuClicked += HandleBackToMenuClicked;
        m_summaryView.Hide();
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

        m_tilesLeft--;

        if (IsLevelCompleted())
            FinishLevel();
    }

    private void FinishLevel()
    {
        m_gameplay.Complete(1000 * 120 + 47 + 665);

        ShowSummary();
    }

    private void HandleBoardTileTapped(int x, int y)
    {
        if (IsTileFilled(x, y))
            return;

        var activeColor = m_hud.m_palette.ActiveColor;
        var requiredColor = m_image.Texture.GetPixel(x, y);

        if (activeColor == requiredColor)
        {
            SetBoardColor(x, y, activeColor);
        }
    }

    private void ShowSummary()
    {
        m_hud.gameObject.SetActive(false);
        m_summaryView.Show(3, 10, false);
    }

    private void Pause()
    {
        m_pauseView.gameObject.SetActive(true);
    }

    private void Resume()
    {
        m_pauseView.gameObject.SetActive(false);
    }

    private bool IsTileFilled(int x, int y)
    {
        return m_board.Image.GetPixel(x, y).a != 0.0f;
    }

    private bool IsLevelCompleted()
    {
        return m_tilesLeft == 0;
    }

    private void CheatSetAllButOnePixels()
    {
        bool skipped = false;

        for (int y = 0; y < m_image.Texture.height; y++)
        {
            for (int x = 0; x < m_image.Texture.width; x++)
            {
                var boardPixel = m_board.Image.GetPixel(x, y);

                if (boardPixel.a == 0.0f && skipped)
                {
                    var requiredColor = m_image.Texture.GetPixel(x, y);

                    SetBoardColor(x, y, requiredColor);
                }
                else
                    skipped = true;
            }
        }
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
        Resume();
    }

    private void HandlePauseViewResumeClicked()
    {
        SceneManager.LoadScene("Levels");
    }

    private void HandleNextLevelClicked()
    {
    }

    private void HandleRetryClicked()
    {
    }

    private void HandleBackToMenuClicked()
    {
    }
}
