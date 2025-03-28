using UnityEngine;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapon Slots")]
    public Transform[] weaponSlots; // inspector'da ekle
    
    [Header("Runtime Weapons")]
    public List<Weapon> currentWeapons = new List<Weapon>();

    // Basit bir gizmo çizerek slotların yerini görebilirsin
    private void OnDrawGizmosSelected()
    {
        if (weaponSlots != null)
        {
            Gizmos.color = Color.yellow;
            foreach (Transform slot in weaponSlots)
            {
                if (slot != null)
                {
                    // Küçük bir küp çiz
                    Gizmos.DrawCube(slot.position, Vector3.one * 0.1f);
                    // Dilersen label da ekleyebilirsin
                }
            }
        }
    }

    public Weapon AddWeapon(WeaponDataSO weaponData, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= weaponSlots.Length)
        {
            Debug.LogError("Slot index out of range!");
            return null;
        }

        // Slot noktası
        Transform slotTransform = weaponSlots[slotIndex];
        if (slotTransform == null)
        {
            Debug.LogError("Slot transform is null!");
            return null;
        }

        // Yeni bir empty GameObject veya bir "Weapon" prefab
        // Örneğin empty GameObject yaratıp "Weapon" scriptini ekleyelim
        GameObject weaponObj = new GameObject(weaponData.weaponName);
        weaponObj.transform.SetParent(slotTransform, false); // local position ve rotation slot'a göre
        weaponObj.transform.localPosition = Vector3.zero;
        weaponObj.transform.localRotation = Quaternion.identity;

        Weapon weapon = weaponObj.AddComponent<Weapon>();
        weapon.weaponData = weaponData;

        // KarakterStats bulmak için
        CharacterStats charStats = GetComponent<CharacterStats>();
        if (charStats != null)
        {
            weapon.characterStats = charStats;
        }

        // Listeye ekle
        currentWeapons.Add(weapon);

        Debug.Log($"Weapon {weaponData.weaponName} added to slot {slotIndex}");
        return weapon;
    }

    public void RemoveWeapon(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= weaponSlots.Length) return;

        // Slot objesindeki Weapon bileşeni bul
        Transform slotTransform = weaponSlots[slotIndex];
        Weapon weapon = slotTransform.GetComponentInChildren<Weapon>();
        if (weapon != null)
        {
            currentWeapons.Remove(weapon);
            Destroy(weapon.gameObject);
            Debug.Log($"Weapon removed from slot {slotIndex}");
        }
    }
}
