using UnityEngine;
using System.Collections;

public class TapGestureDetector : MonoBehaviour
{
    private const float m_tapInchThreshold = 0.06f;

    public event System.Action<Vector2> Tapped;
    public event System.Action TapStarted;

    public TouchDataProvider m_touchProvider;

    private Vector2 m_touchStartPosition;
    private Vector2 m_touchEndPosition;
    private bool m_isTouching;
    private bool m_isTapValid;


    private void Update()
    {
        int touchCount = m_touchProvider.GetTouchCount();

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
                OnTapped(m_touchStartPosition);
            m_isTapValid = false;
            return;
        }

        if (!m_isTouching && touchCount == 1)
        {
            if (!m_isTouching)
                OnTapStarted();

            var touch = m_touchProvider.GetTouch(0);
            m_isTouching = true;
            m_isTapValid = true;
            m_touchStartPosition = touch.Position;
            m_touchEndPosition = touch.Position;
            return;
        }

        if (m_isTouching && touchCount == 1)
        {
            var touch = m_touchProvider.GetTouch(0);
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

    private void OnTapStarted()
    {
        if (TapStarted != null)
            TapStarted();
    }

    private bool IsLessThanThreshold()
    {
        float screenDiag = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height);
        float pixelsMoved = (m_touchStartPosition - m_touchEndPosition).magnitude;
        float inchesMoved = pixelsMoved / Screen.dpi;
        return inchesMoved <= m_tapInchThreshold;
    }
}
