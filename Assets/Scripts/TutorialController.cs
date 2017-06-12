using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController
{
    private TutorialView m_view;
    private List<TutorialStep> m_steps;
    private int m_currentStep;

    public void Init(TutorialView view)
    {
        m_view = view;
        m_view.IndicatorTapped += HandleIndicatorTapped;
        m_view.Tapped += HandleTapped;

        m_steps = new List<TutorialStep>();
        m_steps.Add(new TutorialStep1(this, view));
        m_steps.Add(new TutorialStep2(this, view));

        SetStep(0);
    }

    private void HandleIndicatorTapped()
    {
        m_steps[m_currentStep].NotifyIndicatorTapped();
        NextStep();
    }

    private void HandleTapped()
    {
        m_steps[m_currentStep].NotifyTapped();
        NextStep();
    }

    public void NextStep()
    {
        if (m_currentStep == m_steps.Count - 1)
        {
            m_view.Hide();
            return;
        }

        SetStep(m_currentStep + 1);
    }

    private void SetStep(int step)
    {
        if (m_currentStep != -1)
            m_steps[m_currentStep].Dectivate();

        m_currentStep = step;

        m_steps[m_currentStep].Activate();
    }
}
