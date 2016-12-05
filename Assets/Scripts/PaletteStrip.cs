using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PaletteStrip : MonoBehaviour
{
    public Image m_image;

    public Color Color
    {
        get
        {
            return m_image.color;
        }

        set
        {
            m_image.color = value;
        }
    }
}
