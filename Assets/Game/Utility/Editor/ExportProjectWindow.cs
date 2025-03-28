using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ExportProjectWindow : EditorWindow
{
    private List<Object> selectedFolders = new List<Object>();

    [MenuItem("Tools/Export Project Settings and Folders Window")]
    public static void ShowWindow()
    {
        GetWindow<ExportProjectWindow>("Export Project Settings & Folders");
    }

    private Vector2 scrollPos;

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Add folders to Export:", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        {
            for (int i = 0; i < selectedFolders.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    selectedFolders[i] = EditorGUILayout.ObjectField(selectedFolders[i], typeof(Object), false);

                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        selectedFolders.RemoveAt(i);
                        break;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();

        GUI.backgroundColor = new Color(1.0f, 0.9f, 0.3f);
        if (GUILayout.Button("Add Folder Element"))
        {
            selectedFolders.Add(null);
        }
        GUI.backgroundColor = Color.white;
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Export Folders & Project Settings"))
        {
            ExportSelectedFolders(includeProjectSettings: true);
        }
        GUI.backgroundColor = Color.white;
        GUI.backgroundColor = Color.blue;
        if (GUILayout.Button("Export Only Folders"))
        {
            ExportSelectedFolders(includeProjectSettings: false);
        }
        GUI.backgroundColor = Color.white;
        EditorGUILayout.Space(5);
    }

    private void ExportSelectedFolders(bool includeProjectSettings)
    {
        List<string> folderPaths = new List<string>();
        foreach (var obj in selectedFolders)
        {
            if (obj == null) continue;

            string path = AssetDatabase.GetAssetPath(obj);
            if (AssetDatabase.IsValidFolder(path))
            {
                folderPaths.Add(path);
            }
            else
            {
                Debug.LogWarning($"'{obj.name}' is not a folder.");
            }
        }

        if (folderPaths.Count == 0)
        {
            EditorUtility.DisplayDialog("Warning", "No folders selected", "Back");
            return;
        }

        ExportProjectSettingsWithFolders.ExportFolders(folderPaths, includeProjectSettings);
    }
}
