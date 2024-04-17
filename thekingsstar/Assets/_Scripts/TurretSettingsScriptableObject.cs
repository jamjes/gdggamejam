using UnityEngine;

[CreateAssetMenu]
public class TurretSettingsScriptableObject : ScriptableObject
{
    [Header("Spawn Settings")]
    public int Health;
    public bool StartDelay;
    [Range(3,5)] public float DelayDuration;
    [Range(1, 5)] public float ReloadSpeed;

    [Header("Projectile Settings")]
    public GameObject[] Projectiles;
    public int ProjectileSpeed;
    public int ProjectilePower;
}
