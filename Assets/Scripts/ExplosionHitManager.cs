using UnityEngine;

public class ExplosionHitManager : HitManager
{
    private void Awake() 
    {
        Destroy(gameObject, 0.15f);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.GetComponent<LivingEntities>() != null)
        {
            float distance = Vector3.Distance(transform.position, other.transform.position) + 0.2f;

            float range = GetComponent<Light>().range;

            float damageMulti = -0.5f * ((distance / range) - 1.0f);

            for (int i = 0; i < Stats.DamageValues.Count; i++)
            {
                Stats.DamageValues[i] = (int)((float)Stats.DamageValues[i] * damageMulti);
            }

            HitSomething(other.GetComponent<LivingEntities>(), false);
        }    
    }
}
