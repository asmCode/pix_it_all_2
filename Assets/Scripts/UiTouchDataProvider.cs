using System;
using System.Collections;
using System.Collections.Generic;
using Ssg;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiTouchDataProvider : TouchDataProvider
{
    private List<Ssg.Touch> m_touches = new List<Ssg.Touch>();

    public override Ssg.Touch GetTouch(int index)
    {
        return m_touches[index];
    }

    public override int GetTouchCount()
    {
        return m_touches.Count;
    }

    private void Awake()
    {
        var eventTrigger = GetComponent<EventTrigger>();

        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((eventData) => { OnPointerDown(eventData as PointerEventData); });
        eventTrigger.triggers.Add(pointerDown);

        var pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((eventData) => { OnPointerUp(eventData as PointerEventData); });
        eventTrigger.triggers.Add(pointerUp);

        var drag = new EventTrigger.Entry();
        drag.eventID = EventTriggerType.Drag;
        drag.callback.AddListener((eventData) => { OnDrag(eventData as PointerEventData); });
        eventTrigger.triggers.Add(drag);
    }

    private void OnPointerDown(PointerEventData eventData)
    {
        m_touches.RemoveAll((t) => { return t.Id == eventData.pointerId; });
        var touch = new Ssg.Touch();
        touch.Id = eventData.pointerId;
        touch.Position = eventData.position;
        m_touches.Add(touch);
    }

    private void OnPointerUp(PointerEventData eventData)
    {
        m_touches.RemoveAll((t) => { return t.Id == eventData.pointerId; });
    }

    private void OnDrag(PointerEventData eventData)
    {
        var touch = m_touches.Find((t) => { return t.Id == eventData.pointerId; });
        if (touch == null)
            return;

        touch.Position = eventData.position;
        touch.Delta = eventData.delta;
    }
}
