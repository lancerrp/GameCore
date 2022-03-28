using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfigData
{
    public Dictionary<string, object> mXmlData = new Dictionary<string, object>();
    public Dictionary<string, Dictionary<int, object>> mSheetData = new Dictionary<string, Dictionary<int, object>>();

    public GameConfigData() 
    {
    }
}
