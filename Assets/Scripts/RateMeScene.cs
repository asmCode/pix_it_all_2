using UnityEngine;
using UnityEngine.SceneManagement;

public class RateMeScene : MonoBehaviour
{
    public RateMeView m_view;

    private static bool m_isVisible;
    private RateMeController m_controller;

    public static void Show()
    {
        SceneManager.LoadScene("RateMe", LoadSceneMode.Additive);

        var persistent = Pix.Game.GetInstance().Persistent;
        persistent.SetRateMeTimeWhenPresented(System.DateTime.Now);

        m_isVisible = true;
    }

    public static void Close()
    {
        m_isVisible = false;

        SceneManager.UnloadSceneAsync("RateMe");
    }

    public static RateMeController GetController()
    {
        var scene = GameObject.FindObjectOfType<RateMeScene>();
        if (scene == null)
            return null;

        return scene.m_controller;
    }

    public static bool IsVisible()
    {
        return m_isVisible;
    }

    private void Awake()
    {
        m_controller = new RateMeController();
        m_controller.Init(m_view);
    }
}
