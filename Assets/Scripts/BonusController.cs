using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusController
{
    private Gameplay m_gameplay;
    private BonusView m_bonusView;

    public void Init(Gameplay gameplay, BonusView bonusView)
    {
        m_bonusView = bonusView;
        m_gameplay = gameplay;

        m_gameplay.TileRevealedWithSuccess += HandleTileRevealedWithSuccess;
    }

    private void ShowBonus(int inRow)
    {
        int type = 0;
        switch (inRow)
        {
            case 5:
                type = 0;
                break;

            case 10:
                type = 1;
                break;

            case 20:
                type = 2;
                break;

			default:
				return;
        }
        m_bonusView.ShowBonus(type);
        m_gameplay.ApplyBonus(-m_gameplay.SuccessInRow * 0.5f);
    }

    private void HandleTileRevealedWithSuccess()
    {
        ShowBonus(m_gameplay.SuccessInRow);
    }
}
