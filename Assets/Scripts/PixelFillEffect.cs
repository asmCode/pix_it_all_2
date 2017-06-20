using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelFillEffect 
{
	private AnimatedImage m_pixelFillPrefab;
	private RectTransform m_container;

	public void Init(AnimatedImage pixelFillPrefab, RectTransform container)
	{
		m_pixelFillPrefab = pixelFillPrefab;
		m_container = container;
	}

	public void Show(int x, int y)
	{
		var pixelFill = CreatePixelFill();
		pixelFill.transform.SetParent(m_container);
		pixelFill.transform.localPosition = new Vector3(x, y, 0.0f);
		pixelFill.Play();
	}

	private AnimatedImage CreatePixelFill()
	{
		var obj = GameObject.Instantiate(m_pixelFillPrefab);
		return obj;
	}
}
