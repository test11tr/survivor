using UnityEngine;

[CreateAssetMenu(fileName = "BaseStats", menuName = "T11/Character/Base Stats")]
public class BaseStatsSO : ScriptableObject
{
    [Header("Core Stats")]
    public float baseHealth = 100f;
    public float baseMoveSpeed = 5f;
    public float rotateSpeed = 10f;
    public float baseFireRate = 1f;
    public float baseDamage = 10f;
    public float baseProjectileCount = 1f;
    public float baseProjectileAngle = 0f;
    public float baseLifeSteal = 0f;
    // İhtiyacına göre ekle (shield, vb.)
}
