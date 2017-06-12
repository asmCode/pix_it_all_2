using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    private static float FadeSpeed = 4.0f;
    private System.Action m_callback;

    private Image m_image;
    private float m_targetFadeValue;

    public static void FadeIn(Fade fade, bool blockInput, System.Action callback)
    {
        FadeInternal(fade, 1.0f, blockInput, callback);
    }

    public static void FadeOut(Fade fade,bool blockInput, System.Action callback)
    {
        FadeInternal(fade, 0.0f, blockInput, callback);
    }

    public static void SetFadeValue(Fade fade, bool blockInput, float fadeValue)
    {
        if (fade == null)
            fade = GetInstance();

        if (fade == null)
            return;

        fade.m_image.color = new Color(0, 0, 0, fadeValue);
        fade.BlockInput(blockInput);
        fade.m_targetFadeValue = fadeValue;
    }

    public static void FadeInternal(Fade fade, float targetFadeValue, bool blockInput, System.Action callback)
    {
        if (fade == null)
            fade = GetInstance();

        if (fade == null)
            return;

        if (fade.IsDuringFading())
            return;

        fade.BlockInput(blockInput);
        fade.m_image.color = new Color(0, 0, 0, 1 - targetFadeValue);
        fade.m_targetFadeValue = targetFadeValue;
        fade.m_callback = callback;
    }

    private void BlockInput(bool block)
    {
        m_image.raycastTarget = block;
    }

    private static Fade GetInstance()
    {
        var fadeObject = GameObject.Find("Fade");
        if (fadeObject == null)
            return null;

        return fadeObject.GetComponentInChildren<Fade>(true);
    }

    private bool IsDuringFading()
    {
        return m_image.color.a != m_targetFadeValue;
    }

    private void Awake()
    {
        m_image = GetComponent<Image>();
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.F))
        //     FadeIn(true, () => { Debug.Log("fade in"); });

        // if (Input.GetKeyDown(KeyCode.O))
        //     FadeOut(false, () => { Debug.Log("fade out"); });

        if (!IsDuringFading())
            return;

        float fadeValue = Mathf.MoveTowards(m_image.color.a, m_targetFadeValue, FadeSpeed * Time.deltaTime);

        m_image.color = new Color(0, 0, 0, fadeValue);

        if (m_image.color.a == m_targetFadeValue)
            NotifyFinished();
    }

    private void NotifyFinished()
    {
        if (m_callback != null)
            m_callback();
    }
}
