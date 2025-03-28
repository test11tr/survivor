using UnityEngine;

public class Player : MonoBehaviour
{
    [DisplayWithoutEdit()] public GameManager gameManager;
    void Start()
    {
        if(gameManager == null)
            gameManager = GameManager.Instance;
        
        gameManager.player = this;
    }
}
