using UnityEngine;
using System.Collections;

public class Palette : MonoBehaviour
{
    public PaletteStrip m_paletteStripPrefab;
    public RectTransform m_stripContainer;
    public RectTransform m_rowPivot;

    public void Awake()
    {
        int rowCount = 9;

        float stepAngle = 3.2f;
        float initAnglePos = stepAngle * (rowCount - 1) / 2.0f;
        float angle = initAnglePos;

        for (int i = 0; i < rowCount; i++)
        {
            var paletteStrip = Instantiate(m_paletteStripPrefab);
            var rectTr = paletteStrip.gameObject.GetComponent<RectTransform>();
            rectTr.parent = m_stripContainer.transform;
            rectTr.localPosition = Vector3.zero;
            rectTr.localScale = Vector3.one;
            rectTr.RotateAround(m_rowPivot.position, Vector3.back, angle);
            angle -= stepAngle;
        }
    }
}
