using UnityEngine;

public class ChargedSpellHitManager : ProjectileSpellHitManger
{
    [SerializeField] private GameObject Explosion;

    [SerializeField] private float Range;


    protected override void BeforeDestroy()
    {
        GameObject explosion = Instantiate(Explosion, transform.position, transform.rotation);

        explosion.GetComponent<ExplosionHitManager>().Stats = new DamageStats(Stats);
        explosion.GetComponent<Light>().range = Range;
        explosion.GetComponent<SphereCollider>().radius = Range; 

        explosion.GetComponent<ExplosionHitManager>().SetDamage();
    }
}
