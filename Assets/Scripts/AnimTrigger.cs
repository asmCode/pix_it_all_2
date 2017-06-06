using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTrigger : MonoBehaviour
{
    public string m_animName = "";
    public bool m_triggerAnim2 = false;

    private Animator m_animator;
    private bool m_triggerred = false;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!m_triggerAnim2 || m_triggerred)
            return;

        m_triggerred = true;

        m_animator.Play(m_animName, 0, 0.0f);
    }
}
