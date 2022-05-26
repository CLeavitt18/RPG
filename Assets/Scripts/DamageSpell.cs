using System.Collections.Generic;

public class DamageSpell : Spell
{
    public int CritDamage;

    public List<DamageTypeStruct> DamageRanges;

    public List<int> StatusChance;

    public override void SetStats(SpellStats stats)
    {
        if (GetSpellAffect() != null)
        {
            return;
        }

        DamageSpellStats statsD = stats as DamageSpellStats;

        DamageRanges = new List<DamageTypeStruct>();
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
}
