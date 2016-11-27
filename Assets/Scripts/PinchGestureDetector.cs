using UnityEngine;
using System.Collections;
using System;

public class PinchGestureDetector : MonoBehaviour
{
    public event System.Action<Vector2, float> PinchChanged;

    private bool m_isPinching = false;

    private void Update()
    {
        if (TouchProxy.GetTouchCount() < 2 && !m_isPinching)
            return;

        if (TouchProxy.GetTouchCount() < 2 && m_isPinching)
        {
            m_isPinching = false;
            return;
        }

        if (!m_isPinching)
        {
            m_isPinching = true;
        }

        var touch0 = TouchProxy.GetTouch(0);
        var touch1 = TouchProxy.GetTouch(1);

        if (touch0.Delta != Vector2.zero ||
            touch1.Delta != Vector2.zero)
        {
            float delta = CalculatePinchDelta(touch0, touch1);
            var pivot = (touch0.Position + touch1.Position) / 2.0f;
            OnPinchChanged(pivot, delta);
        }
    }

    private float CalculatePinchDelta(TouchProxy.Touch touch0, TouchProxy.Touch touch1)
    {
        var t0OldPos = touch0.Position - touch0.Delta;
        var t1OldPos = touch1.Position - touch1.Delta;
        var oldDistance = (t0OldPos - t1OldPos).magnitude;
        var newDistance = (touch0.Position - touch1.Position).magnitude;
        return newDistance - oldDistance;
    }

    private void OnPinchChanged(Vector2 pivot, float delta)
    {
        if (PinchChanged != null)
            PinchChanged(pivot, delta);
    }
}
