using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public LayerMask enemyLayer;
    private float _damage;
    private BulletEffectType _effectType;
    private CharacterStats _attackerStats;

    public void Initialize(float damage, BulletEffectType effectType, CharacterStats attackerStats = null)
    {
        _damage = damage;
        _effectType = effectType;
        _attackerStats = attackerStats;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1 << other.gameObject.layer => Bulunan gameobject'in layer bit'i
        // Bu bit'i "enemyLayer" maskesiyle AND'liyoruz.
        // Sonuç 0'dan farklıysa demek ki "enemyLayer" ile uyuşuyor.
        if ((enemyLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            // Bu obje "enemyLayer" kapsamında
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth)
            {
                enemyHealth.TakeDamage(_damage, _attackerStats);
                // destroy bullet, vs...
                ApplyEffect(enemyHealth);
            }
            
            Destroy(gameObject);
        }
    }

    private void ApplyEffect(EnemyHealth enemy)
    {
        switch (_effectType)
        {
            case BulletEffectType.Electric:
                enemy.TakeElectricDamage(5f);
                break;
            case BulletEffectType.Poison:
                enemy.ApplyPoison(2f, 5f);
                break;
            default:
                break;
        }
    }
}