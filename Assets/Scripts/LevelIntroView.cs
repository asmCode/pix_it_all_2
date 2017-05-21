using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelIntroView : MonoBehaviour
{
    public Text m_labelLevelName;
    public Text m_labelBestTime;
    public Text m_labelTime;
    public Text m_labelStarsReq;

    public event System.Action StartPressed;
    public event System.Action BackPressed;

    public void Show(ImageViewData imageViewData)
    {
        gameObject.SetActive(true);

        m_labelLevelName.text = imageViewData.ImageData.Name;

        if (imageViewData.LevelProgress.IsCompleted)
        {
            m_labelBestTime.text = Utils.TimeToString(imageViewData.LevelProgress.BestTime);
            m_labelBestTime.gameObject.SetActive(true);
        }
        else
            m_labelBestTime.gameObject.SetActive(false);

        if (imageViewData.LevelProgress.IsInProgress)
        {
            m_labelTime.text = Utils.TimeToString(imageViewData.LevelProgress.ContinueTime);
            m_labelTime.gameObject.SetActive(true);
        }
        else
            m_labelTime.gameObject.SetActive(false);

        int tilesCount = imageViewData.ImageData.Texture.width * imageViewData.ImageData.Texture.height;
        int colorsCount = imageViewData.ImageData.Colors.Length;
        float timeFor3Stars = StarRatingCalc.RequiredTimeForStars(3, tilesCount, colorsCount);
        float timeFor2Stars = StarRatingCalc.RequiredTimeForStars(2, tilesCount, colorsCount);
        m_labelStarsReq.text = string.Format("2 stars - {0}\n3 stars - {1}\n", Utils.TimeToString(timeFor2Stars), Utils.TimeToString(timeFor3Stars));
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void UiEvent_BackButtonPreesed()
    {
        if (BackPressed != null)
            BackPressed();
    }

    public void UiEvent_StartButtonPressed()
    {
        if (StartPressed != null)
            StartPressed();
    }
}
