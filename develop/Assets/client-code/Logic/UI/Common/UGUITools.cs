using UnityEngine;
using System.Collections.Generic;

public static class UGUITools
{
	// Add a new child game object.

	static public GameObject AddChild(GameObject parent)
	{
		return AddChild(parent, true);
	}

	// Add a new child game object.

	static public GameObject AddChild(GameObject parent, bool undo)
	{
		GameObject go = new GameObject();
#if UNITY_EDITOR
		if (undo) UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");
#endif
		if (parent != null)
		{
			RectTransform rectTransform = go.AddComponent<RectTransform>();
			rectTransform.SetParent(parent.transform, false);
			SetLayerRecursively(go, parent.layer);
		}
		return go;
	}

	// Instantiate an object and add it to the specified parent.

	static public GameObject AddChild(GameObject parent, GameObject prefab)
	{
		GameObject go = GameObject.Instantiate(prefab) as GameObject;
#if UNITY_EDITOR
		UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");
#endif
		if (go != null && parent != null)
		{
			RectTransform rectTransform = go.AddComponent<RectTransform>();
			rectTransform.SetParent(parent.transform, false);
			SetLayerRecursively(go, parent.layer);
		}
		return go;
	}

	// Add a child object to the specified parent and attaches the specified script to it.

	static public T AddChild<T>(GameObject parent) where T : Component
	{
		GameObject go = AddChild(parent);
		go.name = GetTypeName<T>();
		return go.AddComponent<T>();
	}

	// Add a child object to the specified parent and attaches the specified script to it.

	static public T AddChild<T>(GameObject parent, bool undo = true) where T : Component
	{
		GameObject go = AddChild(parent, undo);
		go.name = GetTypeName<T>();
		return go.AddComponent<T>();
	}

	static public T AddChild<T>(GameObject parent, string name) where T : Component
	{
		GameObject go = AddChild(parent);
		go.name = name;
		return go.AddComponent<T>();
	}

	/// <summary>
	/// Finds the specified component on the game object or one of its parents.
	/// </summary>

	static public T FindInParents<T>(GameObject go) where T : Component
	{
		if (go == null) return null;
		// Commented out because apparently it causes Unity 4.5.3 to lag horribly:
		// http://www.tasharen.com/forum/index.php?topic=10882.0
#if UNITY_4_3
#if UNITY_FLASH
		object comp = go.GetComponent<T>();
#else
		T comp = go.GetComponent<T>();
#endif
		if (comp == null)
		{
			Transform t = go.transform.parent;

			while (t != null && comp == null)
			{
				comp = t.gameObject.GetComponent<T>();
				t = t.parent;
			}
		}
#if UNITY_FLASH
		return (T)comp;
#else
		return comp;
#endif
#else
		return go.GetComponentInParent<T>();
#endif
	}

	/// <summary>
	/// Finds the specified component on the game object or one of its parents.
	/// </summary>

	static public T FindInParents<T>(Transform trans) where T : Component
	{
		if (trans == null) return null;
#if UNITY_4_3
#if UNITY_FLASH
		object comp = trans.GetComponent<T>();
#else
		T comp = trans.GetComponent<T>();
#endif
		if (comp == null)
		{
			Transform t = trans.transform.parent;

			while (t != null && comp == null)
			{
				comp = t.gameObject.GetComponent<T>();
				t = t.parent;
			}
		}
#if UNITY_FLASH
		return (T)comp;
#else
		return comp;
#endif
#else
		return trans.GetComponentInParent<T>();
#endif
	}

	/// <summary>
	/// Destroy the specified object, immediately if in edit mode.
	/// </summary>

	static public void Destroy(UnityEngine.Object obj)
	{
		if (obj)
		{
			if (obj is Transform)
			{
				Transform t = (obj as Transform);
				GameObject go = t.gameObject;

				if (Application.isPlaying)
				{
					t.parent = null;
					UnityEngine.Object.Destroy(go);
				}
				else UnityEngine.Object.DestroyImmediate(go);
			}
			else if (obj is GameObject)
			{
				GameObject go = obj as GameObject;
				Transform t = go.transform;

				if (Application.isPlaying)
				{
					t.parent = null;
					UnityEngine.Object.Destroy(go);
				}
				else UnityEngine.Object.DestroyImmediate(go);
			}
			else if (Application.isPlaying) UnityEngine.Object.Destroy(obj);
			else UnityEngine.Object.DestroyImmediate(obj);
		}
	}

	/// <summary>
	/// Convenience extension that destroys all children of the transform.
	/// </summary>

	static public void DestroyChildren(this Transform t)
	{
		bool isPlaying = Application.isPlaying;

		while (t.childCount != 0)
		{
			Transform child = t.GetChild(0);

			if (isPlaying)
			{
				child.SetParent(null);
				UnityEngine.Object.Destroy(child.gameObject);
			}
			else UnityEngine.Object.DestroyImmediate(child.gameObject);
		}
	}

	/// <summary>
	/// Destroy the specified object immediately, unless not in the editor, in which case the regular Destroy is used instead.
	/// </summary>
	static public void DestroyImmediate(UnityEngine.Object obj)
	{
		if (obj != null)
		{
			if (Application.isEditor) UnityEngine.Object.DestroyImmediate(obj);
			else UnityEngine.Object.Destroy(obj);
		}
	}

	// Helper function that returns the string name of the type.

	static public string GetTypeName<T>()
	{
		string s = typeof(T).ToString();
		// TODO 以后移除NGUI后要改为UI;
		if (s.StartsWith("UGUI")) s = s.Substring(2);
		else if (s.StartsWith("UnityEngine.")) s = s.Substring(12);
		return s;
	}

	// Helper function that returns the string name of the type.

	static public string GetTypeName(UnityEngine.Object obj)
	{
		if (obj == null) return "Null";
		string s = obj.GetType().ToString();
		// TODO 以后移除NGUI后要改为UI;
		if (s.StartsWith("UGUI")) s = s.Substring(2);
		else if (s.StartsWith("UnityEngine.")) s = s.Substring(12);
		return s;
	}

	private static void SetLayerRecursively(GameObject go, int layer)
	{
		go.layer = layer;
		Transform t = go.transform;
		for (int i = 0; i < t.childCount; i++)
			SetLayerRecursively(t.GetChild(i).gameObject, layer);
	}

	#region Audio

	static AudioListener mListener;

	static bool mLoaded = false;
	static float mGlobalVolume = 1f;

	/// <summary>
	/// Globally accessible volume affecting all sounds played via UGUITools.PlaySound().
	/// </summary>

	static public float soundVolume
	{
		get
		{
			if (!mLoaded)
			{
				mLoaded = true;
				mGlobalVolume = PlayerPrefs.GetFloat("Sound", 1f);
			}
			return mGlobalVolume;
		}
		set
		{
			if (mGlobalVolume != value)
			{
				mLoaded = true;
				mGlobalVolume = value;
				PlayerPrefs.SetFloat("Sound", value);
			}
		}
	}

	static float mLastTimestamp = 0f;
	static AudioClip mLastClip;

	/// <summary>
	/// Play the specified audio clip with the specified volume and pitch.
	/// </summary>
	static public AudioSource PlaySound(AudioClip clip, float volume, float pitch)
	{
		float time = Time.timeSinceLevelLoad;
		if (mLastClip == clip && mLastTimestamp + 0.1f > time) return null;

		mLastClip = clip;
		mLastTimestamp = time;
		volume *= soundVolume;

		if (clip != null && volume > 0.01f)
		{
			if (mListener == null || !mListener.isActiveAndEnabled)
			{
				AudioListener[] listeners = GameObject.FindObjectsOfType(typeof(AudioListener)) as AudioListener[];

				if (listeners != null)
				{
					for (int i = 0; i < listeners.Length; ++i)
					{
						if (listeners[i].isActiveAndEnabled)
						{
							mListener = listeners[i];
							break;
						}
					}
				}

				if (mListener == null)
				{
					Camera cam = Camera.main;
					if (cam == null) cam = GameObject.FindObjectOfType(typeof(Camera)) as Camera;
					if (cam != null) mListener = cam.gameObject.AddComponent<AudioListener>();
				}
			}

			if (mListener != null && mListener.isActiveAndEnabled)
			{
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6
				AudioSource source = mListener.audio;
#else
				AudioSource source = mListener.GetComponent<AudioSource>();
#endif
				if (source == null) source = mListener.gameObject.AddComponent<AudioSource>();
#if !UNITY_FLASH
				source.priority = 50;
				source.pitch = pitch;
#endif
				source.PlayOneShot(clip, volume);
				return source;
			}
		}
		return null;
	}

	#endregion
}