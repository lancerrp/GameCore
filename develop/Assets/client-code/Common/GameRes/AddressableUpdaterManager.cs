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
        Helper.Log("������" + DateTime.Now);
        UILoadingPanel.instance.SetLoadValue(0, 1, 1f);
        UILoadingPanel.instance.SetTipsText(GameDataLang.Check_Update);

        //��ʼ��
        yield return Addressables.InitializeAsync();
        Helper.Log("Addressables��ʼ�����" + DateTime.Now);

        //������
        var checkHandle = Addressables.CheckForCatalogUpdates(false);
        yield return checkHandle;

        if (checkHandle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded) 
        {
            var catalogs = checkHandle.Result;
            if (catalogs != null && catalogs.Count > 0) 
            {
                mNeedUpdate = true;
                mNeedUpdateRes = catalogs;

                var updateHanlde = Addressables.UpdateCatalogs(mNeedUpdateRes, false);
                yield return updateHanlde;
                if (updateHanlde.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                {
                    mNeedDownLoadRes.Clear();
                    foreach (var item in updateHanlde.Result)
                    {
                        mNeedDownLoadRes.AddRange(item.Keys);
                    }
                }
                Addressables.Release(updateHanlde);
            }
        }

        Helper.LogFormat("����Catalog�������:{0},{1},{2}", checkHandle.Status, mNeedUpdate, DateTime.Now);
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
        Addressables.Release(checkHandle);
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
                Helper.LogFormat("������Դ�ļ�:{0}mb, key:{1}", sizeHandle.Result / (1024.0f * 1024.0f), mNeedDownLoadRes[i]);
                var download = Addressables.DownloadDependenciesAsync(mNeedDownLoadRes[i]);
                yield return download;

                Addressables.Release(download);
            }
            Addressables.Release(sizeHandle);
        }
    }

    //�������
    private void CheckUpdateEnd()
    {
        Helper.Log("�������" + DateTime.Now);
        UILoadingPanel.instance.SetLoadValue(1);
        UILoadingPanel.instance.SetTipsText(GameDataLang.Check_Update_End);
        mCallBack?.Invoke();
    }
}
