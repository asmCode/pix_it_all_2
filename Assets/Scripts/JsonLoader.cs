using UnityEngine;

public class JsonLoader
{
    public static T LoadFromFile<T>(string path)
        where T : class
    {
        if (string.IsNullOrEmpty(path))
            return null;

        string data = FileReader.ReadAllText(path);
        if (data == null)
            return null;

        var deserializedObject = JsonUtility.FromJson<T>(data);

        return deserializedObject;
    }

    public static bool SaveToFile(string path, object data)
    {
        if (string.IsNullOrEmpty(path))
            return false;

        if (data == null)
            return false;

        var jsonCode = JsonUtility.ToJson(data);
        if (jsonCode == null)
            return false;

        System.IO.File.WriteAllText(path, jsonCode);

        return true;
    }
}
