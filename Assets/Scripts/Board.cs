using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public RawImage m_referenceImage;
    public RawImage m_image;
    public RectTransform m_parentRectTransform;
    public RectTransform m_scalePivot;
    public AnimatedImage m_pixelFillPrefab;
    public RectTransform m_pixelFillContainer;

    public event System.Action PreviewEnded;

    private PixelFillEffect m_pixelFillEffect;
    private RectTransform m_rectTransform;
    private float m_scaleMin;
    private float m_scaleMax;
    private float m_scaleOptimal;
    private float m_scale = 1.0f;
    private float m_previewFade;
    private float m_previewFadeDir = 0.0f;
    private Color m_previewImageColor = new Color(1, 1, 1, 1);

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

    public bool IsPreviewActive
    {
        get { return m_referenceImage.gameObject.activeSelf;  }
    }

    public float MaxScale
    {
        get { return m_scaleMax; }
    }

    public float MinScale
    {
        get { return m_scaleMin; }
    }

    public float OptimalScale
    {
        get { return m_scaleOptimal; }
    }

    public float CurrentScale
    {
        get { return m_scale; }
    }

    public void SetSize(int width, int height)
    {
        m_rectTransform.sizeDelta = new Vector2(width, height);
        InitScaleBounds();
        RecreateImage();
    }

    public void PlayPixelFillEffect(int x, int y, Color color)
    {
        m_pixelFillEffect.Show(x, y);
    }

    public bool IsScaleLessThanOptimal()
    {
        return m_scale < m_scaleOptimal;
    }

    public void ShowPreview()
    {
        m_previewFadeDir = 1.0f;
        m_referenceImage.gameObject.SetActive(true);
    }

    public void HidePreview()
    {
        m_previewFadeDir = -1.0f;
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

    public void SetScale(Vector2 pivot, float scale)
    {
        m_scale = Mathf.Clamp(scale, m_scaleMin, m_scaleMax);

        var boardPosition = m_rectTransform.position;
        var scalePivotPosition = m_scalePivot.position;

        m_scalePivot.position = pivot;
        m_rectTransform.position = boardPosition;

        m_scalePivot.localScale = new Vector3(m_scale, m_scale, 1.0f);

        boardPosition = m_rectTransform.position;
        m_scalePivot.position = scalePivotPosition;
        m_rectTransform.position = boardPosition;
    }

    public void SetLocalPosition(Vector2 position)
    {
        m_rectTransform.localPosition = position;
    }

    public void SetPosition(Vector2 position)
    {
        m_rectTransform.position = position;
    }

    public Vector2 GetPosition()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }

    public void ChangeLocalPosition(Vector2 delta)
    {
        ChangeLocalPositionInternal(delta);
    }

    public void Scale(Vector2 pivot, float scale)
    {
        float newScale = m_scale * scale;
        SetScale(pivot, newScale);
    }

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        const float fadeSpeed = 8.0f;
        if (m_previewFadeDir >= 1.0f)
        {
            m_previewFade = Mathf.MoveTowards(m_previewFade, 1.0f, m_previewFadeDir * Time.deltaTime * fadeSpeed);
            if (m_previewFade >= 1.0f)
                m_previewFadeDir = 0.0f;
        } else if (m_previewFadeDir <= -1.0f)
        {
            m_previewFade = Mathf.MoveTowards(m_previewFade, 0.0f, -m_previewFadeDir * Time.deltaTime * fadeSpeed);
            if (m_previewFade <= 0.0f)
            {
                m_previewFadeDir = 0.0f;
                m_referenceImage.gameObject.SetActive(false);
                if (PreviewEnded != null)
                    PreviewEnded();
            }
        }

        m_previewImageColor.a = m_previewFade;
        m_referenceImage.color = m_previewImageColor;
    }

    private void Init()
    {
        if (m_isInitialized)
            return;

        m_isInitialized = true;

        m_rectTransform = GetComponent<RectTransform>();

        InitScaleBounds();

        m_pixelFillEffect = new PixelFillEffect();
        m_pixelFillEffect.Init(m_pixelFillPrefab, m_pixelFillContainer);

        var screenCenter = new Vector2(Screen.width, Screen.height) / 2.0f;
        SetScale(screenCenter, 1.0f);
    }

    private void InitScaleBounds()
    {
        m_scaleMin = CalculateScaleMin();
        m_scaleMax = CalculateScaleMax();
        m_scaleOptimal = CalculateScaleOptimal();

        if (m_scaleMax < m_scaleMin)
            m_scaleMax = m_scaleMin;

        m_scaleOptimal = Mathf.Clamp(m_scaleOptimal, m_scaleMin, m_scaleMax);
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

    private float CalculateScaleOptimal()
    {
        float scaleOptimal = CalculateScaleMax() * 0.8f;
        float scaleMax = CalculateScaleMin();
        if (scaleOptimal < scaleMax)
            scaleOptimal = scaleMax;
        
        return scaleOptimal;
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
