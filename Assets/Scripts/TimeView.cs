using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeView : MonoBehaviour
{
    private Text m_labelTime;
    private Animator m_animator;
    private float m_lastRedBlinkTime;  // This prevents from spamming red blink when using preview

    public void SetTime(float time)
    {
        m_labelTime.text = Utils.TimeToString(time);
    }

    public void BlinkRed()
    {
        if ((Time.realtimeSinceStartup - m_lastRedBlinkTime) < 0.15f)
            return;

        m_lastRedBlinkTime = Time.realtimeSinceStartup;

        m_animator.Play("TimeRedBlink", 0, 0.0f);
    }

    public void BlinkGreen()
    {
        m_animator.Play("TimeGreenBlink", 0, 0.0f);
    }

    private void Awake()
    {
        m_labelTime = transform.FindChild("LabelTime").GetComponent<Text>();
        m_animator = GetComponent<Animator>();
    }
}
