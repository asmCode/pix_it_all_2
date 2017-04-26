using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings
{
	public static bool IsRestoreAvailable
	{
		get;
		private set;
	}

	static GameSettings()
	{		
		IsRestoreAvailable = true;
	}
}
