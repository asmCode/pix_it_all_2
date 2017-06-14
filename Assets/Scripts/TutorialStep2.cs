using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStep2 : TutorialStep
{
    private GameplayController m_gameplayController;

    public TutorialStep2(TutorialController ctrl, TutorialView view, GameplayController gameplayController) :
        base(ctrl, view)
    {
        m_gameplayController = gameplayController;
    }

    public override void Activate()
    {
        m_view.SetStep(1);
        m_view.SetIndicatorTarget(m_view.m_paletteButton);
    }

    public override void Dectivate()
    {

    }

    public override void NotifyIndicatorTapped(Vector2 screenPoint)
    {
        m_gameplayController.ShowPalette();
        m_ctrl.NextStep();
    }
}
