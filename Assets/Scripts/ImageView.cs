using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageView : MonoBehaviour
{
    public Text m_id;
    public Text m_name;
    public Text m_numberOfColors;

    public Text m_bestTime;
    public Text m_inProgress;
    public Text m_completed;
    public Text m_stars;

    public GameObject m_bestTimeGroup;
    public GameObject m_inProgressGroup;
    public GameObject m_completedGroup;
    public GameObject m_starsGroup;

    public RawImage m_thumbnail;

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
        m_numberOfColors.text = image.Colors.Length.ToString();

        m_bestTime.text = Utils.TimeToString(data.BestTime);
        m_inProgress.text = data.InProgress.ToString();
        m_completed.text = data.BestTime != 0 ? true.ToString() : false.ToString();
        m_stars.text = data.Stars.ToString();

        m_thumbnail.texture = image.Texture;
    }

    public void SetStoreMode(bool storeMode)
    {
        m_bestTimeGroup.SetActive(!storeMode);
        m_inProgressGroup.SetActive(!storeMode);
        m_completedGroup.SetActive(!storeMode);
        m_starsGroup.SetActive(!storeMode);
    }

    public void UiEvent_Clicked()
    {
        if (Clicked != null)
            Clicked(ImageId);
    }
}
