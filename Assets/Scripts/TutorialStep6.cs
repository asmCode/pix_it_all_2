using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStep6 : TutorialStep
{
    private const int RedColorIndex = 0;
    
    private GameplayController m_gameplayController;
    private float m_previewTime;

    public TutorialStep6(TutorialController ctrl, TutorialView view, GameplayController gameplayController) :
        base(ctrl, view)
    {
        m_gameplayController = gameplayController;
    }

    public override void Activate()
    {
        m_view.SetStep(5);
        m_view.SetIndicatorTarget(m_view.m_previewButton);
    }

    public override void NotifyIndicatorPressed(Vector2 screenPoint)
    {
        m_previewTime = Time.time;
        m_gameplayController.HandlePreviewPressed();
    }

    public override void NotifyIndicatorReleased(Vector2 screenPoint)
    {
        m_gameplayController.HandlePreviewReleased();

        var duration = Time.time - m_previewTime;
        if (duration < 0.08f)
            return;

        m_ctrl.NextStep();
    }
}
