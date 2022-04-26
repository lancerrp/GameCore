using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BaseLoader
{
    enum eTaskType 
    {
        Free = 0,
        Loading = 1,
        Done = 2,
    }

    protected string mName = string.Empty;

    //是否已加载
    protected bool mIsLoad = false;

    private AsyncOperationHandle mHandler;

    public BaseLoader(string name)
    {
        mName = name;
        mIsLoad = false;
    }

    //异步加载
    public virtual void LoadAsync<T>(Action<T> onComplete) where T : UnityEngine.Object
    {
        if (mIsLoad)
        {
            if (mHandler.IsDone)
            {
                onComplete?.Invoke(mHandler.Result as T);
            }
            else 
            {
                mHandler.Completed += (result) =>
                {
                    if (result.Status == AsyncOperationStatus.Succeeded)
                    {
                        onComplete?.Invoke(result.Result as T);
                    }
                    else 
                    {
                        onComplete?.Invoke(null);
                    }
                    
                };
            }
        }
        else 
        {
            mIsLoad = true;
            mHandler = Addressables.LoadAssetAsync<T>(mName);
            mHandler.Completed += (result) =>
            {
                if (result.Status == AsyncOperationStatus.Succeeded)
                {
                    onComplete?.Invoke(result.Result as T);
                }
                else
                {
                    onComplete?.Invoke(null);
                }
            };
        }
    }

    //同步加载
    public virtual T LoadSync<T>() where T : UnityEngine.Object
    {
        T result;
        if (mIsLoad)
        {
            if (mHandler.IsDone)
            {
                result = mHandler.Result as T;
            }
            else
            {
                Debug.LogError("已有异步任务在加载：" + mName);
                return null;
            }
        }
        else 
        {
            mIsLoad = true;
            mHandler = Addressables.LoadAssetAsync<T>(mName);
            result = mHandler.WaitForCompletion() as T;
        }
        return result;
    }

    public virtual void Release()
    {
        if (mIsLoad) 
        {
            mIsLoad = false;
            Addressables.Release(mHandler);
        }
    }
}
