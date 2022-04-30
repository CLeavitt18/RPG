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

        int id;

        if ((id = Stats.DamageTypes.IndexOf(DamageTypeEnum.Fire)) != -1)
        {
            if (Stats.Status[id] == true)
            {
                //Debug.Log("Burning status Started");
                Stats.Parent.StatusManger.StartBurning(OL, Stats.DamageValues[id]);
            }
        }

        if ((id = Stats.DamageTypes.IndexOf(DamageTypeEnum.Lightning)) != -1)
        {
            if (Stats.Status[id] == true)
            {
                Stats.Parent.StatusManger.StartChain(OL, Stats.DamageValues[id], 0);
            }
        }

        if ((id = Stats.DamageTypes.IndexOf(DamageTypeEnum.Ice)) != -1 )
        {
            if (Stats.Status[id] == true)
            {
                Stats.Parent.StatusManger.StartChill(OL);
            }
        }
    }
}
