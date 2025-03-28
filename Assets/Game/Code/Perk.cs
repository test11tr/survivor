using UnityEngine;

[CreateAssetMenu(fileName = "NewPerk", menuName = "T11/Perk", order = 0)]
public class Perk : ScriptableObject
{
    public string perkName;
    public string description;
    public Sprite icon;

    public float damageBonus;
    public float fireRateBonus;
    public float projectileCountBonus;
    public bool unlockNewWeapon;
    public WeaponDataSO weaponPrefabToUnlock;

    // Mesela perk uygulama metodu
    public void ApplyPerk(CharacterStats stats, WeaponManager weaponManager)
    {
        stats.ModifyStat(stats.damage, damageBonus);
        stats.ModifyStat(stats.fireRate, fireRateBonus);
        stats.ModifyStat(stats.projectileCount, projectileCountBonus);

        if (unlockNewWeapon && weaponPrefabToUnlock != null)
        {
            weaponManager.AddWeapon(weaponPrefabToUnlock, 0);
        }
    }
}
