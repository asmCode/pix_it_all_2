using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class sd : MonoBehaviour
{
    public Board m_board;
    [Range(0.0f, 1.0f)]
    public float m_zoom;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        m_board.SetZoom(new Vector2(0, 0), m_zoom);
    }

    public void cipka(BaseEventData d)
    {
        var e = d as PointerEventData;
        Debug.LogFormat("{0}, {1}", e.position.x, e.position.y);
        //Debug.LogFormat("{0}", d.GetType().ToString());
    }
}
