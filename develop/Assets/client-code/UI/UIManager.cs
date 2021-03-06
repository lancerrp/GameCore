using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : BaseSingle<UIManager>
{
    private Canvas mCanvas = null;
    public Canvas Canvas 
    {
        get 
        {
            if (mCanvas == null) 
            {
                mCanvas = GameObject.Find("Canvas")?.GetComponent<Canvas>();
            }
            return mCanvas;
        }
    }

    private Dictionary<string, UIPanelInfo> mPanelMap = new Dictionary<string, UIPanelInfo>();

    public override void OnInit()
    {
    }

    //??ʾUI
    public void ShowUIPanel(string panel, bool active)
    {
        UIPanelInfo info = GetPanelInfo(panel);
        if (info == null) 
        {
            return;
        }
        if (info.active == active) 
        {
            return;
        }
        info.active = active;
        if (active)
        {
            OpenPanelInternal(info);
        }
        else 
        {
            CloseInternal(info);
        }
    }

    public UIPanelInfo GetPanelInfo(string panel)
    {
        UIPanelInfo info = null;
        if (!mPanelMap.ContainsKey(panel))
        {
            info = new UIPanelInfo(panel);
            info.active = false;
            mPanelMap.Add(panel, info);
        }

        return info;
    }

    private void OpenPanelInternal(UIPanelInfo info) 
    {
        if (info == null) 
        {
            return;
        }
        info.ShowPanel();
    }

    private void CloseInternal(UIPanelInfo info)
    {
        if (info == null)
        {
            return;
        }
        info.Close();
    }
}
