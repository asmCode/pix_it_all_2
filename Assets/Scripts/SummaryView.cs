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

    public float m_animTimeProgress;
    private float m_prevTimeToDisplay;
    public bool m_animShowNewRecord;

    public event System.Action BackToMenuClicked;
    public event System.Action Clicked;

    private Animator m_animator;
    private int m_starsCount;
    private float m_time;
    private int m_numOfPlayedStarSound;
    private System.Action m_completedBannerFinishedCallback;

    private bool m_cachedRecord;

    public void Show(string levelName, int stars, float time, bool record, float currentRecord, float timeFor3Stars, float timeFor2Stars)
    {
        m_time = time;
        m_cachedRecord = record;
        m_starsCount = stars;

        UiUtils.ShowChildren(m_stars, stars);
        m_labelTimeValue.text = Utils.TimeToString(time);
        m_labelLevelName.text = levelName;
        m_newRecord.SetActive(record && m_animShowNewRecord);
        m_record.SetActive(!record);
        m_labelRecordValue.text = Utils.TimeToString(currentRecord);
        m_timeFor3Stars.text = Utils.TimeToString(timeFor3Stars);
        m_timeFor2Stars.text = Utils.TimeToString(timeFor2Stars);

        gameObject.SetActive(true);
        m_animator.Play("SummaryAppear", 0, 0.0f);
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

    public void UiEvent_Clicked()
    {
        Debug.Log("cups");

        if (Clicked != null)
            Clicked();
    }

    public void ShowCompletedBanner(System.Action finishedCallback)
    {
        gameObject.SetActive(true);
        m_completedBannerFinishedCallback = finishedCallback;
        m_animator.Play("CompletedBanner", 0, 0.0f);
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

    public void AnimEvent_CompletedBannerFinished()
    {
        if (m_completedBannerFinishedCallback != null)
            m_completedBannerFinishedCallback();
    }

    public void AnimEvent_StarSound()
    {
        if (m_numOfPlayedStarSound >= m_starsCount)
            return;

        AudioManager.GetInstance().SoundSummaryStar.Play();
        m_numOfPlayedStarSound++;
    }

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        AudioManager.GetInstance().SoundSummaryTime.Stop();
    }
}
