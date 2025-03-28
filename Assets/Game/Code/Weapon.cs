using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("ScriptableObject")]
    public WeaponDataSO weaponData;

    [Header("References")]
    public CharacterStats characterStats;

    private float _fireTimer;
    private bool _isActive;

    private void Start()
    {
        if (weaponData == null)
        {
            Debug.LogWarning($"{name} has no WeaponData assigned! Possibly assigned by manager at runtime.");
        }
        else
        {
            _isActive = weaponData.isActiveByDefault;
        }

        if (characterStats == null)
        {
            characterStats = GetComponentInParent<CharacterStats>();
        }
    }

    public void FireWeapon(Vector3 aimPoint)
    {
        // 1) Silah, aimPoint'e doğru dönmek istiyorsa (opsiyonel):
        Vector3 fixedAim = new Vector3(aimPoint.x, transform.position.y, aimPoint.z);
        transform.LookAt(fixedAim);

        // 2) Mermiyi spawn et
        // (Bu kısım senin "FireWeapon" mantığınla aynı)
        int projectileCount = Mathf.RoundToInt(weaponData.baseProjectileCount + characterStats.projectileCount.Value);
        float dmg = weaponData.baseDamage + characterStats.damage.Value;
        float angleStep = weaponData.baseProjectileAngle + characterStats.projectileAngle.Value;

        for (int i = 0; i < projectileCount; i++)
        {
            float angleOffset = (i - (projectileCount - 1) / 2f) * angleStep;
            Quaternion spawnRot = Quaternion.Euler(transform.eulerAngles.x,
                transform.eulerAngles.y + angleOffset,
                transform.eulerAngles.z);
            Vector3 spawnPos = transform.position;

            if (weaponData.bulletPrefab != null)
            {
                GameObject bulletObj = Instantiate(weaponData.bulletPrefab, spawnPos, spawnRot);
                Bullet bullet = bulletObj.GetComponent<Bullet>();
                if (bullet != null)
                {
                    bullet.Initialize(dmg, weaponData.bulletEffect, characterStats);
                }
            }
        }
    }

    // Silah aktif/pasif yönetimi
    public void SetActive(bool value)
    {
        _isActive = value;
    }
}
