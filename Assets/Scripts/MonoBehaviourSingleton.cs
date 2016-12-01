using UnityEngine;
using System.Collections;

public class MonoBehaviourSingleton<T, TMeta> : MonoBehaviour
    where T : MonoBehaviour
    where TMeta : MonoBehaviourSingletonMeta, new()
{
    private bool m_createdManually;

    protected static T m_instance;

    protected virtual string PrefabName
    {
        get { return null; }
    }

    public static T GetInstance()
    {
        if (m_instance == null)
        {
            m_instance = LoadAsPrefab();

            if (m_instance == null)
            {
                var gameObject = new GameObject(typeof(T).Name);
                m_instance = gameObject.AddComponent<T>();
            }

            var singletonInterface = m_instance as MonoBehaviourSingleton<T, TMeta>;
            singletonInterface.m_createdManually = true;

            DontDestroyOnLoad(m_instance.gameObject);
        }

        return m_instance;
    }

    protected virtual void Awake()
    {
        if (m_instance != null)
        {
            Destroy(gameObject);
        }
    }

    private static T LoadAsPrefab()
    {
        var meta = new TMeta();
        if (string.IsNullOrEmpty(meta.PrefabName))
            return null;

        var prefab = Resources.Load<T>(meta.PrefabName);
        return Instantiate<T>(prefab);
    }
}
