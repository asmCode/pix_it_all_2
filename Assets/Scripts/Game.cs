using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
        TouchProxy.Init();
    }

	private void Update()
    {
        TouchProxy.Update();
	}
}
