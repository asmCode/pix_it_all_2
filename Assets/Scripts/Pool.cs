using UnityEngine;
using System.Collections;

public class Pool<T>
    where T : Component
{
    private T m_prefab;
    private int m_count;
    private T[] m_objects;
    private Transform m_container;

    public Pool(T prefab, int count, Transform container, System.Action<T> createdCallback)
    {
        m_prefab = prefab;
        m_count = count;
        m_container = container;

        m_objects = new T[m_count];
        for (int i = 0; i < m_count; i++)
        {
            m_objects[i] = Object.Instantiate(m_prefab);
            if (container != null)
                m_objects[i].transform.SetParent(m_container);

            if (createdCallback != null)
                createdCallback(m_objects[i]);

            m_objects[i].gameObject.SetActive(false);
        }
    }

    public virtual T Get()
    {
        for (int i = 0; i < m_objects.Length; i++)
        {
            if (!m_objects[i].gameObject.activeSelf)
            {
                m_objects[i].gameObject.SetActive(true);
                return m_objects[i];
            }
        }

        return null;
    }
}
