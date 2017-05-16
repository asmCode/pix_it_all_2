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
}
