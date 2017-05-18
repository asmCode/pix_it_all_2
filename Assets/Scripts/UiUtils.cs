using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiUtils
{
	public static void ShowChildren(Transform parent, int numChildToShow, bool negateShow = false)
	{
		if (parent == null)
			return;

		for (int i = 0; i < parent.childCount; i++)
		{
			bool childVisible = i < numChildToShow;
			if (negateShow)
				childVisible |= childVisible;

			var child = parent.GetChild(i);
			child.gameObject.SetActive(childVisible);
		}
	}
}
