using UnityEngine;

public class DataInitializer : MonoBehaviour
{
    [Foldout("Default Data", foldEverything = true, styled = true, readOnly = false)]
    public int coinCount;
    public int currentPlayerLevel;
    public int currentExperience;

    private void Awake()
    {
        if(SaveManager.GetInt("NewPlayer",0) == 0)
        {
            //FIRST TIME PLAYER ONBOARDING
        }
        else
        {
            //SKIP
        }
    }
}
