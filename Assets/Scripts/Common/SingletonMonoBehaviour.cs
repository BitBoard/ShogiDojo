using UnityEngine;

/// <summary>
/// MonoBehaviorのシングルトンパターン利用時のクラス
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T instance;
 
	[SerializeField] private bool isDontDestroy = true;

	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				instance = (T)FindObjectOfType(typeof(T));
 
				if (instance == null)
				{
					Debug.LogError(typeof(T) + " is not Found");
				}
			}
 
			return instance;
		}
	}

	private void Awake()
	{
		if (this != Instance)
		{
			Destroy(this.gameObject);
			return;
		}

		if(isDontDestroy)
		{
			// シーンを跨いでも消えないように設定
			DontDestroyOnLoad(this.gameObject);
		}
		doAwake();
	}

	protected abstract void doAwake();
}