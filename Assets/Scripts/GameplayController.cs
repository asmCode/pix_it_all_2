using UnityEngine;
using System.Collections;

public class GameplayController
{
    private Gameplay m_gameplay;
    private Hud m_hud;
    private Board m_board;
    private BoardController m_boardInputController;

    private ImageData m_image;

    public GameplayController(
        Gameplay gameplay,
        Hud hud,
        Board board,
        BoardController boardInputController)
    {
        m_gameplay = gameplay;
        m_hud = hud;
        m_board = board;
        m_boardInputController = boardInputController;
    }

    public void SetupGameplay()
    {
        m_image = m_gameplay.Image;
        var initialColor = m_image.Colors[0];

        m_boardInputController.BoardTileTapped += HandleBoardTileTapped;

        m_hud.Init(m_image.Colors);
        m_hud.SetPaleteButtonColor(initialColor);
        m_hud.PreviewPressed += HandlePreviewPressed;
        m_hud.PreviewReleased += HandlePreviewReleased;
        m_hud.PaletteClicked += HandlePaletteClicked;

        m_board.SetSize(m_image.Texture.width, m_image.Texture.height);
        m_board.SetReferenceImage(m_image.Texture);
        m_board.HidePreview();

        m_hud.m_palette.ColorClicked += HandleColorClicked;
        m_hud.m_palette.HidePalette();
        m_hud.m_palette.SetActiveColor(initialColor);
    }

    public void Cleanup()
    {
        m_boardInputController.BoardTileTapped -= HandleBoardTileTapped;
        m_hud.PreviewPressed -= HandlePreviewPressed;
        m_hud.PreviewReleased -= HandlePreviewReleased;
        m_hud.PaletteClicked -= HandlePaletteClicked;
    }

    private void HandleBoardTileTapped(int x, int y)
    {
        m_board.Image.SetPixel(x, y, m_image.Texture.GetPixel(x, y));
        m_board.Image.Apply();
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
}
