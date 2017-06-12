using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStep2 : TutorialStep
{
    public TutorialStep2(TutorialController ctrl, TutorialView view) : base(ctrl, view)
    {

    }

    public override void Activate()
    {
        m_view.SetStep(1);
        m_view.SetIndicatorTarget(m_view.m_paletteButton);
    }

    public override void Dectivate()
    {

    }
}
