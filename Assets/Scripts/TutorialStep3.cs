using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStep3 : TutorialStep
{
    private const int RedColorIndex = 0;
    
    private GameplayController m_gameplayController;

    public TutorialStep3(TutorialController ctrl, TutorialView view, GameplayController gameplayController) :
        base(ctrl, view)
    {
        m_gameplayController = gameplayController;
    }

    public override void Activate()
    {
        m_view.SetStep(2);

        var redColor = GetRedColor();
        m_view.SetIndicatorTarget(redColor);
    }

    public override void Dectivate()
    {

    }

    public override void NotifyIndicatorTapped(Vector2 screenPoint)
    {
        m_gameplayController.SetColor(RedColorIndex);
        m_gameplayController.HidePalette();
        m_ctrl.NextStep();
    }

    private RectTransform GetRedColor()
    {
        if (m_view.m_paletteContainer.childCount <= RedColorIndex)
            return null;

        var redColorObject = m_view.m_paletteContainer.GetChild(RedColorIndex);
        if (redColorObject == null)
            return null;

        return redColorObject.GetComponent<RectTransform>();
    }
}
