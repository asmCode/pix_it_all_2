using UnityEngine;
using System.Collections;

public class GameplayScene : MonoBehaviour
{
    public static string m_selectedBundleId;
    public static string m_selectedLevelId;
    public static bool m_continueLevel;

    public Hud m_hud;
    public PauseView m_pauseView;
    public SummaryView m_summaryView;
    public PenaltyView m_penaltyView;
    public BonusView m_bonusView;
    public Board m_board;
    public LevelIntroView m_levelIntroView;
    public BoardController m_boardInputController;

    private Gameplay m_gameplay;
    private GameplayController m_gameplayController;

    private void Awake()
    {
        SetupSceneInputVariables();

        m_gameplay = new Gameplay();
        m_gameplay.Init(m_selectedBundleId, m_selectedLevelId, m_continueLevel);

        m_gameplayController = new GameplayController(
            m_gameplay,
            m_hud,
            m_pauseView,
            m_summaryView,
            m_penaltyView,
            m_bonusView,
            m_board,
            m_boardInputController,
            m_levelIntroView);
    }

    private void Start()
    {
        m_gameplayController.SetupGameplay();
    }

    private void Update()
    {
        m_gameplayController.Update(Time.deltaTime);

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            m_gameplayController.HandleBackButton();
        }
    }

    private void SetupSceneInputVariables()
    {
        if (string.IsNullOrEmpty(m_selectedBundleId) ||
            string.IsNullOrEmpty(m_selectedLevelId))
        {
            m_selectedBundleId = "extra";
            m_selectedLevelId = "motorowka";
            m_continueLevel = true;
        }
    }

    private void OnApplicationPause(bool paused)
    {
        m_gameplayController.NotifyOnApplicationPause(paused);
    }
}
