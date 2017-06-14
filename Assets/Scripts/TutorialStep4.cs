using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStep4 : TutorialStep
{
    private const int RedColorIndex = 0;
    
    private GameplayController m_gameplayController;

    public TutorialStep4(TutorialController ctrl, TutorialView view, GameplayController gameplayController) :
        base(ctrl, view)
    {
        m_gameplayController = gameplayController;
    }

    public override void Activate()
    {
        m_view.SetStep(3);

        Vector3[] corners = new Vector3[4];
        m_view.m_board.GetWorldCorners(corners);

        var leftBottomWorld = Vu.XY(m_view.m_board.TransformPoint(new Vector3(0, 3, 0)));
        var rightTopWorld = Vu.XY(m_view.m_board.TransformPoint(new Vector3(2, 5, 0)));
    
        var rect = new RectSides();
        rect.Left = corners[0].x;
        rect.Right = corners[2].x;
        rect.Top = corners[2].y;
        rect.Bottom = corners[0].y;

        var size = rightTopWorld - leftBottomWorld;
        size.x /= m_view.m_rootCanvas.transform.localScale.x;
        size.y /= m_view.m_rootCanvas.transform.localScale.y;

        m_view.SetIndicatorTarget(leftBottomWorld, size);
    }

    public override void Dectivate()
    {

    }

    public override void NotifyIndicatorTapped(Vector2 screenPoint)
    {
        int x;
        int y;
        if (!m_gameplayController.BoardController.ScreenPointToTile(screenPoint, out x, out y))
            return;

        m_gameplayController.HandleBoardTileTapped(x, y);

        if (m_gameplayController.Gameplay.ImageProgress.RevealedTiles >= 4)
            m_ctrl.NextStep();
    }
}
