using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 50f;
    private float _currentHealth;

    private void Start()
    {
        _currentHealth = maxHealth;
    }

    // Normal hasar
    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        if (_currentHealth < 0) _currentHealth = 0;

        Debug.Log($"{gameObject.name} took damage: {amount}, current HP: {_currentHealth}");

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    // Elektrik hasarı (örnek)
    // İstersen çarpan, stun, ek hasar gibi şeyler yapabilirsin
    public void TakeElectricDamage(float damageValue)
    {
        // Örneğin direkt ek hasar:
        Debug.Log($"{gameObject.name} is shocked by electric damage: {damageValue}");
        TakeDamage(damageValue);

        // Buraya stun, yavaşlatma vs. efekti ekleyebilirsin
        // Örneğin:
        // StartCoroutine(ApplyStun(2f));
    }

    // Zehir hasarı (örnek)
    // Bu genelde "Damage over time" şeklinde yapılır.
    public void ApplyPoison(float tickDamage, float duration)
    {
        // Bu metod, coroutine çağırıyor
        Debug.Log($"{gameObject.name} is poisoned for {duration} seconds, {tickDamage} damage per tick.");
        StartCoroutine(DoPoisonDamageOverTime(tickDamage, duration));
    }

    private IEnumerator DoPoisonDamageOverTime(float tickDamage, float duration)
    {
        float timeElapsed = 0f;
        float tickInterval = 1f; // Her 1 saniyede bir hasar verecek şekilde

        while (timeElapsed < duration)
        {
            yield return new WaitForSeconds(tickInterval);

            // Her "tickInterval" süresinde hasar
            TakeDamage(tickDamage);
            timeElapsed += tickInterval;
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} is dead!");
        // Ölüm animasyonu, loot düşürme vs. 
        // Sonra yok et
        Destroy(gameObject);
    }
}
