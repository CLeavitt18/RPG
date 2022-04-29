using UnityEngine;

public class WeaponHitManager : HitManager
{
    public void OnTriggerEnter(Collider other)
    {
        LivingEntities OL = other.GetComponent<LivingEntities>();

        if (Stats.Parent == null)
        {
            this.enabled = false;
            return;
        }

        if (OL == null || 
            (!OL.CompareTag(GlobalValues.PlayerTag) && !OL.CompareTag(GlobalValues.EnemyTag) && 
            !OL.CompareTag(GlobalValues.MinionTag) && !OL.CompareTag(GlobalValues.NPCTag)) || 
            OL.CompareTag(Stats.Parent.tag) 
             || OL.GetDead())
        {
            return;
        }

        WeaponHolder weapon = Stats.Parent.GetHeldItem(Stats.SourceHand).GetComponent<WeaponHolder>();

        if (weapon.CurrentDurability > 0)
        {
            weapon.CurrentDurability--;
        }
        else if(weapon.MaxDurability != 0)
        {
            return;
        }

        int DamageDelt = HitSomething(OL);

        if (DamageDelt > 0 && Stats.LifeSteal > 0)
        {
            Stats.Parent.GainAttribute((int)(DamageDelt * Stats.LifeSteal * .01f), (int)AttributesEnum.Health);
        }

        DamageStats statsCopy = new DamageStats(Stats);

        if (Stats.DamageTypes.Contains(DamageTypeEnum.Fire))
        {
            int id = Stats.DamageTypes.IndexOf(DamageTypeEnum.Fire);

            if (Stats.Status[id] == true)
            {
                //Starts burning on self if relfect Burning is active
                if (Stats.Parent.GetPower(1, 0))
                {
                    //Starts burning on self if
                    //not already burning
                    //or 
                    //burning can stack
                    if (!Stats.Parent.GetBurning() || Stats.Parent.GetPower(0, 3))
                    {
                        StartCoroutine(Stats.Parent.BurningStatus(statsCopy, id));
                    }
                }

                //Start burning on enemy if
                //the enemy is not already buring
                //or 
                //buring can stack
                if (!OL.GetBurning() || Stats.Parent.GetPower(0, 3))
                {
                    StartCoroutine(OL.BurningStatus(statsCopy, id));
                    //Debug.Log("Burning Started on enemy");
                }
            }
        }

        if (Stats.DamageTypes.Contains(DamageTypeEnum.Lightning))
        {
            int id = Stats.DamageTypes.IndexOf(DamageTypeEnum.Lightning);

            if (Stats.Status[id] == true)
            {
                StartCoroutine(OL.ChainLightning(statsCopy, id));
            }
        }

        if (Stats.DamageTypes.Contains(DamageTypeEnum.Ice))
        {
            int id = Stats.DamageTypes.IndexOf(DamageTypeEnum.Ice);

            if (Stats.Status[id] == true && !OL.GetChilled())
            {
                StartCoroutine(OL.ChillStatus(statsCopy));
            }
        }
    }
}
