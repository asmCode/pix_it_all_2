using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsScene : MonoBehaviour
{
    private static bool m_duringGameplay;

    public OptionsView m_optionsView;

    private OptionsController m_optionsController;

    public static void Show(bool duringLevel)
    {
        m_duringGameplay = duringLevel;
        SceneManager.LoadScene("Options", LoadSceneMode.Additive);
    }

    private void Awake()
    {
        m_optionsController = new OptionsController();
        m_optionsController.Init(m_optionsView, m_duringGameplay);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            m_optionsController.HandleBackButton();
        }
    }
}
