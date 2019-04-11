using UnityEngine;


#pragma warning disable 0414

	public abstract class ODSingletonMono<T> : MonoBehaviour where T : MonoBehaviour
	{

		private static T _instance;

		private static readonly object _lock = new object();

		private static bool destroyed = false;

		public static T Instance
		{
			get
			{
				return FindOrCreateInstance();
			}
		}

		public static bool IsAvailable
		{
			get
			{
				return FindObjectOfType<T>() != null;
			}
		}

		public static bool IsSingleton
		{
			get
			{
				T[] components = FindObjectsOfType<T>();
				return components.Length == 1;
			}
		}

		private static T FindOrCreateInstance()
		{
			lock (_lock)
			{
				if (_instance == null)
				{
					_instance = FindObjectOfType<T>();
				}
				if (_instance == null)
				{
					var go = new GameObject(typeof(T).Name);
					_instance = go.AddComponent<T>();
				}
				return _instance;
			}
		}

		protected virtual void OnDestroy()
		{
			destroyed = true;
		}

	}
	