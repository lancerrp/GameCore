using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UIPanelInfo
{
    public string key;
    public UIPanelBase panelBase;
    public bool active;
    public object[] param = null;

    private bool mIsLoad = false;
    private GameObject mPanelObject = null;

    public UIPanelInfo(string panel) 
    {
        key = panel;
    }

    public void ShowPanel()
    {
        if (mIsLoad)
        {
            OnPanelLoadComplete();
        }
        else
        {
            LoadPanelRes();
        }
    }

    public void Close()
    {
        if (mIsLoad)
        {
            SetPanelActive();
            GameResManager.instance.FreeGameObject(mPanelObject);
        }
    }

    public void UnloadPanelRes() 
    {
        if (mIsLoad) 
        {
            mPanelObject.ReleaseInstance();
        }
    }

    private void SetParentNode(GameObject go) 
    {
        RectTransform rectTrans = go.GetComponent<RectTransform>();
        Transform parent = UIManager.instance.Canvas.transform;
        if (rectTrans != null)
        {
            rectTrans.SetParent(parent, false);
            rectTrans.anchorMin = Vector2.zero;
            rectTrans.anchorMax = Vector2.one;
            rectTrans.sizeDelta = Vector2.zero;
            rectTrans.localRotation = Quaternion.identity;
            rectTrans.localScale = Vector3.one;
        }
        else
        {
            Transform tran = go.transform;
            tran.SetParent(parent, false);
            tran.localPosition = Vector3.zero;
            tran.localRotation = Quaternion.identity;
            tran.localScale = Vector3.one;
        }
    }

    private void LoadPanelRes() 
    {
        GameResManager.instance.InstantiateAsync(key, OnPanelResLoaded);
    }

    private void OnPanelResLoaded(GameObject obj)
    {
        mPanelObject = obj;
        if (mPanelObject == null) 
        {
            return;
        }
        SetParentNode(mPanelObject);
        mIsLoad = true;
        panelBase = mPanelObject.GetComponent<UIPanelBase>();
        OnPanelLoadComplete();
    }

    private void OnPanelLoadComplete() 
    {
        SetPanelActive();
    }

    private void SetPanelActive() 
    {
        if (panelBase == null)
        {
            return;
        }
        panelBase.SetParam(param);
        panelBase.SetActive(active);
    }

}
