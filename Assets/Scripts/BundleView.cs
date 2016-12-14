using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BundleView : MonoBehaviour
{
    public Text m_id;
    public Text m_name;
    public Text m_numberOfImages;

    public string BundleId
    {
        get;
        private set;
    }

    public void SetBundle(BundleData bundle)
    {
        BundleId = null;

        if (bundle == null)
            return;

        m_id.text = bundle.Id;
        m_name.text = bundle.Name;

        var images = bundle.GetImages();
        if (images != null)
            m_numberOfImages.text = images.Length.ToString();
        else
            m_numberOfImages.text = "0";
    }
}
