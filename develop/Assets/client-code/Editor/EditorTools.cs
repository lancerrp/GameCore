using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Xml.Serialization;
using UnityEditor.U2D;
using Config;
using System.Reflection;

public class EditorTools
{
    private const string mCodePath = "client-code";

    [MenuItem("Tools/编译code")]
    public static void CompileCode() 
    {
        CSharpCodeProvider complier = new CSharpCodeProvider();
        //设置编译参数
        CompilerParameters paras = new CompilerParameters();
        //引入第三方dll
        paras.ReferencedAssemblies.Add(@"C:\Program Files\Unity\Hub\Editor\2020.3.3f1c1\Editor\Data\Managed\UnityEngine\UnityEngine.dll");
        paras.ReferencedAssemblies.Add(@"C:\Program Files\Unity\Hub\Editor\2020.3.3f1c1\Editor\Data\Managed\UnityEngine\UnityEditor.dll");
        paras.ReferencedAssemblies.Add(@"C:\Program Files\Unity\Hub\Editor\2020.3.3f1c1\Editor\Data\Managed\UnityEngine\UnityEditor.CoreModule.dll");
        paras.ReferencedAssemblies.Add(@"C:\Program Files\Unity\Hub\Editor\2020.3.3f1c1\Editor\Data\Managed\UnityEngine\UnityEngine.CoreModule.dll");
        paras.ReferencedAssemblies.Add(@"System.dll");
        paras.ReferencedAssemblies.Add(@"System.Core.dll");
        paras.ReferencedAssemblies.Add(@"System.Data.dll");
        paras.ReferencedAssemblies.Add(@"System.Xml.dll");
        //引入自定义dll
        //paras.ReferencedAssemblies.Add(@"D:\自定义方法\自定义方法\bin\LogHelper.dll");
        //是否内存中生成输出
        paras.GenerateInMemory = false;
        //是否生成可执行文件
        paras.GenerateExecutable = false;
        paras.OutputAssembly = Application.dataPath + "/game_core.dll";

        //编译代码
        List<FileInfo> fileList = FileUtils.GetFiles(Application.dataPath, ".cs");
        string[] fileNameList = fileList.Select(data => data.FullName).ToArray();
        CompilerResults result = complier.CompileAssemblyFromFile(paras, fileNameList);
        if (result.Errors.HasErrors) 
        {
            Debug.LogError(result.Errors[0].ErrorText);
        }
    }

    [MenuItem("Tools/序列化XML")]
    public static void SerializeConfig() 
    {
        if (PackEditorTools.SerializeConfig())
        {
            EditorUtility.DisplayDialog("提示", "序列化完成", "确定");
        }
        else 
        {
            EditorUtility.DisplayDialog("提示", "序列化失败", "确定");
        }
    }

    [MenuItem("Tools/切换配置加载方式")]
    public static void SwitchConfigLoad()
    {
        if (Application.isPlaying)
        {
            EditorUtility.DisplayDialog("提示", "运行中无法操作", "确定");
            return;
        }
        int value = PlayerPrefs.GetInt(PlayerPrefsDefine.Config_Key, 0);
        PlayerPrefs.SetInt(PlayerPrefsDefine.Config_Key, value == 0 ? 1 : 0);
    }
}
