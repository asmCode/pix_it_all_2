using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
    private void Awake()
    {
        TouchProxy.Init();
    }

	private void Update()
    {
        TouchProxy.Update();
	}
}
