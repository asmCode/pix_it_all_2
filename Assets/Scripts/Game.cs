using UnityEngine;
using System.Collections;

public class Game : MonoBehaviourSingleton<Game, MonoBehaviourSingletonMeta>
{
    public ImageManager ImageManager
    {
        get;
        private set;
    }

    protected override void Awake()
    {
        Application.targetFrameRate = 60;
        TouchProxy.Init();

        ImageManager = new ImageManager();
        ImageManager.LoadImages();
    }

	private void Update()
    {
        TouchProxy.Update();
	}
}
