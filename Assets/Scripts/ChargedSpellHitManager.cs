using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedSpellHitManager : ProjectileSpellHitManger
{
    [SerializeField] private GameObject Explosion;

    [SerializeField] private float Range;


    protected override void BeforeDestroy()
    {
        GameObject explosion = Instantiate(Explosion, transform.position, transform.rotation);

        explosion.GetComponent<ExplosionHitManager>().Stats = Stats;
        explosion.GetComponent<Light>().range = Range;
        explosion.GetComponent<SphereCollider>().radius = Range; 
    }
}
