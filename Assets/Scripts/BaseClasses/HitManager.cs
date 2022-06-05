using UnityEngine;

public class HitManager : MonoBehaviour
{
    public DamageStats Stats = new DamageStats();

    public int HitSomething(LivingEntities other, bool hitShield)
    {
        return other.TakeDamage(Stats, hitShield);
    }
}
