using UnityEngine;

public class Cheats : MonoBehaviour
{
    [DisplayWithoutEdit()] private GameManager gameManager;
    public WeaponDataSO cheatWeaponData;
    public KeyCode cheatKey;
    public int slotIndex = 0;

    private void Start()
    {
        if(gameManager == null)
            gameManager = GameManager.Instance;
    }

    void Update()
    {
        if (gameManager == null)
            return;
        
        if (Input.GetKeyDown(cheatKey))
        {
            GameManager.Instance.player.gameObject.GetComponent<WeaponManager>().AddWeapon(cheatWeaponData, slotIndex);
        }
    }
}