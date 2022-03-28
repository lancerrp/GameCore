

using UnityEngine;

public static class GameConst
{
    public static string GameDefine = "Lancer";
    
    public static string ConfPath 
    {
        get 
        {
            string path = "";
#if UNITY_EDITOR
            path = string.Format("{0}/../../conf", Application.dataPath);
#elif UNITY_ANDROID
            path = string.Format("{0}", Application.persistentDataPath);
#elif UNITY_IPHONE
            path = string.Format("{0}", Application.persistentDataPath);
#endif
            return path;
        }
    }

    public static string ConfWebRequestPath
    {
        get
        {
            string path = "";
#if UNITY_EDITOR
            path = string.Format("{0}/../../conf", Application.dataPath);
#elif UNITY_ANDROID
            path = string.Format("file://{0}", Application.persistentDataPath);
#elif UNITY_IPHONE
            path = string.Format("{0}", Application.persistentDataPath);
#endif
            return path;
        }
    }

    //记录所有配置表的配置文件
    public static string ConfXmlFile
    {
        get
        {
            return string.Format("{0}/config/xml_config.xml", ConfPath);
        }
    }

    public static string ConfBinFile
    {
        get
        {
            return string.Format("{0}/bin/xml_config.bin", ConfWebRequestPath);
        }
    }

}

public static class PlayerPrefsDefine 
{
    public static string Install_Key = string.Format("{0}_Install_{1}", GameConst.GameDefine, Application.version);
    public static string Config_Key = string.Format("{0}_Config", GameConst.GameDefine);
}