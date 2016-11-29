using UnityEngine;
using System.Collections;

public class Gameplay : MonoBehaviour
{
    public Board m_board;

    private ImageData m_image;

    private void Start()
    {
        m_image = ImageData.Load("motorowka");

        m_board.SetSize(m_image.Texture.width, m_image.Texture.height);
        m_board.SetImage(m_image.Texture);
    }
}
