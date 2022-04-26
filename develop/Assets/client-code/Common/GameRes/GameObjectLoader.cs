using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectLoader : BaseLoader
{
    //缓存列表
    private Stack<GameObject> mCaches = new Stack<GameObject>();
    //当前使用列表
    private HashSet<GameObject> mReferences = new HashSet<GameObject>();

    public GameObject prefab = null;

    public GameObjectLoader(string name) : base(name) 
    {
        prefab = null;
    }

    public GameObjectLoader(GameObject obj) : base(obj.name) 
    {
        prefab = obj;
    }

    //同步实例化
    public GameObject Instantiate() 
    {
        GameObject obj = null;
        if (mCaches.Count > 0)
        {
            obj = mCaches.Pop();
        }
        else 
        {
            if (prefab == null)
            {
                prefab = LoadSync<GameObject>();
            }
            obj = GameObject.Instantiate(prefab);
            obj.name = mName;
        }
        mReferences.Add(obj);
        return obj;
    }

    //异步实例化
    public void InstantiateAsync(System.Action<GameObject> onComplete)
    {
        GameObject obj = null;
        if (mCaches.Count > 0)
        {
            obj = mCaches.Pop();
            mReferences.Add(obj);
            onComplete?.Invoke(obj);
        }
        else
        {
            if (prefab == null)
            {
                LoadAsync<GameObject>((result) =>
                {
                    prefab = result;
                    obj = GameObject.Instantiate(prefab);
                    obj.name = mName;
                    mReferences.Add(obj);
                    onComplete?.Invoke(obj);
                });
            }
            else 
            {
                obj = GameObject.Instantiate(prefab);
                obj.name = mName;
                mReferences.Add(obj);
                onComplete?.Invoke(obj);
            }
        }
    }

    //回收
    public void Free(GameObject obj) 
    {
        mCaches.Push(obj);
        mReferences.Remove(obj);
        obj.transform.SetParent(GameResManager.instance.cacheRoot);
    }

    //释放
    public override void Release() 
    {
        foreach (var obj in mCaches)
        {
            if (obj != null) 
            {
                GameObject.Destroy(obj);
            }
        }
        if (mReferences.Count <= 0) 
        {
            base.Release();
            GameResManager.instance.RemovePools(mName);
        }
    }
}
