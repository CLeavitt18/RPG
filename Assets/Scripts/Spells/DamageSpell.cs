using System.Collections.Generic;
using UnityEngine;

public class DamageSpell : Spell
{
    [SerializeField] private int CritDamage;

    public List<DamageType> DamageRanges;

    [SerializeField] private List<int> StatusChance;


    public override void SetStats(SpellStats stats)
    {
        if (GetSpellAffect() != null)
        {
            return;
        }

        DamageSpellStats statsD = stats as DamageSpellStats;

        DamageRanges = new List<DamageType>();
        StatusChance = new List<int>();

        for (int i = 0; i < statsD.ranges.Count; i++)
        {
            DamageRanges.Add(statsD.ranges[i]);
            StatusChance.Add(statsD.StatusChances[i]);
        }

        CritDamage = statsD.CritDamage;

        base.SetStats(stats);
    }

    public override bool Equals(Spell spell)
    {
        if (spell is DamageSpell dSpell)
        {
            for (int i = 0; i < DamageRanges.Count; i++)
            {
                if (DamageRanges[i].Type != dSpell.DamageRanges[i].Type ||
                    DamageRanges[i].LDamage != dSpell.DamageRanges[i].LDamage ||
                    DamageRanges[i].HDamage != dSpell.DamageRanges[i].HDamage)
                {
                    return false;
                }
            }

            return base.Equals(spell);
        }

        return false;
    }

    public int GetCritDamage()
    {
        return CritDamage;
    }

    public int GetLowerRange(int id)
    {
        return DamageRanges[id].LDamage;
    }

    public int GetUpperRange(int id)
    {
        return DamageRanges[id].HDamage;
    }

    public int GetStatusChance(int id)
    {
        return StatusChance[id];
    }

    public int GetDamageTypeCount()
    {
       return DamageRanges.Count;
    }

    public DamageTypeEnum GetDamageType(int id)
    {
        return DamageRanges[id].Type;
    }
}
