using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.U2D;
using System.Reflection;
using Config;
using System;
using System.IO;

public class PackEditorTools
{
    //[MenuItem("Tools/打包")]
    //public static void PackAssetBundle()
    //{
    //    SpriteAtlasUtility.PackAllAtlases(EditorUserBuildSettings.activeBuildTarget);
    //    BuildPipeline.BuildAssetBundles(Application.dataPath + "/dev", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
    //}

    [MenuItem("Tools/打包/打包安卓配置")]
    public static void PackDataAndroid()
    {
        if (!SerializeConfig())
        {
            Debug.LogError("配置文件序列化失败！" + DateTime.Now);
            return;
        }
        Debug.Log("配置文件序列化完成！" + DateTime.Now);

        string path = GetPackDataPath(BuildTarget.Android);
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
        Directory.CreateDirectory(path);

        string zipPath = string.Format("{0}/bin", GameConst.ConfPath);
        string outFile = string.Format("{0}/bin.zip", path);

        ZipFileCallBack callBack = new ZipFileCallBack((result) => 
        {
            if (result)
            {
                Debug.Log("配置文件压缩完成！" + DateTime.Now);
                CopyPackData(BuildTarget.Android);
                AssetDatabase.Refresh();
            }
            else 
            {
                Debug.Log("配置文件压缩失败！" + DateTime.Now);
            }
        });
        ZipUtility.Zip(new string[] { zipPath }, outFile, null, callBack);
    }

    //序列化配置
    public static bool SerializeConfig()
    {
        var config = XmlUtils.GetXMLData<XmlConfigGroup>(GameConst.ConfXmlFile);
        string xmlBinPath = string.Format("{0}/bin/xml_config.bin", GameConst.ConfPath);
        if (!BinarySerializer(config, xmlBinPath))
        {
            return false;
        }

        Assembly assm = Assembly.Load("Assembly-CSharp");
        for (int i = 0; i < config.xml_info.Count; i++)
        {
            var info = config.xml_info[i];
            string path = string.Format("{0}/{1}", GameConst.ConfPath, info.path);
            string className = string.Format("Config.{0}", info.xml_type);
            Type type = assm.GetType(className);
            var data = XmlUtils.GetXMLData(type, path);
            string binPath = string.Format("{0}/bin/{1}.bin", GameConst.ConfPath, Path.GetFileNameWithoutExtension(info.path));
            bool success = BinarySerializer(data, binPath);
            if (!success) 
            {
                return false;
            }
        }
        for (int i = 0; i < config.sheet_info.Count; i++)
        {
            var info = config.sheet_info[i];
            string path = string.Format("{0}/{1}", GameConst.ConfPath, info.path);
            string className = string.Format("Config.{0}", info.xml_type);
            Type type = assm.GetType(className);
            List<FileInfo> files = FileUtils.GetFiles(path, ".xml");
            object[] dataArray = new object[files.Count];
            for (int j = 0; j < files.Count; j++)
            {
                var data = XmlUtils.GetXMLData(type, files[j].FullName);
                dataArray[j] = data;
            }
            string binPath = string.Format("{0}/bin/{1}_sheet.bin", GameConst.ConfPath, Path.GetFileNameWithoutExtension(info.path));
            bool success = BinarySerializer(dataArray, binPath);
            if (!success)
            {
                return false;
            }
        }
        return true;
    }

    //拷贝资源
    public static void CopyPackData(BuildTarget target) 
    {
        string path = GetPackDataPath(target);
        string outPath = string.Format("{0}/pack_data", Application.streamingAssetsPath);
        if (Directory.Exists(Application.streamingAssetsPath)) 
        {
            Directory.Delete(Application.streamingAssetsPath, true);
        }
        if (FileUtils.CopyFolder(path, outPath))
        {
            Debug.Log("文件拷贝完成！" + DateTime.Now);
        }
        else 
        {
            Debug.Log("文件拷贝失败！" + DateTime.Now);
        }
    }

    //Object to Bin
    private static bool BinarySerializer(object obj, string path)
    {
        bool success = true;
        Stream stream = null;
        try
        {
            stream = new FileStream(path, FileMode.Create);
            ProtoBuf.Serializer.Serialize(stream, obj);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            success = false;
        }
        finally
        {
            stream?.Dispose();
        }
        return success;
    }

    // 将所有xml配置文件的详细信息写入文件;
    public static void DetailPathWriteToFile<T>(T xmlData, string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
        StreamWriter writer = null;
        System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(T));
        try
        {
            writer = new StreamWriter(fs);
            x.Serialize(writer, xmlData);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            EditorUtility.DisplayDialog("error", "xml write error : " + path, "OK");
        }
        finally
        {
            if (writer != null)
            {
                writer.Close();
            }
        }
    }

    private static string GetPackDataPath(BuildTarget target)
    {
        string path = "";
        if (target == BuildTarget.Android)
        {
            path = string.Format("{0}/../../pack_data/DataAndroid", Application.dataPath);
        }
        else if (target == BuildTarget.iOS)
        {
            path = string.Format("{0}/../../pack_data/DataIOS", Application.dataPath);
        }
        else 
        {
            path = string.Format("{0}/../../pack_data/DataPC", Application.dataPath);
        }
        return path;
    }
}
