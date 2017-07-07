using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileProgress : MonoBehaviour
{
	public Text m_labelCurrent;
	public Text m_labelMax;
	public Slider m_progress;

	private float m_current;
	private float m_max;

	public void SetMax(int max)
	{
		m_labelMax.text = max.ToString();
		m_max = max;

		UpdateProgress();
	}

	public void SetCurrent(int current)
	{
		m_labelCurrent.text = current.ToString();
		m_current = current;

		UpdateProgress();
	}

	private void UpdateProgress()
	{
		if (m_max != 0.0f)
			m_progress.value = m_current / m_max;
		else
			m_progress.value = 0.0f;
	}
}
