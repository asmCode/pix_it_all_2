using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStep6 : TutorialStep
{
    private const int RedColorIndex = 0;
    
    private GameplayController m_gameplayController;
    private float m_previewTime;
    private bool m_previewWasReleased;

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
        m_previewWasReleased = true;
    }

    public override void Update()
    {
        if (!m_previewWasReleased || m_previewTime == 0.0f)
            return;

        var duration = Time.time - m_previewTime;
        if (duration < 0.6f)
            return;

        m_gameplayController.HandlePreviewReleased();
        m_ctrl.NextStep();
    }
}
