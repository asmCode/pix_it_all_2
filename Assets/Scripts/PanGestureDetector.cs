using UnityEngine;
using System.Collections;

public class PanGestureDetector : MonoBehaviour
{
    public event System.Action PanStarted;
    public event System.Action<Vector2, Vector2> PanMoved;
    public event System.Action PanEnded;

    private bool m_isPanning = false;
    private int m_numberOfTouches;
    private Vector2 m_lastPosition;

    private void Update()
    {
        bool isTouching = IsTouching();

        if (!isTouching && !m_isPanning)
            return;

        if (!isTouching && m_isPanning)
        {
            m_isPanning = false;
            m_numberOfTouches = 0;
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

        if (NumberOfTouchsChanged())
        {
            m_numberOfTouches = GetNumberOfTouches();
            m_lastPosition = GetCurrentTouchPosition();
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
        return TouchProxy.GetTouchCount() > 0;
    }

    private Vector2 GetCurrentTouchPosition()
    {
        return GetAverageOfAllTouches();
    }

    private bool NumberOfTouchsChanged()
    {
        return m_numberOfTouches != GetNumberOfTouches();
    }

    private int GetNumberOfTouches()
    {
        return TouchProxy.GetTouchCount();
    }

    private Vector2 GetAverageOfAllTouches()
    {
        Vector2 avg = Vector2.zero;

        for (int i = 0; i < TouchProxy.GetTouchCount(); i++)
        {
            var touch = TouchProxy.GetTouch(i);
            avg += touch.Position;
        }

        avg /= TouchProxy.GetTouchCount();
        return avg;
    }
}
