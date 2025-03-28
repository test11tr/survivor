using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Foldout("Game Manager Important References", foldEverything = true, styled = true, readOnly = false)]
    public static GameManager Instance;
    public DataInitializer dataInitializer;
    public DelayHelper delayHelper;
    
    [Foldout("Game Manager", foldEverything = true, styled = true, readOnly = true)]
    public Player player;

    [Foldout("Gameplay Settings", foldEverything = true, styled = true, readOnly = false)]
    public int targetFPS;
    [DisplayWithoutEdit()] public bool isGamePaused;

    private void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        Application.targetFrameRate = targetFPS;
    }

    private void Start()
    {
        GameObject autoSaveObject = new GameObject("AutoSaver");
        autoSaveObject.AddComponent<SaveManager.AutoSaver>();
        autoSaveObject.transform.SetParent(transform);
    }
}
