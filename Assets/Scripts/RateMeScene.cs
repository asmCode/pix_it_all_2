using UnityEngine;
using UnityEngine.SceneManagement;

public class RateMeScene : MonoBehaviour
{
    public RateMeView m_view;

    private RateMeController m_controller;

    public static void Show()
    {
        SceneManager.LoadScene("RateMe", LoadSceneMode.Additive);

        var persistent = Game.GetInstance().Persistent;
        persistent.SetRateMeTimeWhenPresented(System.DateTime.Now);
    }

    private void Awake()
    {
        m_controller = new RateMeController();
        m_controller.Init(m_view);
    }
}
