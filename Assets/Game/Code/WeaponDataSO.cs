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
    public GameObject bulletPrefab;       // Mermi prefabini buraya sürükle
    public BulletEffectType bulletEffect; // Örnek: Electric, Poison vs.

    [Header("Misc")]
    public bool isActiveByDefault = true; // Eklendiğinde pasif mi, aktif mi olsun
    
    // İleride special stats ekleyebilirsin (ör. multiShot, piercing, vb.)
}
