using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelFillEffect 
{
	private PixelFill m_pixelFillPrefab;
	private RectTransform m_container;

	public void Init(PixelFill pixelFillPrefab, RectTransform container)
	{
		m_pixelFillPrefab = pixelFillPrefab;
		m_container = container;
	}

	public void Show(int x, int y, Color color)
	{
		var pixelFill = CreatePixelFill();
		pixelFill.transform.SetParent(m_container);
		pixelFill.transform.localPosition = new Vector3(x, y, 0.0f);
		pixelFill.Play(color, null);
	}

	private PixelFill CreatePixelFill()
	{
		var obj = GameObject.Instantiate(m_pixelFillPrefab);
		return obj;
	}
}
