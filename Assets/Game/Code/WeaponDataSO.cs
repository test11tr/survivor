using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "MyGame/Weapon/WeaponData")]
public class WeaponDataSO : ScriptableObject
{
    [Header("Basic Weapon Settings")]
    public string weaponName;
    public float baseDamage = 10f;
    public float baseFireRate = 1f;
    public float baseProjectileCount = 1f;
    public float baseProjectileAngle = 10f;

    [Header("Bullet Settings")]
    public GameObject bulletPrefab;       // Mermi prefabı
    public BulletEffectType bulletEffect; // Merminin efekt tipi

    [Header("Weapon Prefab")]
    public GameObject weaponPrefab;       // Silahın kendisinin prefabı (model, vb.)

    [Header("Misc")]
    public bool isActiveByDefault = true;
}

public enum BulletEffectType
{
    None,
    Electric,
    Poison
}