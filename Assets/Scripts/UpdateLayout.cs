using UnityEngine;
using UnityEngine.UI;

public class UpdateLayout : MonoBehaviour
{
    private RectTransform m_rectTransform;

    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        LayoutRebuilder.MarkLayoutForRebuild(m_rectTransform);
    }
}
