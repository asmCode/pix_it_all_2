using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryView : MonoBehaviour
{
    public Transform m_stars;
    public Text m_labelTimeValue;
    public GameObject m_newRecord;
    public GameObject m_record;
    public Text m_labelLevelName;
    public Text m_labelRecordValue;
    public Text m_timeFor3Stars;
    public Text m_timeFor2Stars;

    public event System.Action BackToMenuClicked;

    public void Show(string levelName, int stars, float time, bool record, float currentRecord, float timeFor3Stars, float timeFor2Stars)
    {
        UiUtils.ShowChildren(m_stars, stars);
        m_labelTimeValue.text = Utils.TimeToString(time);
        m_labelLevelName.text = levelName;
        m_newRecord.SetActive(record);
        m_record.SetActive(!record);
        m_labelRecordValue.text = Utils.TimeToString(currentRecord);
        m_timeFor3Stars.text = Utils.TimeToString(timeFor3Stars);
        m_timeFor2Stars.text = Utils.TimeToString(timeFor2Stars);

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void UiEvent_BackToMenuClicked()
    {
        if (BackToMenuClicked != null)
            BackToMenuClicked();
    }
}
