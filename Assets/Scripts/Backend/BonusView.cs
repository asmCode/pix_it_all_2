using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusView : MonoBehaviour
{
    public Image m_image;
	public RectTransform m_content;

    public Sprite m_5inARow;
    public Sprite m_10inARow;
    public Sprite m_20inARow;

	private Animator m_animator;

    public void ShowBonus(int type)
    {
        switch (type)
        {
            case 0:
                m_image.sprite = m_5inARow;
                break;

            case 1:
                m_image.sprite = m_10inARow;
                break;

            case 2:
                m_image.sprite = m_20inARow;
                break;
        }

		m_content.gameObject.SetActive(true);
		m_image.SetNativeSize();
		m_animator.Play("Appear", 0, 0.0f);
    }

	private void Awake()
	{
		m_animator = m_content.GetComponent<Animator>();

		m_content.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			ShowBonus(0);
		}

		if (Input.GetKeyDown(KeyCode.B))
		{
			ShowBonus(1);
		}

		if (Input.GetKeyDown(KeyCode.C))
		{
			ShowBonus(2);
		}
	}
}
