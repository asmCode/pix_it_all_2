using UnityEngine;
using System.Collections;

public class Palette : MonoBehaviour
{
    public PaletteStrip m_paletteStripPrefab;
    public RectTransform m_stripContainer;
    public RectTransform m_rowPivot;

    public event System.Action<Color> ColorClicked;

    private const int MaxStripsInRow = 9;

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
        const float yOffsetStep = 100.0f;
        const float stepAngle = 3.2f;
        const float initAngle = stepAngle * (MaxStripsInRow - 1) / 2.0f;

        int stripNum = 0;
        float angle = initAngle;
        float yOffset = 0.0f;

        foreach (var color in colors)
        {
            CreatePaletteStrip(angle, yOffset, color);
            angle -= stepAngle;
            stripNum++;

            if (stripNum == MaxStripsInRow)
            {
                stripNum = 0;
                angle = initAngle;
                yOffset += yOffsetStep;
            }
        }
    }

    private PaletteStrip CreatePaletteStrip(float angle, float yOffset, Color color)
    {
        var paletteStrip = Instantiate(m_paletteStripPrefab);
        paletteStrip.Color = color;
        paletteStrip.Clicked += HandlePaletteStripClicked;

        var paletteStripRectTr = paletteStrip.gameObject.GetComponent<RectTransform>();
        paletteStripRectTr.parent = m_stripContainer.transform;
        paletteStripRectTr.localPosition = Vector3.zero;
        paletteStripRectTr.localScale = Vector3.one;
        paletteStripRectTr.RotateAround(m_rowPivot.position, Vector3.forward, angle);
        paletteStripRectTr.localPosition = Vu.AddY(paletteStripRectTr.localPosition, yOffset);
        paletteStripRectTr.SetAsFirstSibling();

        return paletteStrip;
    }

    public void ShowPalette()
    {
        gameObject.SetActive(true);
    }

    public void HidePalette()
    {
        gameObject.SetActive(false);
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
