using UnityEngine;

/// <summary>
/// Manager scripts should extend this class to implement a singleton pattern
/// </summary>
/// <typeparam name="T">Type of class</typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
	public static T instance;

	protected virtual void Awake() {
		if (instance == null) {
			instance = this as T;
		}
		else {
			//Debug.LogWarning("Duplicate singeton, destroys the new object");
			Destroy(gameObject);
		}
	}
}