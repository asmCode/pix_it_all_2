using UnityEngine;
using UnityEngine.UI;

public class PenaltyView : MonoBehaviour
{
    public Text m_labelSeconds;

    private Animator m_animator;

    public void ShowPenalty(int seconds)
    {
        m_labelSeconds.text = string.Format("PENALTY {0}s", seconds);
        gameObject.SetActive(true);
        PlayAppearAnimation();
    }

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void PlayAppearAnimation()
    {
        m_animator.Play("PenaltyViewAppear", 0, 0);
    }

    public void AnimEvent_AnimFinished()
    {
        gameObject.SetActive(false);
    }
}
