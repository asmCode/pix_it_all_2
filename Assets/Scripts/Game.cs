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

    public PlayerProgress PlayerProgress
    {
        get;
        private set;
    }

    public void StartLevel(string bundleId, string imageId, bool continueLevel)
    {
        GameplayScene.m_selectedBundleId = bundleId;
        GameplayScene.m_selectedLevelId = imageId;
        GameplayScene.m_continueLevel = continueLevel;

        SceneManager.LoadScene("Gameplay");
    }

    protected override void Awake()
    {
        Application.targetFrameRate = 60;
        TouchProxy.Init();

        ImageManager = new ImageManager();
        ImageManager.LoadImages();

        PlayerProgress = new PlayerProgress();
    }

	private void Update()
    {
        TouchProxy.Update();
	}
}
