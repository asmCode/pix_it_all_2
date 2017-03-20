using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BundleView : MonoBehaviour
{
    public Text m_id;
    public Text m_name;
    public Text m_numberOfImages;
    public Text m_price;

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
            m_price.text = "available";
        else
        {
            if (!string.IsNullOrEmpty(bundle.LocalizedPrice))
            {
                m_price.text = bundle.LocalizedPrice;
            }
            else
            {
                m_price.text = "BUY";
            }
        }
    }

    public void UiEvent_Clicked()
    {
        if (Clicked != null)
            Clicked(BundleId);
    }
}
