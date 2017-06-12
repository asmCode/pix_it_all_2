using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStep : MonoBehaviour
{
    protected TutorialView m_view;
    protected TutorialController m_ctrl;

    public TutorialStep(TutorialController ctrl, TutorialView view)
    {
        m_view = view;
        m_ctrl = ctrl;
    }

    public virtual void Activate()
    {

    }

    public virtual void Dectivate()
    {

    }

    public virtual void NotifyIndicatorTapped()
    {
        Debug.Log("Indicator tapped");
    }

    public virtual void NotifyTapped()
    {
        Debug.Log("Tapped");
    }
}
