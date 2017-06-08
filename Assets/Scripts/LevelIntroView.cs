using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelIntroView : MonoBehaviour
{
    public System.Action Finished;

    public float m_boardPreviewValue;
    public bool m_hudVisible;

    public void Show(ImageViewData imageViewData)
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Anim_NotifyAnimationEnded()
    {
        if (Finished != null)
            Finished();
    }
}
