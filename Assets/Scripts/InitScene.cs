using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InitScene : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return InitBackend();

        InitDirectories();

        SceneManager.LoadScene("Levels");
    }

    private IEnumerator InitBackend()
    {
        Timeout timeout = new Timeout();

        timeout.Start(5.0f);
        while (!GameSparks.Core.GS.Available && !timeout.IsTimeout)
            yield return null;

        if (!GameSparks.Core.GS.Available)
        {
            Debug.Log("GameSparks: timeout when waiting for availability.");
            yield break;
        }

        bool authenticationDone = false;

        Debug.Log("GameSparks: authenticating...");

        GSpark.GetInstance().Authenticate((success) =>
        {
            Debug.LogFormat("GamesSpark: authentication: {0}", success);
            authenticationDone = true;
        });

        timeout.Start(5.0f);
        while (!authenticationDone && !timeout.IsTimeout)
            yield return null;

        if (!GSpark.GetInstance().IsAuthenticated)
        {
            Debug.Log("GameSparks: timeout when authenticating.");
        }
    }

    private void InitDirectories()
    {
        string path = Application.persistentDataPath + "/Bundles";
        if (!System.IO.Directory.Exists(path))
            System.IO.Directory.CreateDirectory(path);

        path = Application.persistentDataPath + "/progress";
        if (!System.IO.Directory.Exists(path))
            System.IO.Directory.CreateDirectory(path);
    }
}
