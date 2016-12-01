using UnityEngine;
using System.Collections;

public class TapGestureDetector : MonoBehaviour
{
    // Default is 3 pixels on screen with the width 640.
    public float m_panThreshold = 3.0f / 640.0f;

    public event System.Action<Vector2> Tapped;

    private Vector2 m_touchStartPosition;
    private Vector2 m_touchEndPosition;
    private bool m_isTouching;
    private bool m_isTapValid;

    private void Update()
    {
        int touchCount = TouchProxy.GetTouchCount();

        if (touchCount > 1)
        {
            m_isTapValid = false;
            m_isTouching = true;
            return;
        }

        if (m_isTouching && touchCount == 0)
        {
            m_isTouching = false;
            if (m_isTapValid)
                OnTapped(m_touchEndPosition);
            m_isTapValid = false;
            return;
        }

        if (!m_isTouching && touchCount == 1)
        {
            var touch = TouchProxy.GetTouch(0);
            m_isTouching = true;
            m_isTapValid = true;
            m_touchStartPosition = touch.Position;
            m_touchEndPosition = touch.Position;
            return;
        }

        if (m_isTouching && touchCount == 1)
        {
            var touch = TouchProxy.GetTouch(0);
            m_touchEndPosition = touch.Position;
            if (!IsLessThanThreshold())
                m_isTapValid = false;
            return;
        }
    }

    private void OnTapped(Vector2 position)
    {
        if (Tapped != null)
            Tapped(position);
    }

    private bool IsLessThanThreshold()
    {
        float screenDiag = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height);
        float pixelsMoved = (m_touchStartPosition - m_touchEndPosition).magnitude;
        float ratio = pixelsMoved / screenDiag;
        return ratio <= m_panThreshold;
    }
}
