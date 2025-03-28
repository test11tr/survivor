using UnityEngine;
using System;

[RequireComponent(typeof(CharacterStats))]
public class PlayerHealth : MonoBehaviour
{
    private CharacterStats _characterStats;

    // Sağlık değiştiğinde UI'ya veya başka sisteme haber vermek için
    public event Action<float, float> OnHealthChanged;

    private void Awake()
    {
        _characterStats = GetComponent<CharacterStats>();
    }

    public void TakeDamage(float amount, CharacterStats attackerStats = null)
    {
        // Hasarı uygula
        _characterStats.health.Value -= amount;

        // Minimum 0
        if (_characterStats.health.Value < 0)
            _characterStats.health.Value = 0;

        // LifeSteal örneği – Tam tersi duruma da koyabilirsin (EnemyHealth’te).
        // Burada “attackerStats” => düşman karakter statları ise 
        // "Attacker oynamasın" diyorsan bu bloğu kaldırabilirsin.
        if (attackerStats != null && attackerStats.lifeSteal.Value > 0)
        {
            float lifeStealAmount = amount * (attackerStats.lifeSteal.Value / 100f);
            attackerStats.health.Value += lifeStealAmount;
            // Burada PlayerHealth'te event tetiklenmiyor ama 
            // attackerStats'a da istersen "OnHealthChanged" vs. ekleyebilirsin.
        }

        // Event ile UI'ye haber ver
        OnHealthChanged?.Invoke(_characterStats.health.Value, _characterStats.health.baseValue);

        if (_characterStats.health.Value <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player is Dead!");
        // Ölüm animasyonu, game over vs. 
        // isControlsActive = false, GameManager'a haber ver, vb.
    }

    // Can doldurma fonksiyonu
    public void Heal(float amount)
    {
        _characterStats.health.Value += amount;
        if (_characterStats.health.Value > _characterStats.health.baseValue)
            _characterStats.health.Value = _characterStats.health.baseValue;

        OnHealthChanged?.Invoke(_characterStats.health.Value, _characterStats.health.baseValue);
    }
}
