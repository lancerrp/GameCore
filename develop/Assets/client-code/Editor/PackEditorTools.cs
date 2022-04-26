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
    //[MenuItem("Tools/���")]
    //public static void PackAssetBundle()
    //{
    //    SpriteAtlasUtility.PackAllAtlases(EditorUserBuildSettings.activeBuildTarget);
    //    BuildPipeline.BuildAssetBundles(Application.dataPath + "/dev", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
    //}

    [MenuItem("Tools/���/�����׿����")]
    public static void PackDataAndroid()
    {
        if (!SerializeConfig())
        {
            Debug.LogError("�����ļ����л�ʧ�ܣ�" + DateTime.Now);
            return;
        }
        Debug.Log("�����ļ����л���ɣ�" + DateTime.Now);
        CopyConfigBin(BuildTarget.Android);
        AssetDatabase.Refresh();
        Debug.Log("�����ļ������ɣ�" + DateTime.Now);
    }

    //���л�����
    public static bool SerializeConfig()
    {
        var config = XmlUtils.GetXMLData<XmlConfigGroup>(GameConst.ConfXmlFile);
        string xmlBinPath = string.Format("{0}/bin/xml_config.txt", GameConst.ConfPath);
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
            string binPath = string.Format("{0}/bin/{1}.txt", GameConst.ConfPath, Path.GetFileNameWithoutExtension(info.path));
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
            string binPath = string.Format("{0}/bin/{1}_sheet.txt", GameConst.ConfPath, Path.GetFileNameWithoutExtension(info.path));
            bool success = BinarySerializer(dataArray, binPath);
            if (!success)
            {
                return false;
            }
        }
        return true;
    }

    //������Դ
    public static void CopyConfigBin(BuildTarget target) 
    {
        string path = string.Format("{0}/bin", GameConst.ConfPath);
        string outPath = string.Format("{0}/config/bin", Application.dataPath);
        if (Directory.Exists(outPath)) 
        {
            Directory.Delete(outPath, true);
        }
        if (FileUtils.CopyFolder(path, outPath))
        {
            Debug.Log("�ļ�������ɣ�" + DateTime.Now);
        }
        else 
        {
            Debug.Log("�ļ�����ʧ�ܣ�" + DateTime.Now);
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

    // ������xml�����ļ�����ϸ��Ϣд���ļ�;
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
}
