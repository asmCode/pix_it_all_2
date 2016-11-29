using UnityEngine;
using System.Collections;

public class Gameplay : MonoBehaviour
{
    public Board m_board;
    public BoardController m_boardInputController;

    private ImageData m_image;

    private void Start()
    {
        m_image = ImageData.Load("motorowka");

        m_board.SetSize(m_image.Texture.width, m_image.Texture.height);
        m_board.SetReferenceImage(m_image.Texture);
        m_board.HidePreview();
    }

    private void OnEnable()
    {
        m_boardInputController.BoardTileTapped += HandleBoardTileTapped;
    }

    private void OnDisable()
    {
        m_boardInputController.BoardTileTapped -= HandleBoardTileTapped;
    }

    private void HandleBoardTileTapped(int x, int y)
    {
        Debug.LogFormat("tap: {0}, {1}", x, y);
    }
}
