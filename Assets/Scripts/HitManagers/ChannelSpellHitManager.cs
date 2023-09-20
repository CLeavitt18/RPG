using UnityEngine;

public class ChannelSpellHitManager : HitManager
{
    public float LifeTime;

    public void SetLifeTime(Spell Spell)
    {
        float CastRate = 0;

        if (Spell is DamageSpell dSpell)
        {
            CastRate = dSpell.GetCastRate();
        }

        LifeTime = Time.time + (1f / CastRate);
    }

    private void Update()
    {
        if (Time.time >= LifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GlobalValues.PlayerTag) || 
        other.CompareTag(GlobalValues.EnemyTag) || 
        other.CompareTag(GlobalValues.NPCTag) ||
        other.CompareTag(GlobalValues.MinionTag))
        {
            HitSomething(other.GetComponent<LivingEntities>(), false);
        }
    }
}
