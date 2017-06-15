using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStep
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

    public virtual void Deactivate()
    {

    }

    public virtual void NotifyIndicatorTapped(Vector2 screenPoint)
    {
    }

    public virtual void NotifyIndicatorPressed(Vector2 screenPoint)
    {
    }

    public virtual void NotifyIndicatorReleased(Vector2 screenPoint)
    {
    }

    public virtual void NotifyTapped()
    {
    }

    public virtual void Update()
    {
    }
}
