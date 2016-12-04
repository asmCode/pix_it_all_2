using UnityEngine;
using System.Collections;

public class BoardController : MonoBehaviour
{
    public event System.Action<int, int> BoardTileTapped;

    public PanGestureDetector m_panGestureDetector;
    public PinchGestureDetector m_pinchGestureDetector;
    public TapGestureDetector m_tapGestureDetector;

    private Board m_board;

    private void Awake()
    {
        Debug.LogFormat("dpi = {0}", Screen.dpi);

        m_board = GetComponent<Board>();
    }

    private void OnEnable()
    {
        m_panGestureDetector.PanMoved += HandlePanMoved;
        m_pinchGestureDetector.PinchChanged += HandlePinchChanged;
        m_tapGestureDetector.Tapped += HandleTapped;
    }

    // dupa
    private void OnDisable()
    {
        m_panGestureDetector.PanMoved -= HandlePanMoved;
        m_pinchGestureDetector.PinchChanged -= HandlePinchChanged;
        m_tapGestureDetector.Tapped -= HandleTapped;
    }

    private void HandlePanMoved(Vector2 position, Vector2 delta)
    {
        m_board.ChangeLocalPosition(delta);
    }

    private void HandlePinchChanged(Vector2 pivot, float delta)
    {
        m_board.ChangeZoom(pivot, delta);
    }

    private void HandleTapped(Vector2 position)
    {
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
