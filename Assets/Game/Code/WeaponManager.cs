using UnityEngine;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapon Slots (Local Positions)")]
    public Vector3[] weaponSlotsPositions;  // Inspector'da ayarlayacağın slot konumları

    [Header("Runtime Weapons")]
    public Weapon[] currentWeaponSlots;     // Her slotun o an taşıdığı Weapon referansı
    public List<Weapon> currentWeapons = new List<Weapon>();

    private void Awake()
    {
        // Dizilerin uzunluğu eşit olsun diye kontrol:
        // (İlk kez sahneye eklerken bu dizi boş olabilir, o yüzden boyut eşitleyebilirsin)
        if (currentWeaponSlots == null || currentWeaponSlots.Length != weaponSlotsPositions.Length)
        {
            currentWeaponSlots = new Weapon[weaponSlotsPositions.Length];
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        // weaponSlotsPositions dizisi varsa, her birine bir küre çiz
        if (weaponSlotsPositions != null)
        {
            for (int i = 0; i < weaponSlotsPositions.Length; i++)
            {
                // LocalPosition -> WorldPosition
                Vector3 worldPos = transform.TransformPoint(weaponSlotsPositions[i]);
                Gizmos.DrawWireSphere(worldPos, 0.2f);

                // Ayrıca küçük bir label da ekleyebilirsin
                // Gizmos.color = Color.white;
                // UnityEditor.Handles.Label(worldPos, "Slot " + i);
            }
        }
    }

    public Weapon AddWeapon(WeaponDataSO weaponData, int slotIndex)
    {
        // Slot index kontrolü
        if (slotIndex < 0 || slotIndex >= weaponSlotsPositions.Length)
        {
            Debug.LogError("Slot index out of range!");
            return null;
        }

        // Silah prefab’i var mı?
        if (weaponData.weaponPrefab == null)
        {
            Debug.LogError($"WeaponData {weaponData.name} has no weaponPrefab assigned!");
            return null;
        }

        // Prefab instantiate
        // Parent olarak bu WeaponManager objesini alıyoruz (dilersen Player objesi de olabilir).
        GameObject weaponObj = Instantiate(weaponData.weaponPrefab, transform);

        // LocalPosition ve localRotation
        weaponObj.transform.localPosition = weaponSlotsPositions[slotIndex];
        weaponObj.transform.localRotation = Quaternion.identity;

        // Weapon scripti al
        Weapon weapon = weaponObj.GetComponent<Weapon>();
        if (weapon == null)
        {
            Debug.LogError($"Weapon prefab {weaponObj.name} doesn't have a 'Weapon' component!");
            return null;
        }

        // Scriptable data ataması
        weapon.weaponData = weaponData;

        // KarakterStats referansı
        CharacterStats charStats = GetComponent<CharacterStats>();
        if (charStats != null)
        {
            weapon.characterStats = charStats;
        }

        // Slot dizisinde tut
        // Önce eğer o slotta daha önce silah varsa (örn. slot doluysa) temizlemeyi düşünebilirsin.
        if (currentWeaponSlots[slotIndex] != null)
        {
            // Belki önceki silahı removeWeapon(slotIndex) ile temizleyebilirsin
            RemoveWeapon(slotIndex);
        }

        currentWeaponSlots[slotIndex] = weapon;        
        currentWeapons.Add(weapon);

        Debug.Log($"Weapon {weaponData.weaponName} added to slot {slotIndex}");
        return weapon;
    }

    public void RemoveWeapon(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= weaponSlotsPositions.Length)
        {
            Debug.LogError("Slot index out of range!");
            return;
        }

        Weapon weapon = currentWeaponSlots[slotIndex];
        if (weapon != null)
        {
            // Listeden çıkar
            if (currentWeapons.Contains(weapon))
            {
                currentWeapons.Remove(weapon);
            }
            // Oyun objesini yok et
            Destroy(weapon.gameObject);

            // Dizideki referansı sıfırla
            currentWeaponSlots[slotIndex] = null;

            Debug.Log($"Weapon removed from slot {slotIndex}");
        }
    }
}
