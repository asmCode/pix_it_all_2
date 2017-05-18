using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageView : MonoBehaviour
{
    public Text m_id;
    public Text m_name;
    public Transform m_colors;
    public Transform m_stars;

    public Text m_bestTime;
    public Text m_inProgress;
    public Text m_dimensions;

    public GameObject m_bestTimeGroup;
    public GameObject m_inProgressGroup;
    public GameObject m_starsGroup;

    public RawImage m_thumbnail;
    public AspectRatioFitter m_imageAspectRatio;

    public event System.Action<string> Clicked;

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

        m_id.text = image.Id;
        m_name.text = image.Name;

        Debug.LogFormat("Best time = {0}", data.BestTime);
        if (data.BestTime != 0)
        {
            UiUtils.ShowChildren(m_stars, data.Stars);
            m_bestTime.text = Utils.TimeToString(data.BestTime);
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

        m_inProgress.text = data.InProgress.ToString();
        m_dimensions.text = string.Format("{0} x {1}", image.Texture.width, image.Texture.height);

        m_imageAspectRatio.aspectRatio = (float)image.Texture.width / (float)image.Texture.height;

        m_thumbnail.texture = image.Texture;
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

    public void UiEvent_Clicked()
    {
        if (Clicked != null)
            Clicked(ImageId);
    }
}
