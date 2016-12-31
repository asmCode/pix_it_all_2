using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageView : MonoBehaviour
{
    public Text m_id;
    public Text m_name;
    public Text m_numberOfColors;
    public RawImage m_thumbnail;

    public event System.Action<string> Clicked;

    public string ImageId
    {
        get;
        private set;
    }

    public void SetImage(ImageData image)
    {
        ImageId = null;

        if (image == null)
            return;

        ImageId = image.Id;

        m_id.text = image.Id;
        m_name.text = image.Name;
        m_numberOfColors.text = image.Colors.Length.ToString();
        m_thumbnail.texture = image.Texture;
    }

    public void UiEvent_Clicked()
    {
        if (Clicked != null)
            Clicked(ImageId);
    }
}
