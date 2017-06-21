using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelFillEffect 
{
    private Pool<PixelFill> m_pixelFillPool;

	public void Init(PixelFill pixelFillPrefab, RectTransform container)
	{
        m_pixelFillPool = new Pool<PixelFill>(pixelFillPrefab, 10, container, null);
    }

	public void Show(int x, int y, Color color, System.Action finishedCallback)
	{
        var pixelFill = m_pixelFillPool.Get();
        if (pixelFill == null)
        {
            if (finishedCallback != null)
                finishedCallback();

            return;
        }

        float halfTileOffset = 0.5f;
		pixelFill.transform.localPosition = new Vector3(x + halfTileOffset, y + halfTileOffset, 0.0f);
        pixelFill.transform.localScale = GetRandomScale(Mathf.Abs(pixelFill.transform.localScale.x));
        pixelFill.Play(color, () =>
        {
            if (finishedCallback != null)
                finishedCallback();

            pixelFill.gameObject.SetActive(false);
        });
	}

    private Vector3 GetRandomScale(float baseScale)
    {
        float x = Random.Range(0, 2) == 0 ? 1.0f : -1.0f;
        float y = Random.Range(0, 2) == 0 ? 1.0f : -1.0f;

        return new Vector3(baseScale * x, baseScale * y, 1.0f);
    }
}
