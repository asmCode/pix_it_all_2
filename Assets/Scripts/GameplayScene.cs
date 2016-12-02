using UnityEngine;
using System.Collections;

public class GameplayScene : MonoBehaviour
{
    public Hud m_hud;
    public Board m_board;
    public BoardController m_boardInputController;

    private Gameplay m_gameplay;
    private GameplayController m_gameplayController;

    private void Awake()
    {
        m_gameplay = new Gameplay();
        m_gameplay.Init();

        m_gameplayController = new GameplayController(
            m_gameplay,
            m_hud,
            m_board,
            m_boardInputController);
    }

    private void Start()
    {
        m_gameplayController.SetupGameplay();
    }
}
