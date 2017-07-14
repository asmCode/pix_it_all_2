using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BundleView : MonoBehaviour
{
    public Text m_id;
    public Text m_name;
    public Text m_numberOfImages;
    public Text m_available;
    public RawImage m_icon;

    private AspectRatioFitter m_aspect;

    public event System.Action<string> Clicked;

    public string BundleId
    {
        get;
        private set;
    }

    public void SetBundle(BundleViewData bundle)
    {
        BundleId = null;

        if (bundle == null)
            return;

        BundleId = bundle.BundleData.Id;

        m_id.text = bundle.BundleData.Id;
        m_name.text = bundle.BundleData.Name;

        var images = bundle.BundleData.GetImages();
        if (images != null)
            m_numberOfImages.text = images.Length.ToString();
        else
            m_numberOfImages.text = "0";

        if (bundle.IsAvailable)
            m_available.text = "available";
        else
            m_available.text = "buy";

        SetIcon(bundle.BundleData);
    }

    public void UiEvent_Clicked()
    {
        if (Clicked != null)
            Clicked(BundleId);
    }

    private void SetIcon(BundleData bundle)
    {
        var images = bundle.GetImages();
        if (images == null || images.Length == 0)
            return;
        
        var icon = images[0];

        m_icon.texture = icon.Texture;

        float scale = icon.Width < icon.Height ?
            (float)icon.Texture.width / (float)icon.Width :
            (float)icon.Texture.height / (float)icon.Height;

        m_icon.transform.localScale = new Vector2(scale, scale);

        m_aspect.aspectRatio = (float)icon.Texture.width / (float)icon.Texture.height;
    }

    private void Awake()
    {
        m_aspect = m_icon.GetComponent<AspectRatioFitter>();
    }
}
