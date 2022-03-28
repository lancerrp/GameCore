using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    private void Awake()
    {
        StartGame();
    }

    private void StartGame()
    {
        Debug.Log("开始游戏" + DateTime.Now);
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
        UnZipFileCallBack callBack = new UnZipFileCallBack((result) =>
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
        });
        string zipPath = string.Format("{0}/pack_data/bin.zip", Application.streamingAssetsPath);
        Debug.Log("开始加载配置:" + zipPath + "  " + DateTime.Now);
        StartCoroutine(UnityWebRequestUtils.LoadStream(zipPath, (result, data)=>
        {
            Debug.Log("加载配置完成" + DateTime.Now);
            string outPath = string.Format("{0}", GameConst.ConfPath);
            Debug.Log("开始解压配置:" + outPath + "  " + DateTime.Now);
            ZipUtility.UnzipFile(data, outPath, null, callBack);
        }));
    }

    private void EndLoad()
    {

    }
}
