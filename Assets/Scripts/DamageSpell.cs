using System.Collections.Generic;

public class DamageSpell : Spell
{
    public int CritDamage;

    public List<DamageTypeStruct> DamageRanges;

    public List<int> StatusChance;

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
