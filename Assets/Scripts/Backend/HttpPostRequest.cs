using System.Collections;
using UnityEngine;

public class HttpPostRequest : MonoBehaviour
{
    public static bool Send(string url, string data, System.Action<bool, string> callback)
    {
        if (string.IsNullOrEmpty(url))
            return false;

        var gameObject = new GameObject("HttpPostRequest", typeof(HttpPostRequest));
        if (gameObject == null)
            return false;

        DontDestroyOnLoad(gameObject);

        var httpPostRequest = gameObject.GetComponent<HttpPostRequest>();
        httpPostRequest.SendInternal(url, data, callback);

        return true;
    }

    private void SendInternal(string url, string data, System.Action<bool, string> callback)
    {
        StartCoroutine(SendCoroutine(url, data, callback));
    }

    private IEnumerator SendCoroutine(string url, string data, System.Action<bool, string> callback)
    {
        byte[] dataBytes = null;
        if (data != null)
            dataBytes = System.Text.Encoding.ASCII.GetBytes(data);

        WWW www = new WWW(url, dataBytes);
        yield return www;

        bool success = string.IsNullOrEmpty(www.error);

        if (callback != null)
            callback(success, www.text);

        DestroyObject(gameObject);
    }
}
