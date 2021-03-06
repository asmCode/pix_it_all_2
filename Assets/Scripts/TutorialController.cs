﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController
{
    private TutorialView m_view;
    private List<TutorialStep> m_steps;
    private int m_currentStep;
    private GameplayController m_gameplayController;

    public void Init(TutorialView view, GameplayController gameplayController)
    {
        m_view = view;
        m_gameplayController = gameplayController;
        m_view.IndicatorTapped += HandleIndicatorTapped;
        m_view.IndicatorPressed += HandleIndicatorPressed;
        m_view.IndicatorReleased += HandleIndicatorReleased;
        m_view.Tapped += HandleTapped;

        m_steps = new List<TutorialStep>();
        m_steps.Add(new TutorialStep1(this, view));
        m_steps.Add(new TutorialStep2(this, view, gameplayController));
        m_steps.Add(new TutorialStep3(this, view, gameplayController));
        m_steps.Add(new TutorialStep4(this, view, gameplayController));
        m_steps.Add(new TutorialStep5(this, view));
        m_steps.Add(new TutorialStep6(this, view, gameplayController));
        m_steps.Add(new TutorialStep7(this, view));
        m_steps.Add(new TutorialStep8(this, view, gameplayController));

        SetStep(0);
    }

    public void Update()
    {
        m_steps[m_currentStep].Update();
    }

    private void HandleIndicatorTapped(Vector2 screenPoint)
    {
        m_steps[m_currentStep].NotifyIndicatorTapped(screenPoint);
    }

    private void HandleIndicatorPressed(Vector2 screenPoint)
    {
        m_steps[m_currentStep].NotifyIndicatorPressed(screenPoint);
    }

    private void HandleIndicatorReleased(Vector2 screenPoint)
    {
        m_steps[m_currentStep].NotifyIndicatorReleased(screenPoint);
    }

    private void HandleTapped()
    {
        m_steps[m_currentStep].NotifyTapped();
    }

    public void NextStep()
    {
        if (m_currentStep == m_steps.Count - 1)
        {
            SendTrackingEvent(m_currentStep + 1);  // HACK: fix it some day
            m_view.Hide();
            return;
        }

        SetStep(m_currentStep + 1);
    }

    private void SetStep(int step)
    {
        if (m_currentStep != -1)
            m_steps[m_currentStep].Deactivate();

        m_currentStep = step;

        SendTrackingEvent(m_currentStep);

        m_steps[m_currentStep].Activate();
    }

    private void SendTrackingEvent(int step)
    {
        if (step == 0)
            return;

        var prevStep = step - 1;
            
        string progressName = "tutorial_step_" + prevStep.ToString();

        GameAnalyticsSDK.GameAnalytics.NewProgressionEvent(
            GameAnalyticsSDK.GAProgressionStatus.Complete,
            progressName);
    }
}
