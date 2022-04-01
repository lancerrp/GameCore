using System.Collections.Generic;
using UnityEngine;

public static class ObjectCommonUtils
{
	public static GameObject GetChild(GameObject go, string name, bool includeSelf = false)
	{
		if (go == null || string.IsNullOrEmpty(name))
		{
			return go;
		}
		if (includeSelf && go.name == name)
		{
			return go;
		}

		Transform rootTrans = go.transform;
		Transform childTrans = null;
		if (name.IndexOf('/') != -1)
		{
			childTrans = rootTrans.Find(name);
			if (childTrans != null)
			{
				return childTrans.gameObject;
			}
			return null;
		}

		childTrans = rootTrans.Find(name);
		if (childTrans != null)
		{
			return childTrans.gameObject;
		}
		// 当查找对象的父节点隐藏时，上面的逻辑找不到，必须按个查找;
		for (int i = 0; i < rootTrans.childCount; i++)
		{
			GameObject childGo = rootTrans.GetChild(i).gameObject;
			childGo = GetChild(childGo, name);
			if (childGo != null)
			{
				return childGo;
			}
		}
		return null;
	}

	public static Transform GetTransChild(Transform trans, string name)
	{
		if (trans == null || string.IsNullOrEmpty(name))
		{
			return trans;
		}

		Transform tranChild = trans.Find(name);
		if (tranChild != null)
		{
			return tranChild;
		}
		return null;
	}

	public static T GetChildComponent<T>(GameObject go, string name) where T : Component
	{
		if (go == null)
		{
			return null;
		}
		if (string.IsNullOrEmpty(name))
		{
			return go.GetComponent<T>();
		}

		GameObject child = GetChild(go, name);
		if (child != null)
		{
			return child.GetComponent<T>();
		}
		return null;
	}

	public static T[] GetChildComponents<T>(GameObject go, string name) where T : Component
	{
		if (go == null)
		{
			return null;
		}
		if (string.IsNullOrEmpty(name))
		{
			return go.GetComponents<T>();
		}
		GameObject child = GetChild(go, name);
		if (child != null)
		{
			return child.GetComponents<T>();
		}
		return null;
	}

	public static T GetComponentOrAdd<T>(GameObject go) where T : Component
	{
		if (go == null)
		{
			return null;
		}

		T component = go.GetComponent<T>();
		if (component == null)
		{
			return go.AddComponent<T>();
		}
		return component;
	}

	public static T GetChildComponentOrAdd<T>(GameObject go, string name) where T : Component
	{
		GameObject childGo = GetChild(go, name);
		return GetComponentOrAdd<T>(childGo);
	}

	public static T AddChildComponent<T>(GameObject go, string name) where T : Component
	{
		if (go == null || string.IsNullOrEmpty(name))
		{
			return null;
		}

		GameObject childGo = GetChild(go, name);
		if (childGo != null)
		{
			T component = childGo.GetComponent<T>();
			if (component == null)
			{
				return childGo.AddComponent<T>();
			}
			else
			{
				return component;
			}
		}
		return null;
	}

	public static void SetObjLayer(GameObject go, int layer, bool recursively)
	{
		if (go == null)
		{
			return;
		}

		go.layer = layer;
		if (recursively)
		{
			Transform[] childTrans = go.transform.GetComponentsInChildren<Transform>(true);
			if (childTrans != null)
			{
				for (int i = 0; i < childTrans.Length; i++)
				{
					childTrans[i].gameObject.layer = layer;
				}
			}
		}
	}

	public static void SetObjLayer(Transform trans, int layer, bool recursively)
	{
		if (trans == null)
		{
			return;
		}

		trans.gameObject.layer = layer;
		if (recursively)
		{
			Transform[] childTrans = trans.GetComponentsInChildren<Transform>(true);
			if (childTrans != null)
			{
				for (int i = 0; i < childTrans.Length; i++)
				{
					childTrans[i].gameObject.layer = layer;
				}
			}
		}
	}

	public static void DestoryChildren(GameObject go)
	{
		if (go == null)
		{
			return;
		}

		Transform rootTrans = go.transform;
		List<GameObject> childList = new List<GameObject>();
		for (int i = 0; i < rootTrans.childCount; i++)
		{
			GameObject child = rootTrans.GetChild(i).gameObject;
			DestoryChildren(child);
			childList.Add(child);
		}

		rootTrans.DetachChildren();
		foreach (GameObject obj in childList)
		{
			obj.SetActive(false);
			UGUITools.DestroyImmediate(obj);
		}
	}

	public static bool IsChild(Transform child, Transform parent)
	{
		if (child == null || parent == null)
		{
			return false;
		}

		Transform tran = child.parent;
		while (tran != null)
		{
			if (tran == parent)
			{
				return true;
			}

			tran = tran.parent;
		}

		return false;
	}

    //释放资源
    public static void ReleaseInstance(this GameObject obj)
    {
        UnityEngine.AddressableAssets.Addressables.ReleaseInstance(obj);
    }

    public static TComponent SetActive<TComponent>(this TComponent self, bool is_active) where TComponent : Component
    {
        if (self && self.gameObject)
            self.gameObject.SetActive(is_active);

        return self;
    }
}
