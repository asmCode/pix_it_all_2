﻿using UnityEngine;
using System.Collections;

public class PanGestureDetector : MonoBehaviour
{
    public event System.Action PanStarted;
    public event System.Action<Vector2, Vector2> PanMoved;
    public TouchDataProvider m_touchProvider;
    // Velocity
    public event System.Action<Vector2> PanEnded;

    private bool m_isPanning = false;
    private int m_numberOfTouches;
    private Vector2 m_lastPosition;
    private GestureVelocity m_gestureVelocity = new GestureVelocity();

    private void Update()
    {
        bool isTouching = IsTouching();

        if (!isTouching && !m_isPanning)
            return;

        if (!isTouching && m_isPanning)
        {
            m_isPanning = false;
            m_numberOfTouches = 0;
            OnPanEnded(m_gestureVelocity.GetVelocity(Time.time));
            return;
        }

        if (!m_isPanning)
        {
            m_isPanning = true;
            m_lastPosition = GetCurrentTouchPosition();
            m_gestureVelocity.Reset();
            m_gestureVelocity.Track(m_lastPosition, Time.time);
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
            m_gestureVelocity.Track(currentPosition, Time.time);
            OnPanMoved(currentPosition, delta);
        }
    }

    private void OnPanEnded(Vector2 velocity)
    {
        if (PanEnded != null)
            PanEnded(velocity);
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
        return m_touchProvider.GetTouchCount() > 0;
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
        return m_touchProvider.GetTouchCount();
    }

    private Vector2 GetAverageOfAllTouches()
    {
        Vector2 avg = Vector2.zero;

        for (int i = 0; i < m_touchProvider.GetTouchCount(); i++)
        {
            var touch = m_touchProvider.GetTouch(i);
            avg += touch.Position;
        }

        avg /= m_touchProvider.GetTouchCount();
        return avg;
    }
}
