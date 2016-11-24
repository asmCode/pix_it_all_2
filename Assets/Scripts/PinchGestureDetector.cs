using UnityEngine;
using System.Collections;
using System;

public class PinchGestureDetector : MonoBehaviour
{
    public event System.Action<float> PinchChanged;

    private bool m_isPinching = false;

    private void Update()
    {
        if (Input.touchCount < 2 && !m_isPinching)
            return;

        if (Input.touchCount < 2 && m_isPinching)
        {
            m_isPinching = false;
            return;
        }

        if (!m_isPinching)
        {
            m_isPinching = true;
        }

        var touch0 = Input.GetTouch(0);
        var touch1 = Input.GetTouch(1);

        if (touch0.deltaPosition != Vector2.zero ||
            touch1.deltaPosition != Vector2.zero)
        {
            float delta = CalculatePinchDelta(touch0, touch1);
            OnPinchChanged(delta);
        }
    }

    private float CalculatePinchDelta(Touch touch0, Touch touch1)
    {
        var t0OldPos = touch0.position - touch0.deltaPosition;
        var t1OldPos = touch1.position - touch1.deltaPosition;
        var oldDistance = (t0OldPos - t1OldPos).magnitude;
        var newDistance = (touch0.position - touch1.position).magnitude;
        return newDistance - oldDistance;
    }

    private void OnPinchChanged(float delta)
    {
        if (PinchChanged != null)
            PinchChanged(delta);
    }
}
