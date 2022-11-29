using System.Collections;
using System.Collections.Generic;
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
            HitSomething(other.GetComponent<LivingEntities>(), false);
        }    
    }
}
