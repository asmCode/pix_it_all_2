using UnityEngine;
using System.Collections;

public class BoardController : MonoBehaviour
{
    public PanGestureDetector m_panGestureDetector;
    public PinchGestureDetector m_pinchGestureDetector;

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
    }

    private void OnDisable()
    {
        m_panGestureDetector.PanMoved -= HandlePanMoved;
        m_pinchGestureDetector.PinchChanged -= HandlePinchChanged;
    }

    private void HandlePanMoved(Vector2 position, Vector2 delta)
    {
        m_board.ChangeLocalPosition(delta);
    }

    private void HandlePinchChanged(float delta)
    {
        m_board.ChangeZoom(delta);
    }
}
