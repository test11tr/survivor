using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public static class SaveManager
{
    private static SaveData saveData = new SaveData();
    private static string saveFilePath;
    private static bool isInitialized = false;
    
    static SaveManager()
    {
        Initialize();
    }

    private static void Initialize()
    {
        if (!isInitialized)
        {
            saveFilePath = Application.persistentDataPath + "/saveData.json";
            Load();
            isInitialized = true;
            Debug.Log("SaveManager initialized");
        }
    }

    public class AutoSaver : MonoBehaviour
    {
        private static float autoSaveInterval = 60f;
        private static float saveTimer;

        private void Start()
        {
            Debug.Log("AutoSaver started...");
        }

        private void Update()
        {
            saveTimer -= Time.deltaTime;
            if (saveTimer <= 0f)
            {
                Save();
                saveTimer = autoSaveInterval;
            }
        }

        private void OnApplicationQuit()
        {
            Debug.Log("Auto-saving on application quit...");
            Save();
        }
    }

    public static void Save()
    {
        Initialize();
        try
        {
            string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
            File.WriteAllText(saveFilePath, json);
            Debug.Log("Data Saved Successfully: " + saveFilePath);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save data: {e.Message}");
        }
    }

    public static void Load()
    {
        try
        {
            if (File.Exists(saveFilePath))
            {
                string json = File.ReadAllText(saveFilePath);
                saveData = JsonConvert.DeserializeObject<SaveData>(json);
                Debug.Log("Data Loaded Successfully");
            }
            else
            {
                Debug.Log("No save file found, creating new save data");
                saveData = new SaveData();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load save data: {e.Message}");
            saveData = new SaveData();
        }
    }

    public static void ResetSave()
    {
        Initialize();
        try
        {
            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
                Debug.Log("Save file deleted successfully");
            }
            
            saveData = new SaveData();
            Save();
            
            Debug.Log("Save data reset complete");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to reset save data: {e.Message}");
        }
    }

    public static void SetBool(string key, bool value)
    {
        saveData.boolValues[key] = value;
    }

    public static bool GetBool(string key, bool defaultValue = false)
    {
        return saveData.boolValues.ContainsKey(key) ? saveData.boolValues[key] : defaultValue;
    }

    public static void SetInt(string key, int value)
    {
        saveData.intValues[key] = value;
    }

    public static int GetInt(string key, int defaultValue = 0)
    {
        return saveData.intValues.ContainsKey(key) ? saveData.intValues[key] : defaultValue;
    }

    public static void SetFloat(string key, float value)
    {
        saveData.floatValues[key] = value;
    }

    public static float GetFloat(string key, float defaultValue = 0f)
    {
        return saveData.floatValues.ContainsKey(key) ? saveData.floatValues[key] : defaultValue;
    }

    public static void SetString(string key, string value)
    {
        saveData.stringValues[key] = value;
    }

    public static string GetString(string key, string defaultValue = "")
    {
        return saveData.stringValues.ContainsKey(key) ? saveData.stringValues[key] : defaultValue;
    }
}
