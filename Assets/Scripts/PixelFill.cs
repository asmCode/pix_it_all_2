using UnityEngine;
using UnityEngine.UI;

public class PixelFill : MonoBehaviour
{
	private Image m_image;
	private AnimatedImage m_anim;
	private System.Action m_finishedCallback;

	public void Play(Color color, System.Action finishedCallback)
	{
		m_finishedCallback = finishedCallback;
		m_image.color = color;
		m_anim.Play();
	}

	private void Awake()
	{
		m_image = GetComponent<Image>();
		m_anim = GetComponent<AnimatedImage>();
	}

	private void OnEnable()
	{
		m_anim.Finished += HandleFinished;
	}

	private void OnDisable()
	{
		m_anim.Finished -= HandleFinished;
	}

	private void HandleFinished()
	{
		if (m_finishedCallback != null)
			m_finishedCallback();
	}
}
