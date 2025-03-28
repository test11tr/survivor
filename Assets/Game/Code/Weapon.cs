using UnityEngine;

public enum BulletEffectType
{
    None,
    Electric,
    Poison
    // vs...
}

public class Weapon : MonoBehaviour
{
    [Header("ScriptableObject")]
    public WeaponDataSO weaponData;

    [Header("References")]
    public CharacterStats characterStats; // Karakterin statlarını tutmak için
    
    private float _fireTimer;
    private bool _isActive;

    private void Start()
    {
        // weaponData'daki default ayarı al
        _isActive = weaponData != null && weaponData.isActiveByDefault;

        // Eğer CharacterStats bu objenin parent’ında veya 
        // yukarıdaki bir manager’da ise oradan alabilirsin:
        if (characterStats == null)
        {
            characterStats = GetComponentInParent<CharacterStats>();
        }
    }

    private void Update()
    {
        // Eğer silah aktif değilse ateşleme
        if (!_isActive) return;

        _fireTimer -= Time.deltaTime;
        if (_fireTimer <= 0f)
        {
            FireWeapon();
            float totalFireRate = (weaponData.baseFireRate + characterStats.fireRate.Value);
            // Örneğin: 1 / totalFireRate 
            // (burada tam formülünü dilediğin gibi ayarla)
            _fireTimer = 1f / totalFireRate;
        }
    }

    private void FireWeapon()
    {
        // Kaç mermi? (WeaponData + CharacterStats’tan gelen)
        int projectileCount = Mathf.RoundToInt(weaponData.baseProjectileCount + characterStats.projectileCount.Value);

        for (int i = 0; i < projectileCount; i++)
        {
            float angleStep = weaponData.baseProjectileAngle + characterStats.projectileAngle.Value;
            float angleOffset = (i - (projectileCount - 1) / 2f) * angleStep;

            // Bu silah objesinin konum/rotasyonundan spawn edelim
            Vector3 spawnPos = transform.position;
            Quaternion spawnRot = Quaternion.Euler(transform.eulerAngles.x,
                                                   transform.eulerAngles.y + angleOffset,
                                                   transform.eulerAngles.z);

            // Mermi prefabini, weaponData’dan al
            if (weaponData.bulletPrefab != null)
            {
                GameObject bulletObj = Instantiate(weaponData.bulletPrefab, spawnPos, spawnRot);
                // Bullet scriptini al
                Bullet bullet = bulletObj.GetComponent<Bullet>();
                if (bullet != null)
                {
                    // Hasar hesaplama
                    float finalDamage = weaponData.baseDamage + characterStats.damage.Value;
                    bullet.Initialize(finalDamage, weaponData.bulletEffect, characterStats);
                    // characterStats => Attacker stats (lifesteal vs. için)
                }
            }
        }
    }

    // Aktif/Pasif kontrolü
    public void SetActive(bool value)
    {
        _isActive = value;
    }
}
