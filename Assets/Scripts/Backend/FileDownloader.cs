using System.Collections;
using UnityEngine;

public class FileDownloader : MonoBehaviour
{
    public static void Download(string url, string dstPath, System.Action<bool> callback)
    {
        if (!Validate(url, dstPath))
        {
            if (callback != null)
                callback(false);

            return;
        }

        var gameObject = new GameObject("FileDownloader", typeof(FileDownloader));
        DontDestroyOnLoad(gameObject);

        var fileDownloader = gameObject.GetComponent<FileDownloader>();
        fileDownloader.DownloadInternal(url, dstPath, callback);
    }

    private static bool Validate(string url, string dstPath)
    {
        if (string.IsNullOrEmpty(url) ||
            string.IsNullOrEmpty(dstPath))
            return false;

        var directory = System.IO.Path.GetDirectoryName(dstPath);
        if (!System.IO.Directory.Exists(directory))
            return false;

        return true;
    }

    private void DownloadInternal(string url, string dstPath, System.Action<bool> callback)
    {
        StartCoroutine(DownloadCoroutine(url, dstPath, callback));
    }

    private IEnumerator DownloadCoroutine(string url, string dstPath, System.Action<bool> callback)
    {
        WWW www = new WWW(url);
        yield return www;

        bool success = string.IsNullOrEmpty(www.error);

        if (success)
            System.IO.File.WriteAllBytes(dstPath, www.bytes);

        if (callback != null)
            callback(success);

        DestroyObject(gameObject);
    }
}
