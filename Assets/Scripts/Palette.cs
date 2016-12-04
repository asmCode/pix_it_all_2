using UnityEngine;
using System.Collections;

public class Palette : MonoBehaviour
{
    public PaletteStrip m_paletteStripPrefab;
    public RectTransform m_stripContainer;

    public void Awake()
    {
        int rowCount = 9;

        float stepX = 50.0f;
        float initXPos = stepX * (rowCount - 1) / 2.0f;
        float xPos = initXPos;

        float stepAngle = 3.2f;
        float initAnglePos = stepAngle * (rowCount - 1) / 2.0f;
        float angle = initAnglePos;

        for (int i = 0; i < rowCount; i++)
        {
            var paletteStrip = Instantiate(m_paletteStripPrefab);
            var rectTr = paletteStrip.gameObject.GetComponent<RectTransform>();
            rectTr.parent = m_stripContainer.transform;
            //rectTr.localPosition = new Vector3(xPos, -Mathf.Abs(xPos * 0.2f), 0);
            rectTr.localPosition = Vector3.zero;
            //rectTr.localRotation = Quaternion.AngleAxis(angle, Vector3.back);
            rectTr.RotateAround(new Vector3(rectTr.parent.position.x, rectTr.parent.position.y - 1000, 0), Vector3.back, angle);
            xPos -= stepX;
            angle -= stepAngle;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
