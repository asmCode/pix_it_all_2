using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public Image m_image;
    public RectTransform m_parentRectTransform;

    private RectTransform m_rectTransform;
    private float m_scaleMin;
    private float m_scaleMax;
    private float m_zoom = 1.0f;

    public RectTransform RectTransform
    {
        get { return m_rectTransform; }
    }

    public void SetZoom(Vector2 pivot, float zoom)
    {
        m_zoom = Mathf.Clamp01(zoom);
        float scale = GetScale(m_zoom);
        gameObject.transform.localScale = new Vector3(scale, scale, 1.0f);

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

    public void ChangeZoom(float delta)
    {
        float newZoom = m_zoom + delta / 1000.0f;
        SetZoom(Vector2.zero, newZoom);
    }

    public void FixPositionInsideParent()
    {
        var sizeInPixels = GetSizeInPixels();

        var parentRect = GetParentRect();
        var imageRect = GetImageRect();

        // Fix horizontally
        if (imageRect.Width <= parentRect.Width)
        {
            m_rectTransform.localPosition = new Vector2(0, m_rectTransform.localPosition.x);
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
        m_rectTransform = GetComponent<RectTransform>();
        m_image = transform.GetComponentInChildren<Image>();

        InitScaleBounds();

        SetZoom(Vector2.zero, 1.0f);
    }

    private void InitScaleBounds()
    {
        m_scaleMin = CalculateScaleMin();
        m_scaleMax = CalculateScaleMax();
    }

    private void ChangeLocalPositionInternal(Vector2 delta)
    {
        Vector2 position = m_rectTransform.localPosition;
        position += delta;
        m_rectTransform.localPosition = position;
    }

    private float CalculateScaleMin()
    {
        float width = m_image.sprite.rect.width;
        float height = m_image.sprite.rect.height;

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
        return Screen.dpi;
    }

    public Vector2 GetSizeInPixels()
    {
        return new Vector2(
            RectTransform.rect.width * RectTransform.lossyScale.x,
            RectTransform.rect.height * RectTransform.lossyScale.y);
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
}
