﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStep8 : TutorialStep
{
    private const int RedColorIndex = 0;
    private const int NumSuccessTilesToFinishTutorial = 4;
    
    private GameplayController m_gameplayController;
    private int m_failsInARow;
    private int m_totalSuccess;

    public TutorialStep8(TutorialController ctrl, TutorialView view, GameplayController gameplayController) :
        base(ctrl, view)
    {
        m_gameplayController = gameplayController;
    }

    public override void Activate()
    {
        m_failsInARow = 0;
        m_gameplayController.Gameplay.TileRevealedWithSuccess += HandleTileRevealedWithSuccess;
        m_gameplayController.Gameplay.TileRevealFailed += HandleTileRevealFailed;

        m_view.SetStep(7);
        m_view.Hide();
        m_view.SetIndicatorTarget(null);
    }

    public override void Deactivate()
    {
        m_gameplayController.Gameplay.TileRevealedWithSuccess -= HandleTileRevealedWithSuccess;
        m_gameplayController.Gameplay.TileRevealFailed -= HandleTileRevealFailed;
    }

    public override void NotifyTapped()
    {
        m_view.Hide();
    }

    private void HandleTileRevealedWithSuccess()
    {
        m_failsInARow = 0;
        m_totalSuccess++;

        if (m_totalSuccess == NumSuccessTilesToFinishTutorial)
            m_ctrl.NextStep();  // This will finish the tutorial
    }

    private void HandleTileRevealFailed()
    {
        SendFailureTrackingEvent();

        // If NumSuccessTilesToFinishTutorial tiles are revealed successfully, we are assuming that player know how to play
        if (m_totalSuccess >= NumSuccessTilesToFinishTutorial)
            return;

        m_failsInARow++;
        if (m_failsInARow == 3)
        {
            m_failsInARow = 0;
            m_view.m_paletteColorImage.color = m_gameplayController.Hud.m_palette.ActiveColor;
            m_view.Show();
        }
    }

    private void SendFailureTrackingEvent()
    {
        GameAnalyticsSDK.GameAnalytics.NewDesignEvent("tutorial.fail");
    }
}
