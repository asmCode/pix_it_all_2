using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialIndicator : MonoBehaviour
{
    public RectTransform m_target;

    private RectTransform m_rectTr;
    
    private void Awake()
    {
        m_rectTr = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (m_target == null)
            return;

        m_rectTr.position = m_target.position;

        m_rectTr.sizeDelta = m_target.sizeDelta;
    }
}
