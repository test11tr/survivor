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
    public BulletEffectType bulletEffectType;
    public float baseFireRate = 1f;
    public float baseDamage = 10f;
    public float baseProjectileCount = 1f;

    private CharacterStats _characterStats;
    private float _fireTimer;

    // Silah aktif mi, pasif mi
    public bool isActive = true;

    private void Start()
    {
        // Bulabildiğin bir yerden CharacterStats al
        // Örneğin parent objede duruyorsa:
        _characterStats = GetComponentInParent<CharacterStats>();
        // ya da bir manager varsa oradan da referans alabilirsin
    }

    private void Update()
    {
        if (!isActive) return;

        _fireTimer -= Time.deltaTime;
        if (_fireTimer <= 0f)
        {
            FireWeapon();
            // Karakterin fireRate statını da hesaba kat
            float totalFireRate = baseFireRate * (1 + _characterStats.fireRate.Value / 100f);
            _fireTimer = 1f / totalFireRate; 
        }
    }

    private void FireWeapon()
    {
        // Projeyi ayır: Kaç mermi? Hangi açılarla?
        int projectileCount = Mathf.RoundToInt(baseProjectileCount + _characterStats.projectileCount.Value);

        for (int i = 0; i < projectileCount; i++)
        {
            // Mermiyi instantiate et.
            // Projectile açısını ayarla. 
            // Örnek:
            Vector3 spawnPos = transform.position; // Silahın konumu etrafında
            Quaternion spawnRot = Quaternion.Euler(0f, (i - projectileCount/2f) * _characterStats.projectileAngle.Value, 0f);

            GameObject bulletObj = Instantiate(Resources.Load<GameObject>("BulletPrefab"), spawnPos, spawnRot);
            Bullet bullet = bulletObj.GetComponent<Bullet>();

            // Mermiye final damage değerini geç.
            // (Karakterin damage statı + weapon baseDamage)
            float finalDamage = baseDamage + _characterStats.damage.Value;
            bullet.Initialize(finalDamage, bulletEffectType);
        }
    }
}
