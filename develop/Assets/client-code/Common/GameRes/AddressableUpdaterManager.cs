using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AddressableUpdaterManager : MonoBehaviourSingleton<AddressableUpdaterManager>
{
    private bool mNeedUpdate = false;
    private List<string> mNeedUpdateRes;
    private List<object> mNeedDownLoadRes = new List<object>();

    private Action mCallBack = null;

    protected override void OnInit()
    {

    }

    //开始检查更新
    public void CheckUpdateStart(Action callBack) 
    {
        mCallBack = callBack;
        mNeedUpdate = false;
        StartCoroutine(CheckUpdate());
    }

    //检查更新
    private IEnumerator CheckUpdate()
    {
        Debug.Log("检查更新" + DateTime.Now);
        UILoadingPanel.instance.SetLoadValue(0, 1, 1f);
        UILoadingPanel.instance.SetTipsText(GameDataLang.Check_Update);

        //初始化
        yield return Addressables.InitializeAsync();
        Debug.Log("Addressables初始化完成" + DateTime.Now);

        //检查更新
        var updateHandle = Addressables.CheckForCatalogUpdates(false);
        yield return updateHandle;

        if (updateHandle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded) 
        {
            var catalogs = updateHandle.Result;
            if (catalogs != null && catalogs.Count > 0) 
            {
                mNeedUpdate = true;
                mNeedUpdateRes = catalogs;

                var downloadHanlde = Addressables.UpdateCatalogs(mNeedUpdateRes, false);
                yield return downloadHanlde;
                if (downloadHanlde.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                {
                    mNeedDownLoadRes.Clear();
                    foreach (var item in downloadHanlde.Result)
                    {
                        mNeedDownLoadRes.AddRange(item.Keys);
                    }
                }
            }
        }

        Debug.LogFormat("更新Catalog下载完成:{0},{1},{2}", updateHandle.Status, mNeedUpdate, DateTime.Now);
        if (mNeedUpdate)
        {
            //有更新，开始下载
            yield return StartDownload();

            //下载完
            CheckUpdateEnd();
        }
        else
        {
            //无更新
            CheckUpdateEnd();
        }
        Addressables.Release(updateHandle);
    }

    private IEnumerator StartDownload()
    {
        for (int i = 0; i < mNeedDownLoadRes.Count; i++)
        {
            float value = i / (float)mNeedDownLoadRes.Count;
            string tip = string.Format(GameDataLang.Download_Res, i, mNeedDownLoadRes.Count);
            UILoadingPanel.instance.SetLoadValue(value, 0.1f, 1.0f / mNeedDownLoadRes.Count);
            UILoadingPanel.instance.SetTipsText(tip);
            var sizeHandle = Addressables.GetDownloadSizeAsync(mNeedDownLoadRes[i]);
            yield return sizeHandle;
            if (sizeHandle.Result > 0)
            {
                Debug.LogFormat("下载资源文件:{0}kb, key:{1}", sizeHandle.Result / 1024.0f, mNeedDownLoadRes[i]);
                var download = Addressables.DownloadDependenciesAsync(mNeedDownLoadRes[i]);
                yield return download;
            }
        }
    }

    //更新完成
    private void CheckUpdateEnd()
    {
        Debug.Log("更新完成" + DateTime.Now);
        UILoadingPanel.instance.SetLoadValue(1);
        UILoadingPanel.instance.SetTipsText(GameDataLang.Check_Update_End);
        mCallBack?.Invoke();
    }
}
