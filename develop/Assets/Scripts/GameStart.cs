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
        Helper.Log("��ʼ��Ϸ" + DateTime.Now);
        UILoadingPanel.instance.SetLoadValue(0);
        AddressableUpdaterManager.instance.CheckUpdateStart(CheckUpdateEnd);
    }

    //���½���
    private void CheckUpdateEnd()
    {
        StartLoad();
    }

    private void StartLoad()
    {
        EndLoad();
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
        StringManager.instance.OnInit();
    }
}
