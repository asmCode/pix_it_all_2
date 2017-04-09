using UnityEngine;
using System.Collections;

public class Palette : MonoBehaviour
{
    public PaletteStrip m_paletteStripPrefab;
    public RectTransform m_stripContainer;

    public event System.Action<Color> ColorClicked;
    public event System.Action PaletteShown;
    public event System.Action PaletteClosed;

    private const int MaxStripsInColumn = 6;

    public bool IsPaletteVisible
    {
        get { return gameObject.activeSelf; }
    }

    public Color ActiveColor
    {
        get;
        private set;
    }

    public void Init(Color[] colors)
    {
        const float padding = 110.0f;

        int stripNum = 0;
        Vector2 offset = Vector2.zero;

        foreach (var color in colors)
        {
            CreatePaletteStrip(offset, color);
            stripNum++;

            offset.y += padding;

            if (stripNum > MaxStripsInColumn)
            {
                stripNum = 0;
                offset.x -= padding;
                offset.y = 0;
            }
        }
    }

    private PaletteStrip CreatePaletteStrip(Vector2 offset, Color color)
    {
        var paletteStrip = Instantiate(m_paletteStripPrefab);
        paletteStrip.Color = color;
        paletteStrip.Clicked += HandlePaletteStripClicked;

        var paletteStripRectTr = paletteStrip.gameObject.GetComponent<RectTransform>();
        paletteStripRectTr.SetParent(m_stripContainer.transform);
        paletteStripRectTr.localPosition = offset;
        paletteStripRectTr.localScale = Vector3.one;
        paletteStripRectTr.SetAsFirstSibling();

        return paletteStrip;
    }

    public void ShowPalette()
    {
        gameObject.SetActive(true);

        if (PaletteShown != null)
            PaletteShown();
    }

    public void HidePalette()
    {
        gameObject.SetActive(false);

        if (PaletteClosed != null)
            PaletteClosed();
    }

    public void SetActiveColor(Color color)
    {
        ActiveColor = color;

        MarkActiveStrip(ActiveColor);
    }

    private void MarkActiveStrip(Color color)
    {
        for (int i = 0; i < m_stripContainer.childCount; i++)
        {
            var child = m_stripContainer.GetChild(i).GetComponent<PaletteStrip>();
            child.MarkSelected(child.Color == color);
        }
    }

    private void HandlePaletteStripClicked(PaletteStrip paletteStrip)
    {
        if (ColorClicked != null)
            ColorClicked(paletteStrip.Color);
    }
}
