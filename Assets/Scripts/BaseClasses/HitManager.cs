using UnityEngine;

public class HitManager : MonoBehaviour
{
    public DamageStats Stats = new DamageStats();

    public int HitSomething(LivingEntities other)
    {
        return other.TakeDamage(Stats);
    }
}
