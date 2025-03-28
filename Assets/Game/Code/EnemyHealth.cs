using UnityEngine;
using System;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 50f;
    private float _currentHealth;

    // Düşman canı değiştiğinde UI göstermek istersen
    public event Action<float, float> OnEnemyHealthChanged; 

    private void Start()
    {
        _currentHealth = maxHealth;
    }

    public void TakeDamage(float amount, CharacterStats attackerStats = null)
    {
        _currentHealth -= amount;
        if (_currentHealth < 0) _currentHealth = 0;

        // LifeSteal: eğer saldıran stats varsa
        if (attackerStats != null && attackerStats.lifeSteal.Value > 0)
        {
            float lifeStealAmount = amount * (attackerStats.lifeSteal.Value / 100f);
            attackerStats.health.Value += lifeStealAmount;
            Debug.Log($"LifeSteal triggered. Healed attacker for {lifeStealAmount}");
        }

        OnEnemyHealthChanged?.Invoke(_currentHealth, maxHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    // Elektrik hasarı
    public void TakeElectricDamage(float damageValue, CharacterStats attackerStats = null)
    {
        Debug.Log($"{gameObject.name} is shocked by {damageValue} electric damage.");
        TakeDamage(damageValue, attackerStats);
        // Buraya stun, slow gibi efektler ekleyebilirsin
    }

    // Zehir (damage over time)
    public void ApplyPoison(float tickDamage, float duration, CharacterStats attackerStats = null)
    {
        Debug.Log($"{gameObject.name} is poisoned, taking {tickDamage} dmg/sec for {duration} seconds.");
        StartCoroutine(DoPoisonDamageOverTime(tickDamage, duration, attackerStats));
    }

    private IEnumerator DoPoisonDamageOverTime(float tickDamage, float duration, CharacterStats attackerStats)
    {
        float timeElapsed = 0f;
        float tickInterval = 1f; // her 1 saniyede bir hasar

        while (timeElapsed < duration)
        {
            yield return new WaitForSeconds(tickInterval);

            TakeDamage(tickDamage, attackerStats);
            timeElapsed += tickInterval;
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} is dead!");
        // Örn: animasyon, loot düşürme, xp verme
        Destroy(gameObject);
    }
}
