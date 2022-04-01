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
        Debug.Log("开始游戏" + DateTime.Now);
        UILoadingPanel.instance.SetLoadValue(0);
        AddressableUpdaterManager.instance.CheckUpdateStart(CheckUpdateEnd);
    }

    //更新结束
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
            Debug.Log("无解压" + DateTime.Now);
            EndLoad();
        }
    }

    private void StartLoad()
    {
        //解压
        string zipPath = string.Format("{0}/pack_data/bin.zip", Application.streamingAssetsPath);
        Debug.Log("开始加载配置:" + zipPath + "  " + DateTime.Now);
        UILoadingPanel.instance.SetLoadValue(0, 1, 0.5f);
        UILoadingPanel.instance.SetTipsText(GameDataLang.Load_Config);
        StartCoroutine(UnityWebRequestUtils.LoadStream(zipPath, (result, data)=>
        {
            Debug.Log("加载配置完成" + DateTime.Now);
            UILoadingPanel.instance.SetLoadValue(0.5f, 1, 0.5f);
            UILoadingPanel.instance.SetTipsText(GameDataLang.UnZip_Config);
            string outPath = string.Format("{0}", GameConst.ConfPath);
            Debug.Log("开始解压配置:" + outPath + "  " + DateTime.Now);
            ZipUtility.UnzipFile(data, outPath, null, new UnZipFileCallBack(OnUnZipCallBack));
        }));
    }

    private void OnUnZipCallBack(bool result) 
    {
        if (result)
        {
            Debug.Log("配置文件解压完成" + DateTime.Now);
            PlayerPrefs.SetInt(PlayerPrefsDefine.Install_Key, 1);
            EndLoad();
        }
        else
        {
            Debug.LogError("配置文件解压出错！" + DateTime.Now);
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
