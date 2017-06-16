using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageView : MonoBehaviour
{
    public Text m_name;
    public Transform m_colors;
    public Transform m_stars;
    public Transform m_completedIndicator;
    public Transform m_inProgressIndicator;

    public Text m_bestTime;
    public Text m_dimensions;

    public GameObject m_bestTimeGroup;
    public GameObject m_inProgressGroup;
    public GameObject m_starsGroup;

    public RawImage m_thumbnail;
    public AspectRatioFitter m_imageAspectRatio;

    public event System.Action<string> Clicked;

    private Animator m_animator;

    public string ImageId
    {
        get;
        private set;
    }

    public void SetImage(ImageViewData data)
    {
        ImageId = null;

        if (data == null)
            return;

        var image = data.ImageData;

        ImageId = image.Id;

        m_name.text = image.Name;

        if (data.LevelProgress.BestTime != 0)
        {
            UiUtils.ShowChildren(m_stars, data.Stars);
            m_bestTime.text = Utils.TimeToString(data.LevelProgress.BestTime);
            m_bestTimeGroup.gameObject.SetActive(true);
            m_starsGroup.gameObject.SetActive(true);
        }
        else 
        {
            m_bestTimeGroup.gameObject.SetActive(false);
            m_starsGroup.gameObject.SetActive(false);
        }

        UiUtils.ShowChildren(m_colors, image.Colors.Length);
        
        SetColors(image.Colors);

        m_dimensions.text = string.Format("{0} x {1}", image.Texture.width, image.Texture.height);

        m_imageAspectRatio.aspectRatio = (float)image.Texture.width / (float)image.Texture.height;

        m_thumbnail.texture = image.Texture;
    }

    public void ShowCompletedIndicator(bool show, bool animated)
    {
        if (!show)
        {
            m_completedIndicator.gameObject.SetActive(false);
            return;
        }

        m_completedIndicator.gameObject.SetActive(true);

        if (animated)
            m_animator.Play("ImageViewCompleted", 0, 0.0f);
    }

    public void ShowInProgressIndicator(bool show, bool animated)
    {
        if (!show)
        {
            m_inProgressIndicator.gameObject.SetActive(false);
            return;
        }

        m_inProgressIndicator.gameObject.SetActive(true);

        if (animated)
            m_animator.Play("ImageViewInProgress", 0, 0.0f);
    }

    private void SetColors(Color[] colors)
    {
        if (m_colors.childCount < colors.Length)
            return;

        for (int i = 0; i < colors.Length; i++)
        {
            var child = m_colors.GetChild(i);
            var image = child.GetComponent<Image>();
            if (image == null)
                continue;
            
            image.color = colors[i];
        }
    }

    public void SetStoreMode(bool storeMode)
    {
        // m_bestTimeGroup.SetActive(!storeMode);
        // m_inProgressGroup.SetActive(!storeMode);
        // m_starsGroup.gameObject.SetActive(!storeMode);
    }

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void Start()
    {
        ShowCompletedIndicator(false, false);
        ShowInProgressIndicator(false, false);
    }

    public void UiEvent_Clicked()
    {
        if (Clicked != null)
            Clicked(ImageId);
    }
}
