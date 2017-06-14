using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStep5 : TutorialStep
{
    public TutorialStep5(TutorialController ctrl, TutorialView view) : base(ctrl, view)
    {

    }

    public override void Activate()
    {
        m_view.SetStep(4);
        m_view.SetIndicatorTarget(null);
    }

    public override void Dectivate()
    {

    }

    public override void NotifyTapped()
    {
        m_ctrl.NextStep();
    }
}
