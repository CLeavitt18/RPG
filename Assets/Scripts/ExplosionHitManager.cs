using UnityEngine;

public class ExplosionHitManager : HitManager
{
    private void Awake() 
    {
        //Destroy(gameObject, 0.15f);
    }

    public void SetDamage()
    {
        for (int i = 0; i < Stats.DamageValues.Count; i++)
        {
            Stats.DamageValues[i] = (int)((float)Stats.DamageValues[i] * 0.5f);
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.GetComponent<LivingEntities>() != null)
        {
            HitSomething(other.GetComponent<LivingEntities>(), false);
        }    
    }
}
