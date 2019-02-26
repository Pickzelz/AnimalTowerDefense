using UnityEngine;

namespace Dictus.Utils
{
	public abstract class Singleton<T> : MonoBehaviour where T : Object
	{
		public static T instance;

		protected abstract void Initialize();

		//Don't call Awake if you create a derivative of this class
		protected void Awake()
		{
			if (instance == null)
			{
				DontDestroyOnLoad(gameObject);
				instance = GetComponent<T>();
				Initialize();
			}
			else
			{
				Destroy(gameObject);
			}
		}
	}
}
