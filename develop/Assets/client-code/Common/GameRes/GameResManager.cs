using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameResManager : BaseSingle<GameResManager>
{
    public enum EResType 
    {
        None = 0,
        UI = 1,
    }

    public override void OnInit()
    {

    }

    //º”‘ÿ◊ ‘¥
    public void LoadAddressGameObject(EResType type, string addressName, Action<GameObject> callBack) 
    {
        Addressables.LoadAssetAsync<UnityEngine.Object>(addressName).Completed += (obj) => 
        {
            callBack?.Invoke(obj.Result as GameObject);
        };
    }
}
