using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public Image m_image;
    public RectTransform m_parentRectTransform;
    public RectTransform m_scalePivot;

    private RectTransform m_rectTransform;
    private float m_scaleMin;
    private float m_scaleMax;
    private float m_zoom = 1.0f;

    public RectTransform RectTransform
    {
        get { return m_rectTransform; }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            var scalePivotPosition = m_scalePivot.position;
            var boardPosition = m_rectTransform.position;
            m_scalePivot.position = new Vector2(0, 0);
            m_rectTransform.position = boardPosition;
            m_scalePivot.localScale = new Vector3(22, 22, 1.0f);
            boardPosition = m_rectTransform.position;
            m_scalePivot.position = scalePivotPosition;
            m_rectTransform.position = boardPosition;
        }
    }

    public void SetZoom(Vector2 pivot, float zoom)
    {
        //return;

        //Debug.Log("-----------------");

        var boardPosition = m_rectTransform.position;
        var scalePivotPosition = m_scalePivot.position;

        Vector3 worldPoint;
        //RectTransformUtility.ScreenPointToWorldPointInRectangle(m_scalePivot, pivot, Camera.main, out worldPoint);
        //m_scalePivot.position = worldPoint;
        m_scalePivot.position = pivot;
        m_rectTransform.position = boardPosition;

        m_zoom = Mathf.Clamp01(zoom);
        float scale = GetScale(m_zoom);
        //gameObject.transform.localScale = new Vector3(scale, scale, 1.0f);

        m_scalePivot.localScale = new Vector3(scale, scale, 1.0f);

        boardPosition = m_rectTransform.position;
        m_scalePivot.position = scalePivotPosition;
        m_rectTransform.position = boardPosition;

        FixPositionInsideParent();

        //Debug.LogFormat("screen = ({0}, {1}), scale_pivot = ({2}, {3})", pivot.x, pivot.y, worldPoint.x, worldPoint.y);
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
        Vector2 position = m_rectTransform.position;
        position += delta;
        m_rectTransform.position = position;
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
        return Screen.dpi / 2;
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
