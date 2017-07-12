using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsScene : MonoBehaviour
{
    private static bool m_duringGameplay;
    private static System.Action m_optionsClosedCallback;

    public OptionsView m_optionsView;
    // Options is a scene that is loaded additively, so it needs it's own fade.
    public Fade m_fade;

    private OptionsController m_optionsController;

    public static void Show(bool duringLevel, System.Action closed)
    {
        m_duringGameplay = duringLevel;
        m_optionsClosedCallback = closed;
        SceneManager.LoadScene("Options", LoadSceneMode.Additive);
    }

    private void Awake()
    {
        m_optionsController = new OptionsController();
        m_optionsController.Init(m_optionsView, m_fade, m_duringGameplay);
        m_optionsView.SetDevButtonsEnabled(GameSettings.DevBuild);
    }

    private void Start()
    {
        Fade.FadeOut(m_fade, false, null);
    }

    private void OnEnable()
    {
        SceneManager.sceneUnloaded += HandleSceneUnloaded;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            m_optionsController.HandleBackButton();
        }
    }

    private void HandleSceneUnloaded(Scene scene)
    {
        if (m_optionsClosedCallback != null)
            m_optionsClosedCallback();

        SceneManager.sceneUnloaded -= HandleSceneUnloaded;
    }
}
