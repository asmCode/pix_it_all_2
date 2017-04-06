using UnityEngine;

public class TouchProxy
{
    public class Touch
    {
        public Vector2 Position { get; set; }
        public Vector2 Delta { get; set; }
    }

    private const int MaxTouchCount = 4;

    private static int m_touchCount = 0;
    private static Touch[] m_touches = new Touch[MaxTouchCount];
    private static Vector2 m_lastMousePosition;
    private static bool m_isMouseDown = false;

    public static void Init()
    {
        for (int i = 0; i < MaxTouchCount; i++)
            m_touches[i] = new Touch();
    }

    public static void Update()
    {
        if (Input.touchSupported)
        {
            m_touchCount = Mathf.Min(Input.touchCount, MaxTouchCount);

            for (int i = 0; i < m_touchCount; i++)
            {
                var touch = Input.GetTouch(i);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        m_touches[i].Position = touch.position;
                        m_touches[i].Delta = Vector2.zero;
                        break;

                    case TouchPhase.Moved:
                        m_touches[i].Position = touch.position;
                        m_touches[i].Delta = touch.deltaPosition;
                        break;

                    case TouchPhase.Ended:
                        m_touches[i].Delta = Vector2.zero;
                        break;
                }
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                var mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                if (!m_isMouseDown)
                {
                    m_isMouseDown = true;
                    m_lastMousePosition = mousePosition;
                }
                m_touchCount = 1;
                m_touches[0].Position = mousePosition;
                m_touches[0].Delta = mousePosition - m_lastMousePosition;
                m_lastMousePosition = mousePosition;

                if (Input.GetKey(KeyCode.LeftControl))
                {
                    m_touchCount++;
                    var screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
                    var toScreenCenter = screenCenter - mousePosition;
                    m_touches[1].Position = mousePosition + 2 * toScreenCenter;
                    m_touches[1].Delta = -m_touches[0].Delta;
                }
            }
            else
            {
                m_isMouseDown = false;
                m_touchCount = 0;
            }
        }
    }

    public static Touch GetTouch(int index)
    {
        return m_touches[index];
    }

    public static int GetTouchCount()
    {
        return m_touchCount;
    }
}
