using Config;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StringManager : BaseSingle<StringManager>
{
    private Dictionary<int, string> mStringMap;

    public override void OnInit()
    {
        mStringMap = new Dictionary<int, string>();
        var conf = GameConfigManager.instance.LoadXmlConfig<StringConfig>("string_config");
        if (conf != null) 
        {
            mStringMap = conf.lang.ToDictionary(item => item.id, item => item.value);
        }
    }

    public string GetStringValue(int key) 
    {
        if (mStringMap.ContainsKey(key)) 
        {
            return mStringMap[key];
        }
        return "";
    }
}
