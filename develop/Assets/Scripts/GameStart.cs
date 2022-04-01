using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        Debug.Log("��ʼ��Ϸ" + DateTime.Now);
        UILoadingPanel.instance.SetLoadValue(0);
        AddressableUpdaterManager.instance.CheckUpdateStart(CheckUpdateEnd);
    }

    //���½���
    private void CheckUpdateEnd() 
    {
        bool isInstall = true;
#if !UNITY_EDITOR
        isInstall = PlayerPrefs.GetInt(PlayerPrefsDefine.Install_Key, 0) == 1;
#endif
        if (!isInstall)
        {
            StartLoad();
        }
        else
        {
            Debug.Log("�޽�ѹ" + DateTime.Now);
            EndLoad();
        }
    }

    private void StartLoad()
    {
        //��ѹ
        string zipPath = string.Format("{0}/pack_data/bin.zip", Application.streamingAssetsPath);
        Debug.Log("��ʼ��������:" + zipPath + "  " + DateTime.Now);
        UILoadingPanel.instance.SetLoadValue(0, 1, 0.5f);
        UILoadingPanel.instance.SetTipsText(GameDataLang.Load_Config);
        StartCoroutine(UnityWebRequestUtils.LoadStream(zipPath, (result, data)=>
        {
            Debug.Log("�����������" + DateTime.Now);
            UILoadingPanel.instance.SetLoadValue(0.5f, 1, 0.5f);
            UILoadingPanel.instance.SetTipsText(GameDataLang.UnZip_Config);
            string outPath = string.Format("{0}", GameConst.ConfPath);
            Debug.Log("��ʼ��ѹ����:" + outPath + "  " + DateTime.Now);
            ZipUtility.UnzipFile(data, outPath, null, new UnZipFileCallBack(OnUnZipCallBack));
        }));
    }

    private void OnUnZipCallBack(bool result) 
    {
        if (result)
        {
            Debug.Log("�����ļ���ѹ���" + DateTime.Now);
            PlayerPrefs.SetInt(PlayerPrefsDefine.Install_Key, 1);
            EndLoad();
        }
        else
        {
            Debug.LogError("�����ļ���ѹ����" + DateTime.Now);
        }
    }

    private void EndLoad()
    {
        InitManager();
        UILoadingPanel.instance.OnLoadEnd(()=> 
        {
            UIManager.instance.ShowUIPanel("UILoginPanel", true);
        }, true);
    }


    private void InitManager() 
    { 
        GameConfigManager.instance.OnInit();
        UIManager.instance.OnInit();
        //StringManager.instance.OnInit();
    }
}
