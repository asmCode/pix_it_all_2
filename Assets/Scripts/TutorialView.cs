using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialView : MonoBehaviour
{
    public RectTransform m_paletteButton;
    public RectTransform m_previewButton;
    public RectTransform m_boardImage;
    public Transform m_paletteContainer;
    public TutorialIndicator m_indicator;
    public GameObject[] m_steps;

    public System.Action IndicatorTapped;
    public System.Action Tapped;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetStep(int step)
    {
        foreach (var item in m_steps)
            item.SetActive(false);

        m_steps[step].SetActive(true);
    }

    public void SetIndicatorTarget(RectTransform target)
    {
        if (target != null)
        {
            m_indicator.m_target = target;
            m_indicator.gameObject.SetActive(true);
        }
        else
            m_indicator.gameObject.SetActive(false);
    }

    public void SetIndicatorTarget(Vector2 worldPosition, Vector2 size)
    {

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UiEvent_IndicatorTapped()
    {
        if (IndicatorTapped != null)
            IndicatorTapped();
    }

    public void UiEvent_Tapped()
    {
        if (Tapped != null)
            Tapped();
    }
}
