using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeout
{
    private System.DateTime m_startTime;
    private float m_timeout;

    public bool IsTimeout
    {
        get
        {
            return (System.DateTime.Now - m_startTime).TotalSeconds >= m_timeout;
        }
    }

    public void Start(float timeout)
    {
        m_startTime = System.DateTime.Now;
        m_timeout = timeout;
    }
}
