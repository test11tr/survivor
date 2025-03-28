using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private float _damage;
    private BulletEffectType _effectType;

    public void Initialize(float damage, BulletEffectType effectType)
    {
        _damage = damage;
        _effectType = effectType;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Belli layer/Tag check et (enemy vs.)
        if (other.CompareTag("Enemy"))
        {
            // Damage uygulama
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(_damage);
                ApplyEffect(enemyHealth);
            }

            // Mermi yok olsun
            Destroy(gameObject);
        }
    }

    private void ApplyEffect(EnemyHealth enemy)
    {
        switch (_effectType)
        {
            case BulletEffectType.Electric:
                // Elektrik damage veya stun vs. 
                enemy.TakeElectricDamage(5f);
                break;
            case BulletEffectType.Poison:
                // Poison damage over time vs.
                enemy.ApplyPoison(2f, 5f); // 2f damage, 5 saniye?
                break;
        }
    }
}
