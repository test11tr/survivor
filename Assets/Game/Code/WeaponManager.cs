using UnityEngine;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    public List<Weapon> currentWeapons = new List<Weapon>();

    // Mesela yeni bir silah ekleme
    public void AddWeapon(GameObject weaponPrefab)
    {
        // Karakterin "WeaponHolder" gibi bir child objesi varsa oraya ekle
        GameObject newWeaponObj = Instantiate(weaponPrefab, transform);
        Weapon newWeapon = newWeaponObj.GetComponent<Weapon>();

        currentWeapons.Add(newWeapon);
        Debug.Log("New weapon added: " + newWeapon.name);
    }
    
    public void RemoveWeapon(Weapon weapon)
    {
        if(currentWeapons.Contains(weapon))
        {
            currentWeapons.Remove(weapon);
            Destroy(weapon.gameObject);
        }
    }
}
