using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialView : MonoBehaviour
{
    public RectTransform m_paletteButton;
    public RectTransform m_previewButton;
    public RectTransform m_board;
    public Transform m_paletteContainer;
    public TutorialIndicator m_indicator;
    public GameObject[] m_steps;

    public System.Action<Vector2> IndicatorTapped;
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
        m_indicator.m_target = target;
        m_indicator.gameObject.SetActive(target != null);
    }

    public void SetIndicatorTarget(Vector2 worldPosition, Vector2 size)
    {
        m_indicator.m_target = null;
        
        var rectTr = m_indicator.GetComponent<RectTransform>();
        rectTr.position = worldPosition;
        rectTr.sizeDelta = size;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UiEvent_IndicatorTapped(BaseEventData eventData)
    {
        var pointerEventData = eventData as PointerEventData;
        if (pointerEventData == null)
            return;

        if (IndicatorTapped != null)
            IndicatorTapped(pointerEventData.position);
    }

    public void UiEvent_Tapped()
    {
        if (Tapped != null)
            Tapped();
    }
}
