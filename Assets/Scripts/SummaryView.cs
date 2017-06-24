﻿using System.Collections;
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

    public float m_animTimeProgress;
    private float m_prevTimeToDisplay;
    public bool m_animShowNewRecord;

    public event System.Action BackToMenuClicked;

    private float m_time;

    private bool m_cachedRecord;

    public void Show(string levelName, int stars, float time, bool record, float currentRecord, float timeFor3Stars, float timeFor2Stars)
    {
        m_time = time;
        m_cachedRecord = record;

        UiUtils.ShowChildren(m_stars, stars);
        m_labelTimeValue.text = Utils.TimeToString(time);
        m_labelLevelName.text = levelName;
        m_newRecord.SetActive(record && m_animShowNewRecord);
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

    private void Update()
    {
        float timeToDisplay = m_time * m_animTimeProgress;

        m_labelTimeValue.text = Utils.TimeToString(timeToDisplay);
        m_newRecord.SetActive(m_cachedRecord && m_animShowNewRecord);
        m_record.SetActive(!m_cachedRecord && m_animShowNewRecord);

        if ((int)timeToDisplay != (int)(m_prevTimeToDisplay))
        {
            m_prevTimeToDisplay = timeToDisplay;
            AudioManager.GetInstance().SoundSummaryTime.Play();
        }
    }

    public void AnimEvent_TimeStarted()
    {
    }

    public void AnimEvent_TimeFinished()
    {
    }

    public void AnimEvent_StarSound()
    {
        AudioManager.GetInstance().SoundSummaryStar.Play();
    }

    private void OnDisable()
    {
        AudioManager.GetInstance().SoundSummaryTime.Stop();
    }
}
