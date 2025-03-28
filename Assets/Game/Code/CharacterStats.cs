using UnityEngine;
using System;

[Serializable]
public class Stat
{
    public float baseValue; 
    [DisplayWithoutEdit()] public float _value;

    public float Value 
    {
        get => _value; 
        set => _value = value; 
    }
}

public class CharacterStats : MonoBehaviour
{
    [Header("Scriptable Base Stats")]
    public BaseStatsSO baseStats; // Sürükle-bırak yapacağız

    [Header("Runtime Stats")]
    public Stat health;
    public Stat moveSpeed;
    public Stat rotateSpeed;
    public Stat fireRate;
    public Stat damage;
    public Stat projectileCount;
    public Stat projectileAngle;
    public Stat lifeSteal;

    // Bu event, statlar değiştiğinde (örneğin upgrade alırken) tetiklenebilir.
    public event Action OnStatsChanged;

    private void Awake()
    {
        // Eğer ScriptableObject atadıysan onunla baseValue'ları doldur
        if (baseStats != null)
        {
            health.baseValue           = baseStats.baseHealth;
            moveSpeed.baseValue        = baseStats.baseMoveSpeed;
            rotateSpeed.baseValue      = baseStats.rotateSpeed;
            fireRate.baseValue         = baseStats.baseFireRate;
            damage.baseValue           = baseStats.baseDamage;
            projectileCount.baseValue  = baseStats.baseProjectileCount;
            projectileAngle.baseValue  = baseStats.baseProjectileAngle;
            lifeSteal.baseValue        = baseStats.baseLifeSteal;   
        }

        // Ardından runtime değerleri, baseValue’dan başlat
        health.Value          = health.baseValue;
        moveSpeed.Value       = moveSpeed.baseValue;
        rotateSpeed.Value     = rotateSpeed.baseValue;
        fireRate.Value        = fireRate.baseValue;
        damage.Value          = damage.baseValue;
        projectileCount.Value = projectileCount.baseValue;
        projectileAngle.Value = projectileAngle.baseValue;
        lifeSteal.Value       = lifeSteal.baseValue;
    }

    // Stat değişikliği metodu
    public void ModifyStat(Stat stat, float amount)
    {
        stat.Value += amount;
        OnStatsChanged?.Invoke();
    }

    public void IncreaseDamage(float amount)
    {
        ModifyStat(damage, amount);
    }

    public void IncreaseFireRate(float amount)
    {
        ModifyStat(fireRate, amount);
    }

    // vb...
}
