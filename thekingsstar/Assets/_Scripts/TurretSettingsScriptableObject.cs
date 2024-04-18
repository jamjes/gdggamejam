using UnityEngine;

[CreateAssetMenu]
public class TurretSettingsScriptableObject : ScriptableObject
{
    [Header("Spawn Settings")]
    [Range(0,10)] public float Health;
    [Range(0,6)] public float DelayDuration;
    [Range(2, 6)] public float ReloadSpeed;

    [Header("Projectile Settings")]
    public GameObject[] Projectiles;
    public int ProjectileSpeed;
    public int ProjectilePower;
}
