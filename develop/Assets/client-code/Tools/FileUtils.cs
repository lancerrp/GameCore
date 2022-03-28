using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class FileUtils
{
    public static List<FileInfo> GetFiles(string path, string extName) 
    {
        List<FileInfo> result = new List<FileInfo>();
        if (Directory.Exists(path))
        {
            DirectoryInfo direction = new DirectoryInfo(path);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(extName))
                {
                    result.Add(files[i]);
                }
            }
        }
        return result;
    }

    //复制文件夹
    public static bool CopyFolder(string fromPath, string outPath) 
    {
        if (!Directory.Exists(fromPath)) 
        {
            return false;
        }
        if (!Directory.Exists(outPath)) 
        {
            try
            {
                Directory.CreateDirectory(outPath);
            }
            catch (System.Exception)
            {
                Debug.LogError("创建文件夹失败");
                return false;
            }
        }
        List<string> files = new List<string>(Directory.GetFiles(fromPath));
        foreach (var file in files)
        {
            string outFile = Path.Combine(new string[] { outPath, Path.GetFileName(file) });
            File.Copy(file, outFile, true);
        }
        List<string> folders = new List<string>(Directory.GetDirectories(fromPath));
        foreach (var fold in folders)
        {
            string outFold = Path.Combine(new string[] { outPath, Path.GetFileName(fold) });
            if (!CopyFolder(fold, outFold)) 
            {
                return false;
            }
        }
        return true;
    }
}
