using UnityEngine;
using System.Collections;

public class BoardController : MonoBehaviour
{
    private const float PinchDelay = 0.15f;

    public event System.Action<int, int> BoardTileTapped;

    public PanGestureDetector m_panGestureDetector;
    public PinchGestureDetector m_pinchGestureDetector;
    public TapGestureDetector m_tapGestureDetector;

    private Board m_board;
    private Vector2 m_boardDstPositionVelocity;
    private Vector2 m_inertiaVelocity;

    private const float DecelerationNormal = 10000.0f;
    private const float DecelerationOffScreen = 1000000.0f;

    private float m_timeFromLastPinch = 0.0f;
    private bool m_ignoreNextTap;

    private void Awake()
    {
        Debug.LogFormat("dpi = {0}", Screen.dpi);

        m_board = GetComponent<Board>();
    }

    private void Update()
    {
        if (TouchProxy.GetTouchCount() == 0)
            UpdateSpring();
    }

    private void UpdateSpring()
    {
        var parentRect = m_board.GetParentRect();
        var imageRect = m_board.GetImageRect();

        var imagePos = imageRect.Center;
        var imageOffset = new Vector2();

        // Hori
        if (imageRect.Width <= parentRect.Width)
        {
            imageOffset.x = parentRect.CenterHori - imageRect.CenterHori;
        }
        else
        {
            if (imageRect.Left > parentRect.Left)
                imageOffset.x = parentRect.Left - imageRect.Left;
            else if (imageRect.Right < parentRect.Right)
                imageOffset.x = parentRect.Right - imageRect.Right;
        }

        // Vert
        if (imageRect.Height <= parentRect.Height)
        {
            imageOffset.y = parentRect.CenterVert - imageRect.CenterVert;
        }
        else
        {
            if (imageRect.Bottom > parentRect.Bottom)
                imageOffset.y = parentRect.Bottom - imageRect.Bottom;
            else if (imageRect.Top < parentRect.Top)
                imageOffset.y = parentRect.Top - imageRect.Top;
        }

        var boardDstPosition = imagePos + imageOffset;

        var position = m_board.GetPosition();

        var sampleInteriaDeceleration = -m_inertiaVelocity.normalized;

        if (Mathf.Round(imageOffset.x) != 0.0f)
            sampleInteriaDeceleration.x *= DecelerationOffScreen;
        else
            sampleInteriaDeceleration.x *= DecelerationNormal;

        if (Mathf.Round(imageOffset.y) != 0.0f)
            sampleInteriaDeceleration.y *= DecelerationOffScreen;
        else
            sampleInteriaDeceleration.y *= DecelerationNormal;

        sampleInteriaDeceleration *= Time.deltaTime;

        if (Mathf.Abs(sampleInteriaDeceleration.x) >= Mathf.Abs(m_inertiaVelocity.x))
        {
            // if (m_inertiaVelocity.x != 0.0f)
            //     Debug.LogFormat("[{0}] m_inertiaVelocity.x = {1}, sampleInteriaDeceleration.x = {2}, imageOffset.x = {3}", Time.frameCount, m_inertiaVelocity.x, sampleInteriaDeceleration.x, imageOffset.x);

            m_inertiaVelocity.x = 0.0f;
        }
        else
        {
            m_inertiaVelocity.x += sampleInteriaDeceleration.x;
        }

        if (Mathf.Abs(sampleInteriaDeceleration.y) >= Mathf.Abs(m_inertiaVelocity.y))
        {
            // if (m_inertiaVelocity.y != 0.0f)
            //     Debug.LogFormat("[{0}] m_inertiaVelocity.y = {1}, sampleInteriaDeceleration.y = {2}, imageOffset.x = {3}", Time.frameCount, m_inertiaVelocity.y, sampleInteriaDeceleration.y, imageOffset.y);

            m_inertiaVelocity.y = 0.0f;
        }
        else
        {
            m_inertiaVelocity.y += sampleInteriaDeceleration.y;
        }

        position += m_inertiaVelocity * Time.deltaTime;

        if (m_inertiaVelocity.x == 0.0f)
        {
            position.x = Mathf.SmoothDamp(position.x, boardDstPosition.x, ref m_boardDstPositionVelocity.x, 0.1f, float.MaxValue, Time.deltaTime);
            //position = Vector2.SmoothDamp(position, boardDstPosition, ref m_boardDstPositionVelocity, 0.1f, float.MaxValue, Time.deltaTime);
        }

        if (m_inertiaVelocity.y == 0.0f)
        {
            position.y = Mathf.SmoothDamp(position.y, boardDstPosition.y, ref m_boardDstPositionVelocity.y, 0.1f, float.MaxValue, Time.deltaTime);
            //position = Vector2.SmoothDamp(position, boardDstPosition, ref m_boardDstPositionVelocity, 0.1f, float.MaxValue, Time.deltaTime);
        }

        m_board.SetPosition(position);
    }

    private void OnEnable()
    {
        m_panGestureDetector.PanMoved += HandlePanMoved;
        m_panGestureDetector.PanEnded += HandlePanEnded;
        m_pinchGestureDetector.PinchChanged += HandlePinchChanged;
        m_tapGestureDetector.TapStarted += HandleTapStarted;
        m_tapGestureDetector.Tapped += HandleTapped;
    }

    private void OnDisable()
    {
        m_panGestureDetector.PanMoved -= HandlePanMoved;
        m_panGestureDetector.PanEnded -= HandlePanEnded;
        m_pinchGestureDetector.PinchChanged -= HandlePinchChanged;
        m_tapGestureDetector.TapStarted -= HandleTapStarted;
        m_tapGestureDetector.Tapped -= HandleTapped;
    }

    private void HandlePanMoved(Vector2 position, Vector2 delta)
    {
        var parentRect = m_board.GetParentRect();
        var imageRect = m_board.GetImageRect();

        if (!imageRect.IsInsideHori(parentRect))
            delta.x *= 0.2f;

        if (!imageRect.IsInsideVert(parentRect))
            delta.y *= 0.2f;

        m_board.ChangeLocalPosition(delta);
    }

    private void HandlePanEnded(Vector2 velocity)
    {
        float timeElapsedSinceLastPinch = Time.time - m_timeFromLastPinch;

        if (timeElapsedSinceLastPinch <= PinchDelay &&
            m_panGestureDetector.TouchDataProvider.GetTouchCount() == 0)
        {
            // Debug.LogFormat("timeElapsedSinceLastPinch = {0}. Skipping HandlePanEnded", timeElapsedSinceLastPinch);
            return;
        }

        m_inertiaVelocity = velocity;
    }

    private void HandlePinchChanged(Vector2 pivot, float scale)
    {
        m_timeFromLastPinch = Time.time;

        m_board.Scale(pivot, scale);
    }

    private void HandleTapStarted()
    {
        if (m_inertiaVelocity != Vector2.zero)
        {
            m_ignoreNextTap = true;
            m_inertiaVelocity = Vector2.zero;
        }
        else
            m_ignoreNextTap = false;
    }

    private void HandleTapped(Vector2 position)
    {
        if (m_ignoreNextTap)
        {
            m_ignoreNextTap = false;
            return;
        }

        Vector2 localPoint;
        if (RectTransformUtility.RectangleContainsScreenPoint(m_board.m_parentRectTransform, position) &&
            RectTransformUtility.RectangleContainsScreenPoint(m_board.RectTransform, position) &&
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_board.RectTransform, position, null, out localPoint))
        {
            int x = (int)(localPoint.x + m_board.RectTransform.rect.width / 2);
            int y = (int)(localPoint.y + m_board.RectTransform.rect.height / 2);
            if (BoardTileTapped != null)
                BoardTileTapped(x, y);
        }
    }
}
