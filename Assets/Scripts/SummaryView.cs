using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryView : MonoBehaviour
{
    public Text m_labelStarsValue;
    public Text m_labelTimeValue;
    public Text m_labelRecordValue;
    public Text m_timeFor3Stars;
    public Text m_timeFor2Stars;

    public event System.Action NextLevelClicked;
    public event System.Action RetryClicked;
    public event System.Action BackToMenuClicked;

    public void Show(int stars, float time, bool record, float timeFor3Stars, float timeFor2Stars)
    {
        m_labelStarsValue.text = stars.ToString();
        m_labelTimeValue.text = Utils.TimeToString(time);
        m_labelRecordValue.text = record.ToString().ToUpper();
        m_timeFor3Stars.text = Utils.TimeToString(timeFor3Stars);
        m_timeFor2Stars.text = Utils.TimeToString(timeFor2Stars);

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
