using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public static class ExportProjectSettingsWithFolders
{
    public static void ExportFolders(List<string> folderPaths, bool includeProjectSettings)
    {
        List<(string originalPath, string movedPath)> movedFolders = new List<(string, string)>();

        foreach (var originalFolderPath in folderPaths)
        {
            if (originalFolderPath.StartsWith("Packages/"))
                continue;

            string folderName = Path.GetFileName(originalFolderPath);
            string targetPath = "Assets/" + folderName;

            if (originalFolderPath.Equals(targetPath, System.StringComparison.OrdinalIgnoreCase))
                continue;

            if (AssetDatabase.IsValidFolder(targetPath))
            {
                AssetDatabase.DeleteAsset(targetPath);
            }

            string error = AssetDatabase.MoveAsset(originalFolderPath, targetPath);
            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError($"Folder moving error: {originalFolderPath} -> {targetPath}\n{error}");
            }
            else
            {
                movedFolders.Add((originalFolderPath, targetPath));
            }
        }

        AssetDatabase.Refresh();

        List<string> exportPaths = new List<string>();

        if (includeProjectSettings)
        {
            string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
            foreach (string path in allAssetPaths)
            {
                if (path.StartsWith("ProjectSettings/")
                    && !path.EndsWith("ProjectSettings/ProjectSettings.asset"))
                {
                    exportPaths.Add(path);
                }
            }
        }

        foreach (var (originalPath, movedPath) in movedFolders)
        {
            var guids = AssetDatabase.FindAssets("", new[] { movedPath });
            foreach (var guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (!exportPaths.Contains(assetPath))
                {
                    exportPaths.Add(assetPath);
                }
            }
        }

        foreach (var folderPath in folderPaths)
        {
            string folderName = Path.GetFileName(folderPath);
            string targetPath = "Assets/" + folderName;
            if (AssetDatabase.IsValidFolder(targetPath))
            {
                var guids = AssetDatabase.FindAssets("", new[] { targetPath });
                foreach (var guid in guids)
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    if (!exportPaths.Contains(assetPath))
                    {
                        exportPaths.Add(assetPath);
                    }
                }
            }
        }

        string defaultName = PlayerSettings.productName + "_SettingsFolders.unitypackage";
        if (!includeProjectSettings)
        {
            defaultName = PlayerSettings.productName + "_FoldersOnly.unitypackage";
        }

        string savePath = EditorUtility.SaveFilePanel(
            "Export Unity Package",
            Application.dataPath,
            defaultName,
            "unitypackage"
        );

        if (string.IsNullOrEmpty(savePath))
        {
            RestoreFolders(movedFolders);
            return;
        }

        AssetDatabase.ExportPackage(
            exportPaths.ToArray(),
            savePath,
            ExportPackageOptions.Recurse
        );

        Debug.Log("Package exported to: " + savePath);

        RestoreFolders(movedFolders);
        AssetDatabase.Refresh();
    }

    private static void RestoreFolders(List<(string originalPath, string movedPath)> movedFolders)
    {
        foreach (var (originalPath, movedPath) in movedFolders)
        {
            string error = AssetDatabase.MoveAsset(movedPath, originalPath);
            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError($"Folder re-move error! {movedPath} -> {originalPath}\n{error}");
            }
        }
    }
}
