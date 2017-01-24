using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public RawImage m_referenceImage;
    public RawImage m_image;
    public RectTransform m_parentRectTransform;
    public RectTransform m_scalePivot;

    private RectTransform m_rectTransform;
    private float m_scaleMin;
    private float m_scaleMax;
    private float m_zoom = 1.0f;

    private bool m_isInitialized;

    public Texture2D Image
    {
        get;
        private set;
    }

    public RectTransform RectTransform
    {
        get { return m_rectTransform; }
    }

    public void SetSize(int width, int height)
    {
        m_rectTransform.sizeDelta = new Vector2(width, height);
        InitScaleBounds();
        RecreateImage();
    }

    public void ShowPreview()
    {
        m_referenceImage.gameObject.SetActive(true);
    }

    public void HidePreview()
    {
        m_referenceImage.gameObject.SetActive(false);
    }

    public void SetReferenceImage(Texture2D texture)
    {
        m_referenceImage.texture = texture;
    }

    public void SetTiles(bool[] tiles)
    {
        if (tiles == null ||
            tiles.Length != (Image.width * Image.height))
            return;

        for (int i = 0; i < tiles.Length; i++)
        {
            int x = i % Image.width;
            int y = i / Image.width;

            var color = Color.black;
            color.a = 0.0f;

            if (tiles[i])
                color = ((Texture2D)m_referenceImage.texture).GetPixel(x, y);

            Image.SetPixel(x, y, color);
        }

        Image.Apply();
    }

    public void SetZoom(Vector2 pivot, float zoom)
    {
        m_zoom = Mathf.Clamp01(zoom);
        float scale = GetScale(m_zoom);

        var boardPosition = m_rectTransform.position;
        var scalePivotPosition = m_scalePivot.position;

        m_scalePivot.position = pivot;
        m_rectTransform.position = boardPosition;

        m_scalePivot.localScale = new Vector3(scale, scale, 1.0f);

        boardPosition = m_rectTransform.position;
        m_scalePivot.position = scalePivotPosition;
        m_rectTransform.position = boardPosition;

        FixPositionInsideParent();
    }

    public void SetLocalPosition(Vector2 position)
    {
        m_rectTransform.localPosition = position;
        FixPositionInsideParent();
    }

    public void ChangeLocalPosition(Vector2 delta)
    {
        ChangeLocalPositionInternal(delta);
        FixPositionInsideParent();
    }

    public void ChangeZoom(Vector2 pivot, float delta)
    {
        float newZoom = m_zoom + delta / 500.0f;
        SetZoom(pivot, newZoom);
    }

    public void FixPositionInsideParent()
    {
        var parentRect = GetParentRect();
        var imageRect = GetImageRect();

        // Fix horizontally
        if (imageRect.Width <= parentRect.Width)
        {
            m_rectTransform.localPosition = new Vector2(0, m_rectTransform.localPosition.y);
        }
        else
        {
            if (imageRect.Right < parentRect.Right)
                ChangeLocalPositionInternal(new Vector2(parentRect.Right - imageRect.Right, 0));

            if (imageRect.Left > parentRect.Left)
                ChangeLocalPositionInternal(new Vector2(parentRect.Left - imageRect.Left, 0));
        }

        // Fix vertically
        if (imageRect.Height <= parentRect.Height)
        {
            m_rectTransform.localPosition = new Vector2(m_rectTransform.localPosition.x, 0);
        }
        else
        {
            if (imageRect.Top < parentRect.Top)
                ChangeLocalPositionInternal(new Vector2(0, parentRect.Top - imageRect.Top));

            if (imageRect.Bottom > parentRect.Bottom)
                ChangeLocalPositionInternal(new Vector2(0, parentRect.Bottom - imageRect.Bottom));
        }
    }

    private float GetScale(float zoom)
    {
        return Mathf.Lerp(m_scaleMin, m_scaleMax, zoom);
    }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (m_isInitialized)
            return;

        m_isInitialized = true;

        m_rectTransform = GetComponent<RectTransform>();

        InitScaleBounds();

        var screenCenter = new Vector2(Screen.width, Screen.height) / 2.0f;
        SetZoom(screenCenter, 1.0f);
    }

    private void InitScaleBounds()
    {
        m_scaleMin = CalculateScaleMin();
        m_scaleMax = CalculateScaleMax();
    }

    private void ChangeLocalPositionInternal(Vector2 delta)
    {
        Vector2 position = m_rectTransform.position;
        position += delta;
        m_rectTransform.position = position;
    }

    private float CalculateScaleMin()
    {
        float width = m_rectTransform.rect.width;
        float height = m_rectTransform.rect.height;

        float parentWidth = m_parentRectTransform.rect.width;
        float parentHeight = m_parentRectTransform.rect.height;

        bool fillHori = width / height > parentWidth / parentHeight;

        if (fillHori)
            return parentWidth / width;
        else
            return parentHeight / height;
    }

    private float CalculateScaleMax()
    {
        const float InchToCm = 0.393701f;
        return Screen.dpi * InchToCm;
    }

    public RectSides GetParentRect()
    {
        Vector3[] corners = new Vector3[4];
        m_parentRectTransform.GetWorldCorners(corners);
        return RectFromCorners(corners);
    }

    public RectSides GetImageRect()
    {
        Vector3[] corners = new Vector3[4];
        m_rectTransform.GetWorldCorners(corners);
        return RectFromCorners(corners);
    }

    public RectSides RectFromCorners(Vector3[] corners)
    {
        var rect = new RectSides();
        rect.Left = corners[0].x;
        rect.Right = corners[2].x;
        rect.Top = corners[2].y;
        rect.Bottom = corners[0].y;
        return rect;
    }

    private void RecreateImage()
    {
        int width = (int)m_rectTransform.rect.width;
        int height = (int)m_rectTransform.rect.height;

        if (Image == null)
        {
            Image = new Texture2D(width, height);
            Image.wrapMode = TextureWrapMode.Clamp;
            Image.filterMode = FilterMode.Point;
            Image.name = "image";
        }

        int pixelCount = width * height;

        Color[] colors = new Color[pixelCount];
        for (int i = 0; i < colors.Length; i++)
            colors[i] = new Color32(255, 255, 255, 0);

        Image.SetPixels(colors);
        Image.Apply();

        m_image.texture = Image;
    }
}
