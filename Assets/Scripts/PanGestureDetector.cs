using UnityEngine;
using System.Collections;

public class PanGestureDetector : MonoBehaviour
{
    public event System.Action PanStarted;
    public event System.Action<Vector2, Vector2> PanMoved;
    public event System.Action PanEnded;

    private bool m_isPanning = false;
    private Vector2 m_lastPosition;

    private void Update()
    {
        bool isTouching = IsTouching();

        if (!isTouching && !m_isPanning)
            return;

        if (!isTouching && m_isPanning)
        {
            m_isPanning = false;
            OnPanEnded();
            return;
        }

        if (!m_isPanning)
        {
            m_isPanning = true;
            m_lastPosition = GetCurrentTouchPosition();
            OnPanStarted();
            return;
        }

        var currentPosition = GetCurrentTouchPosition();
        var delta = currentPosition - m_lastPosition;
        m_lastPosition = currentPosition;

        if (delta != Vector2.zero)
        {
            OnPanMoved(currentPosition, delta);
        }
    }

    private void OnPanEnded()
    {
        if (PanEnded != null)
            PanEnded();
    }

    private void OnPanStarted()
    {
        if (PanStarted != null)
            PanStarted();
    }

    private void OnPanMoved(Vector2 position, Vector2 delta)
    {
        if (PanMoved != null)
            PanMoved(position, delta);
    }

    private bool IsTouching()
    {
        if (Input.touchSupported)
            return Input.touchCount > 0;
        else
        {
            return Input.GetMouseButton(0);
        }
    }

    public Vector2 GetCurrentTouchPosition()
    {
        if (Input.touchSupported)
            if (Input.touchCount > 0)
                return Input.GetTouch(0).position;
            else
                return Vector2.zero;
        else
        {
            if (Input.GetMouseButton(0))
                return Input.mousePosition;
            else
                return Vector2.zero;
        }
    }

    public bool NumberOfTouchedChanged()
    {
        return false;
    }
}
