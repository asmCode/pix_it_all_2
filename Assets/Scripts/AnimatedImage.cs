using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedImage : MonoBehaviour
{
	public float m_fps;
	public Sprite[] m_frames;

	public System.Action Finished;

	private Image m_image;
	private bool m_isPlaying;
	private float m_currentFrame;

	public void Play()
	{
		m_isPlaying = true;
		m_currentFrame = 0.0f;
        m_image.sprite = m_frames[0];
    }

	private void Awake()
	{
		m_image = GetComponent<Image>();
	}
	
	private void Update()
	{
		if (!m_isPlaying)
			return;

		int currentFrameInt = (int)m_currentFrame;
		if (currentFrameInt >= m_frames.Length)
		{
			m_isPlaying = false;
			if (Finished != null)
				Finished();

			return;
		}

		m_image.sprite = m_frames[currentFrameInt];

		m_currentFrame += Time.deltaTime * m_fps;
	}
}
