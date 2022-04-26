using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelBase : MonoBehaviour
{
    private bool mIsInit = false;
    private object[] mParams = null;

    protected virtual void OnInit() { }
    protected virtual void OnShow(params object[] param) { }

    protected virtual void Awake()
    {
        mIsInit = false;
        OnInit();
    }

    protected virtual void OnEnable()
    {
        if (mIsInit)
        {
            return;
        }
        mIsInit = true;
        OnShow(mParams);
    }

    public void SetParam(params object[] param) 
    {
        mParams = param;
    }
}
