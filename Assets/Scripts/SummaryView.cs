using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryView : MonoBehaviour
{
    public Text m_labelStarsValue;
    public Text m_labelTimeValue;
    public Text m_labelRecordValue;

    public event System.Action NextLevelClicked;
    public event System.Action RetryClicked;
    public event System.Action BackToMenuClicked;

    public void Show(int stars, float time, bool record)
    {
        m_labelStarsValue.text = stars.ToString();
        m_labelTimeValue.text = string.Format("{0}:{1}", (int)time / 60, (int)time % 60);
        m_labelRecordValue.text = record.ToString().ToUpper();

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void UiEvent_NextLevelClicked()
    {
        if (NextLevelClicked != null)
            NextLevelClicked();
    }

    public void UiEvent_RetryClicked()
    {
        if (RetryClicked != null)
            RetryClicked();
    }

    public void UiEvent_BackToMenuClicked()
    {
        if (BackToMenuClicked != null)
            BackToMenuClicked();
    }
}
