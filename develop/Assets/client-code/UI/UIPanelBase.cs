using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelBase : MonoBehaviour
{
    protected virtual void OnInit() { }
    protected virtual void OnShow() { }

    private void Awake()
    {
        OnInit();
    }
}
