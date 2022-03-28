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
            strInfo = "Editor������ʹ�ö������ļ���������";
        }
        else
        {
            GUI.color = Color.green;
            strInfo = "Editor������ʹ��xmlԴ�ļ���������";
        }
        GUILayout.Label(strInfo);

        GUI.skin.label.fontSize = fontSize;
        Handles.EndGUI();
    }
}
