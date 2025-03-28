using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;

[InitializeOnLoad]
public static class CustomToolbar
{
    static CustomToolbar()
    {
        ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
    }

    static void OnToolbarGUI()
    {
        GUILayout.FlexibleSpace();

        if (GUILayout.Button(new GUIContent("Reset Save", "Reset Save Data"), EditorStyles.toolbarButton))
        {
            ResetSaveData();
        }
        
        if (GUILayout.Button(new GUIContent("Open Save Folder", "Open Save Folder in file explorer"), EditorStyles.toolbarButton))
        {
            OpenSaveFolder();
        }
        
        if (GUILayout.Button(new GUIContent("1x", "1x Speed"), EditorStyles.toolbarButton))
        {
            x1Speed();
        }
        
        if (GUILayout.Button(new GUIContent("2x", "2x Speed"), EditorStyles.toolbarButton))
        {
            x2Speed();
        }
        
        if (GUILayout.Button(new GUIContent("5x", "5x Speed"), EditorStyles.toolbarButton))
        {
            x5Speed();
        }
    }

    private static void x1Speed()
    {
        Time.timeScale = 1;
    }

    private static void x2Speed()
    {
        Time.timeScale = 2;
    }

    private static void x5Speed()
    {
        Time.timeScale = 5;
    }

    static void ResetSaveData()
    {
        SaveManager.ResetSave();
    }

    static void OpenSaveFolder()
    {
        string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string appDataFolder = Directory.GetParent(localAppData).FullName;
        string targetFolder = Path.Combine(appDataFolder, "LocalLow", "TI Crew", "Racing Legends");

        if (!Directory.Exists(targetFolder))
        {
            Debug.LogWarning("Target folder does not exist: " + targetFolder);
            return;
        }

        EditorUtility.RevealInFinder(targetFolder);
    }
}
