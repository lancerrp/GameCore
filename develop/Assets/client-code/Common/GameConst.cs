

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

    //��¼�������ñ�������ļ�
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

public static class GameDataLang
{
    public const string Check_Update = "������...";
    public const string Check_Update_End = "�������...";
    public const string Download_Res = "��Դ����{0}/{1}...";
    public const string Download_Res_Finish = "��Դ�������";
    public const string Load_Config = "���������ļ�...";
    public const string UnZip_Config = "��ѹ�����ļ�...";
}