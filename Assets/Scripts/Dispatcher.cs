using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dispatcher : MonoBehaviour
{
	public delegate void Callback(object args);

	void Awake()
	{
		DontDestroyOnLoad(this);
	}

	private class Task
	{
		public Task(Callback callback, object args)
		{
			m_callback = callback;
			m_args = args;
		}

		public Callback m_callback;
		public object m_args;
	}

	private static Queue<Task> m_methodsQueue = new Queue<Task>();

	private static object m_methodsQueueSynch = new object();
	
	void Update()
	{
		lock (m_methodsQueueSynch)
		{
			while (m_methodsQueue.Count > 0)
			{
				Task task = m_methodsQueue.Dequeue();
				task.m_callback(task.m_args);
			}
		}
	}

	public static void Dispatch(Callback callback)
	{
		Task task = new Task(callback, null);

		lock (m_methodsQueueSynch)
			m_methodsQueue.Enqueue(task);
	}
}
