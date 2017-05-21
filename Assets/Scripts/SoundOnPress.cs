using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundOnPress : MonoBehaviour
{
	private Button m_button;

	private void Awake()
	{
		m_button = GetComponent<Button>();
	}

	private void OnEnable()
	{
		m_button.onClick.AddListener(PlaySound);
	}

	private void OnDisable()
	{
		m_button.onClick.RemoveListener(PlaySound);
	}

	private void PlaySound()
	{
		AudioManager.GetInstance().SoundButton.Play();
	}
}
