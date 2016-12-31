using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Game : MonoBehaviourSingleton<Game, MonoBehaviourSingletonMeta>
{
    public ImageManager ImageManager
    {
        get;
        private set;
    }

    public void StartLevel(string bundleId, string imageId)
    {
        GameplayScene.m_selectedBundleId = bundleId;
        GameplayScene.m_selectedLevelId = imageId;

        SceneManager.LoadScene("Gameplay");
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
