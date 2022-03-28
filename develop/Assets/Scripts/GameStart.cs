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
        Debug.Log("��ʼ��Ϸ" + DateTime.Now);
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
        UnZipFileCallBack callBack = new UnZipFileCallBack((result) =>
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
        });
        string zipPath = string.Format("{0}/pack_data/bin.zip", Application.streamingAssetsPath);
        Debug.Log("��ʼ��������:" + zipPath + "  " + DateTime.Now);
        StartCoroutine(UnityWebRequestUtils.LoadStream(zipPath, (result, data)=>
        {
            Debug.Log("�����������" + DateTime.Now);
            string outPath = string.Format("{0}", GameConst.ConfPath);
            Debug.Log("��ʼ��ѹ����:" + outPath + "  " + DateTime.Now);
            ZipUtility.UnzipFile(data, outPath, null, callBack);
        }));
    }

    private void EndLoad()
    {

    }
}
