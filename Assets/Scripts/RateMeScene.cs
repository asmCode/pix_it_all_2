using UnityEngine;
using UnityEngine.SceneManagement;

public class RateMeScene : MonoBehaviour
{
    public RateMeView m_view;

    private RateMeController m_controller;

    public static void Show()
    {
        SceneManager.LoadScene("RateMe", LoadSceneMode.Additive);
    }

    private void Awake()
    {
        m_controller = new RateMeController();
        m_controller.Init(m_view);
    }
}
