using UnityEngine;
using UnityEngine.SceneManagement;

public class WellcomeScene : MonoBehaviour
{
    public WellcomeView m_wellcomeView;

    private WellcomeController m_controller;

    private void Awake()
    {
        m_controller = new WellcomeController();
        m_controller.Init(m_wellcomeView);
    }

    private void Start()
    {
        Fade.FadeOut(null, false, null);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            m_controller.HandleBackButton();
        }
    }
}
