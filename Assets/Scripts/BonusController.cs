using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusController
{
    private Gameplay m_gameplay;
    private BonusView m_bonusView;
    private Hud m_hud;

    public void Init(Gameplay gameplay, BonusView bonusView, Hud hud)
    {
        m_bonusView = bonusView;
        m_gameplay = gameplay;
        m_hud = hud;

        m_gameplay.TileRevealedWithSuccess += HandleTileRevealedWithSuccess;
    }

    private void ShowBonus(int inRow)
    {
        int type = 0;
        Sound bonusSound = null;

        switch (inRow)
        {
            case 5:
                type = 0;
                bonusSound = AudioManager.GetInstance().SoundBonus1;
                break;

            case 10:
                type = 1;
                bonusSound = AudioManager.GetInstance().SoundBonus2;
                break;

            case 20:
                type = 2;
                bonusSound = AudioManager.GetInstance().SoundBonus3;
                break;

			default:
				return;
        }
        m_bonusView.ShowBonus(type);
        bonusSound.Play();

        float bonusTime = -m_gameplay.SuccessInRow * 0.5f;

        m_hud.ShowBonus(bonusTime);
        m_gameplay.ApplyBonus(bonusTime);
    }

    private void HandleTileRevealedWithSuccess()
    {
        ShowBonus(m_gameplay.SuccessInRow);
    }
}
