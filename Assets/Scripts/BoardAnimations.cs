using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardAnimations : MonoBehaviour
{
	private Animator m_animator;

	private void Awake()
	{
		m_animator = GetComponent<Animator>();
	}

	public void PlayFail()
	{
		m_animator.Play("BoardFail", 0, 0.0f);
	}

	public void PlaySuccess()
	{
		m_animator.Play("BoardSuccess", 0, 0.0f);
	}
}
