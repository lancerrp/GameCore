using Config;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfigManager : BaseSingle<GameConfigManager>
{
    //二进制加载开关
    public static bool LoadBinaryData 
    {
        get 
        {
#if UNITY_EDITOR
            return PlayerPrefs.GetInt(PlayerPrefsDefine.Config_Key, 0) == 1;
#else
            return true;   
#endif
        }
    }

    private XmlConfigGroup mConfig = null;  //xml配置
    private Dictionary<string, object> mCacheXmlMap = new Dictionary<string, object>();
    private Dictionary<Type, Dictionary<int, object>> mCacheSheetMap = new Dictionary<Type, Dictionary<int, object>>();

    public override void OnInit()
    {
        InitConfigs();
    }

    private void InitConfigs()
    {
        if (LoadBinaryData)
        {
            UnityWebRequestUtils.LoadStreamSync(GameConst.ConfBinFile, (success, data) =>
            {
                Debug.LogFormat("加载配置:xml_config,{0},{1}", success, DateTime.Now);
                if (success)
                {
                    mConfig = ProtoBuf.Serializer.Deserialize<XmlConfigGroup>(data);
                }
            });
        }
        else
        {
            mConfig = XmlUtils.GetXMLData<XmlConfigGroup>(GameConst.ConfXmlFile);
        }
    }

    public T LoadXmlConfig<T>(string fileName) where T : class
    {
        if (mCacheXmlMap.ContainsKey(fileName)) 
        {
            return mCacheXmlMap[fileName] as T;
        }
        T result = null;
        if (LoadBinaryData)
        {
            string path = string.Format("{0}/bin/{1}.bin", GameConst.ConfPath, fileName);
            UnityWebRequestUtils.LoadStreamSync(path, (success, data) =>
            {
                Debug.LogFormat("加载配置:{0},{1},{2}", fileName, success, DateTime.Now);
                if (success)
                {
                    result = ProtoBuf.Serializer.Deserialize<T>(data);
                }
            });
        }
        else 
        {
            string path = string.Format("{0}/config/xml_config/{1}.xml", GameConst.ConfPath, fileName);
            result = XmlUtils.GetXMLData<T>(path);
        }
        mCacheXmlMap.Add(fileName, result);
        return result;
    }

    public T LoadSheetConfig<T>(int id) where T : class
    {
        Type type = typeof(T);
        if (mCacheSheetMap.ContainsKey(type))
        {
            if (mCacheSheetMap[type].ContainsKey(id))
            {
                return mCacheSheetMap[type][id] as T;
            }
            else 
            {
                Debug.LogErrorFormat("load sheet error, type:{0},id:{1}",type, id);
                return null;
            }
        }
        string path = GetSheetPath(type.Name);
        T result = null;
        if (LoadBinaryData)
        {
            string binPath = string.Format("{0}/bin/{1}_sheet.bin", GameConst.ConfPath, System.IO.Path.GetFileNameWithoutExtension(path));
            T[] array = null ;
            UnityWebRequestUtils.LoadStreamSync(binPath, (success, data) =>
            {
                Debug.LogFormat("加载配置:{0},{1},{2}", type.ToString(), success, DateTime.Now);
                if (success)
                {
                    array = ProtoBuf.Serializer.Deserialize<T[]>(data);
                }
            });
            Dictionary<int, object> map = new Dictionary<int, object>();
            if (array != null) 
            {
                for (int i = 0; i < array.Length; i++)
                {
                    int tempID = (int)type.GetProperty("id").GetValue(array[i]);
                    map.Add(tempID, array[i]);
                    if (tempID == id) 
                    {
                        result = array[i];
                    }
                }
            }
            mCacheSheetMap.Add(type, map);
        }
        else
        {
            string filePath = string.Format("{0}/{1}", GameConst.ConfPath, path);
            var fileList = FileUtils.GetFiles(filePath, ".xml");
            Dictionary<int, object> map = new Dictionary<int, object>();
            for (int i = 0; i < fileList.Count; i++)
            {
                var data = XmlUtils.GetXMLData<T>(fileList[i].FullName);
                int tempID = (int)type.GetProperty("id").GetValue(data);
                map.Add(tempID, data);
                if (tempID == id)
                {
                    result = data;
                }
            }
        }

        return result;
    }

    private string GetSheetPath(string type) 
    {
        if (mConfig == null) 
        {
            return null;
        }
        for (int i = 0; i < mConfig.sheet_info.Count; i++)
        {
            if (mConfig.sheet_info[i].xml_type == type) 
            {
                return mConfig.sheet_info[i].path;
            }
        }
        return null;
    }
}
