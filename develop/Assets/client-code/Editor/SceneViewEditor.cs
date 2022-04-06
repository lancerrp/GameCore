using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class SceneViewEditor
{
    static SceneViewEditor() 
    {
        SceneView.duringSceneGui += OnSceneFunc;
    }

    static public void OnSceneFunc(SceneView sceneView) 
    {
        int fontSize = GUI.skin.label.fontSize;
        Handles.BeginGUI();
        GUI.skin.label.fontSize = 20;
        string strInfo = "";

        if (GameConfigManager.LoadBinaryData)
        {
            GUI.color = Color.red;
            strInfo = "Editor下正在使用二进制文件加载配置";
        }
        else
        {
            GUI.color = Color.green;
            strInfo = "Editor下正在使用xml源文件加载配置";
        }
        GUILayout.Label(strInfo);

        GUI.skin.label.fontSize = fontSize;
        Handles.EndGUI();
    }
}
