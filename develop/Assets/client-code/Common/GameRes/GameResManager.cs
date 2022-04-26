using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameResManager : BaseSingle<GameResManager>
{
    private Transform mPoolRoot = null;
    public Transform poolRoot
    {
        get
        {
            if (mPoolRoot == null)
            {
                mPoolRoot = new GameObject("[Asset Pool]").transform;
                mPoolRoot.transform.localPosition = Vector3.zero;
                mPoolRoot.transform.localScale = Vector3.zero;
                mPoolRoot.transform.rotation = Quaternion.identity;
                GameObject.DontDestroyOnLoad(mPoolRoot);
            }
            return mPoolRoot;
        }
    }

    private Transform mCacheRoot = null;
    public Transform cacheRoot
    {
        get
        {
            if (mCacheRoot == null)
            {
                mCacheRoot = new GameObject("cache node").transform;
                mCacheRoot.transform.SetParent(poolRoot.transform);
                mCacheRoot.transform.localPosition = Vector3.zero;
                mCacheRoot.transform.localScale = Vector3.zero;
                mCacheRoot.transform.rotation = Quaternion.identity;
                mCacheRoot.SetActive(false);
            }
            return mCacheRoot;
        }
    }

    //Prefab�����
    private Dictionary<string, GameObjectLoader> mPools = new Dictionary<string, GameObjectLoader>();
    //Prefab���ұ�
    private Dictionary<GameObject, GameObjectLoader> mLookup = new Dictionary<GameObject, GameObjectLoader>();
    //������Դ�����
    private Dictionary<string, UnityObjectLoader> mAssetPools = new Dictionary<string, UnityObjectLoader>();
    //������Դ���ұ�
    private Dictionary<UnityEngine.Object, UnityObjectLoader> mAssetLookup = new Dictionary<UnityEngine.Object, UnityObjectLoader>();

    public override void OnInit()
    {
    }

    #region prefab ����

    //ͬ��ʵ����
    public GameObject Instantiate(string name)
    {
        GameObjectLoader loader;
        if (!mPools.TryGetValue(name, out loader))
        {
            loader = new GameObjectLoader(name);
            mPools.Add(name, loader);
        }
        GameObject obj = loader.Instantiate();
        mLookup.Add(obj, loader);
        return obj;
    }

    //�첽ʵ����
    public void InstantiateAsync(string name, System.Action<GameObject> onComplete)
    {
        GameObjectLoader loader;
        if (!mPools.TryGetValue(name, out loader))
        {
            loader = new GameObjectLoader(name);
            mPools.Add(name, loader);
        }
        loader.InstantiateAsync((result) =>
        {
            mLookup.Add(result, loader);
            onComplete?.Invoke(result);
        });
    }

    //���ս������
    public void FreeGameObject(GameObject obj) 
    {
        GameObjectLoader loader;
        if (mLookup.TryGetValue(obj, out loader))
        {
            loader.Free(obj);
            mLookup.Remove(obj);
        }
    }

    public void RemovePools(string name) 
    {
        if (mPools.ContainsKey(name))
        {
            mPools.Remove(name);
        }
    }

    //��ʱ����loading����
    public void ReleaseAll() 
    {
        foreach (var item in mPools.Values)
        {
            item.Release();
        }
    }

    #endregion

    #region ����������Դ����

    //ͬ������
    public T LoadAsset<T>(string name) where T : UnityEngine.Object
    {
        UnityObjectLoader loader;
        if (!mAssetPools.TryGetValue(name, out loader))
        {
            loader = new UnityObjectLoader(name);
            mAssetPools.Add(name, loader);
        }
        var obj = loader.LoadAssetSync<T>();
        mAssetLookup.Add(obj, loader);
        return obj;
    }

    //�첽����
    public void LoadAssetAsync<T>(string name, System.Action<T> onComplete, bool autoRelease = true) where T : UnityEngine.Object
    {
        UnityObjectLoader loader;
        if (!mAssetPools.TryGetValue(name, out loader))
        {
            loader = new UnityObjectLoader(name);
            mAssetPools.Add(name, loader);
        }
        loader.LoadAssetAsync<T>((result)=> 
        {
            onComplete?.Invoke(result);
            mAssetLookup.Add(result, loader);
            if (autoRelease) 
            {
                FreeAsset(result);
            }
        });
    }

    public void FreeAsset<T>(T obj) where T : UnityEngine.Object
    {
        UnityObjectLoader loader;
        if (mAssetLookup.TryGetValue(obj, out loader))
        {
            loader.Free(obj);
            mAssetLookup.Remove(obj);
        }
    }

    public void RemoveAssetPools(string name)
    {
        if (mAssetPools.ContainsKey(name))
        {
            mAssetPools.Remove(name);
        }
    }

    #endregion
}
