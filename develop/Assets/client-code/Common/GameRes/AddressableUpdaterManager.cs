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

    //��ʼ������
    public void CheckUpdateStart(Action callBack) 
    {
        mCallBack = callBack;
        mNeedUpdate = false;
        StartCoroutine(CheckUpdate());
    }

    //������
    private IEnumerator CheckUpdate()
    {
        Debug.Log("������" + DateTime.Now);
        UILoadingPanel.instance.SetLoadValue(0, 1, 1f);
        UILoadingPanel.instance.SetTipsText(GameDataLang.Check_Update);

        //��ʼ��
        yield return Addressables.InitializeAsync();
        Debug.Log("Addressables��ʼ�����" + DateTime.Now);

        //������
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

        Debug.LogFormat("����Catalog�������:{0},{1},{2}", updateHandle.Status, mNeedUpdate, DateTime.Now);
        if (mNeedUpdate)
        {
            //�и��£���ʼ����
            yield return StartDownload();

            //������
            CheckUpdateEnd();
        }
        else
        {
            //�޸���
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
                Debug.LogFormat("������Դ�ļ�:{0}kb, key:{1}", sizeHandle.Result / 1024.0f, mNeedDownLoadRes[i]);
                var download = Addressables.DownloadDependenciesAsync(mNeedDownLoadRes[i]);
                yield return download;
            }
        }
    }

    //�������
    private void CheckUpdateEnd()
    {
        Debug.Log("�������" + DateTime.Now);
        UILoadingPanel.instance.SetLoadValue(1);
        UILoadingPanel.instance.SetTipsText(GameDataLang.Check_Update_End);
        mCallBack?.Invoke();
    }
}
