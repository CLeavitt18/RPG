using System.Collections.Generic;
using UnityEngine;

public class SpellHolder : Item
{
    [SerializeField] private HandType HandType;

    [SerializeField] private SkillType SkillType;
    
    [SerializeField] private MaterialType Type;

    [SerializeField] private Spell[] Spells = new Spell[3];

    [SerializeField] private int ValueMulti;

    [SerializeField] private int numOfSpells;


    public override void SpawnItem()
    {
        base.SpawnItem();
    }

    public override void StoreItem()
    {
        base.StoreItem();
    }

    public void SetSpellState(SpellHolderStats stats)
    {
        SetName(stats.Name);
        name = stats.Name;
        name = stats.Name;

        numOfSpells = 0;

        for (int i = 0; i < 3; i++)
        {
            if (stats.Spells[i] == null)
            {
                continue;
            }

            Spell spell = stats.Spells[i];

            numOfSpells++;

            switch (spell.SpellType)
            {
                case SpellType.DamageSpell:
                    DamageSpell dSpell = spell as DamageSpell;

                    Spells[i] = gameObject.AddComponent<DamageSpell>();

                    DamageSpell spellRef = Spells[i] as DamageSpell;

                    spellRef.DamageRanges = new List<DamageTypeStruct>();
                    spellRef.StatusChance = new List<int>();

                    for (int x  = 0; x < dSpell.DamageRanges.Count; x++)
                    {
                        spellRef.DamageRanges.Add(dSpell.DamageRanges[x]);
                        spellRef.StatusChance.Add(dSpell.StatusChance[x]);
                    }

                    break;
                case SpellType.GolemSpell:
                    GolemSpell gSpell = spell as GolemSpell;

                    Spells[i] = gameObject.AddComponent<GolemSpell>();

                    GolemSpell spellref = Spells[i] as GolemSpell;

                    spellref.DamageRange = gSpell.DamageRange;
                    spellref.Number = gSpell.Number;

                    break;
                default:
                    break;
            }

            Spells[i].SpellType = spell.SpellType;
            Spells[i].CastType = spell.CastType;
            Spells[i].Target = spell.Target;
            Spells[i].CostType = spell.CostType;
            Spells[i].SpellAffect = spell.SpellAffect;
            Spells[i].Cost = spell.Cost;
            Spells[i].CastsPerSecond = spell.CastsPerSecond;
            Spells[i].SkillType = spell.SkillType;

            Spells[i].Name = spell.Name;
        }

    }

    public override bool Equals(Item Item)
    {
        SpellHolder spell;

        try
        {
            spell = Item as SpellHolder;
        }
        catch
        {
            return false;
        }

        if (GetName() == spell.GetName() && HandType == spell.HandType)
        {
            for (int i = 0; i < 3; i++)
            {
                if (Spells[i] == null && spell.Spells[i] == null)
                {
                    continue;
                }

                if ((Spells[i] != null && spell.Spells[i] == null) || 
                (Spells[i] == null && spell.Spells[i] != null) || 
                Spells[i].Equals(spell.Spells[i]) == false)
                {
                    return false;
                }
            }

            return true;
        }

        return false;
    }

    public HandType GetHandType()
    {
        return HandType;
    }

    public SkillType GetSkill()
    {
        return SkillType;
    }

    public MaterialType GetMaterialType()
    {
        return Type;
    }

    public Spell GetRune(int id)
    {
        return Spells[id];
    }

    public int GetValueMulti()
    {
        return ValueMulti;
    }


    public int GetNumOfSpells()
    {
        return numOfSpells;
    }
}