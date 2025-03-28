using UnityEngine;
using System;

[RequireComponent(typeof(CharacterStats))]
public class PlayerHealth : MonoBehaviour
{
    private CharacterStats _characterStats;

    // Sağlık değiştiğinde UI'ye haber vermek vs. için
    public event Action<float, float> OnHealthChanged; 
    // float current, float max gibisinden

    private void Awake()
    {
        _characterStats = GetComponent<CharacterStats>();
    }

    public void TakeDamage(float amount)
    {
        // Hasarı uygula
        _characterStats.health.Value -= amount;

        // Minimum 0
        if (_characterStats.health.Value < 0)
            _characterStats.health.Value = 0;

        // Event ile UI'ya haber
        OnHealthChanged?.Invoke(_characterStats.health.Value, _characterStats.health.baseValue);

        if (_characterStats.health.Value <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player is Dead!");
        // Ölüm animasyonu, game over ekranı vs. 
        // isControlsActive = false vs. 
        // Belki GameManager’a “Oyuncu öldü” haberi yolla
    }

    // Örnek: can doldurma
    public void Heal(float amount)
    {
        _characterStats.health.Value += amount;
        if (_characterStats.health.Value > _characterStats.health.baseValue)
            _characterStats.health.Value = _characterStats.health.baseValue;

        OnHealthChanged?.Invoke(_characterStats.health.Value, _characterStats.health.baseValue);
    }
}
