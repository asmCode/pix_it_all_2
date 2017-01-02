using UnityEngine;

public class FileReader
{
    public static string ReadAllText(string path)
    {
        if (string.IsNullOrEmpty(path))
            return null;

        if (path.Contains("://"))
            return ReadAllTextUrl(path);
        else
            return ReadAllTextNomral(path);
    }

    private static string ReadAllTextNomral(string path)
    {
        if (!System.IO.File.Exists(path))
            return null;

        return System.IO.File.ReadAllText(path);
    }

    private static string ReadAllTextUrl(string path)
    {
        WWW www = new WWW(path);

        while (!www.isDone) {}

        if (!string.IsNullOrEmpty(www.error))
            return null;

        return www.text;
    }
}
