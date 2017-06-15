using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStep7 : TutorialStep
{
    public TutorialStep7(TutorialController ctrl, TutorialView view) : base(ctrl, view)
    {

    }

    public override void Activate()
    {
        m_view.SetStep(6);
        m_view.SetIndicatorTarget(null);
    }

    public override void NotifyTapped()
    {
        m_ctrl.NextStep();
    }
}
