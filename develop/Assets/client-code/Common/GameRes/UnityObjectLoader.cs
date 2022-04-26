using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityObjectLoader : BaseLoader
{
    //��ǰʹ���б�
    private HashSet<UnityEngine.Object> mReferences = new HashSet<UnityEngine.Object>();

    public UnityObjectLoader(string name) : base(name)
    {
    }

    //ͬ������
    public T LoadAssetSync<T>() where T : UnityEngine.Object
    {
        var obj = LoadSync<T>();
        mReferences.Add(obj);
        return obj;
    }

    //�첽����
    public void LoadAssetAsync<T>(System.Action<T> onComplete) where T : UnityEngine.Object
    {
        LoadAsync<T>((result)=>
        {
            mReferences.Add(result);
            onComplete?.Invoke(result);
        });
    }

    public void Free<T>(T asset) where T : UnityEngine.Object
    {
        mReferences.Remove(asset);
    }

    //�ͷ�
    public override void Release()
    {
        if (mReferences.Count <= 0)
        {
            base.Release();
            GameResManager.instance.RemoveAssetPools(mName);
        }
    }
}
