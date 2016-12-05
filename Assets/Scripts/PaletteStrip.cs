using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PaletteStrip : MonoBehaviour
{
    public Image m_image;
    
    public System.Action<PaletteStrip> Clicked;

    private bool m_isInitialized;
    private float m_baseYOffset;

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

    public void MarkSelected(bool selected)
    {
        Init();

        float yOffset = selected ? 50.0f : 0.0f;
        m_image.rectTransform.localPosition = Vu.SetY(m_image.rectTransform.localPosition, m_baseYOffset + yOffset);
    }

    public void UiEventClicked()
    {
        if (Clicked != null)
            Clicked(this);
    }

    private void Init()
    {
        if (m_isInitialized)
            return;

        m_isInitialized = true;
        m_baseYOffset = m_image.transform.localPosition.y;
    }

    private void Start()
    {
        Init();
    }
}
