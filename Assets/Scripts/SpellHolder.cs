﻿using System.Collections.Generic;
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

            numOfSpells++;

            switch ((SpellType)stats.Spells[i].SpellAffectID)
            {
                case SpellType.DamageSpell:
                    Spells[i] = gameObject.AddComponent<DamageSpell>();

                    DamageSpell spellRef = Spells[i] as DamageSpell;

                    Spells[i].SpellType = SpellType.DamageSpell;

                    LoadSystem.LoadRune(stats.Spells[i], spellRef);

                    break;
                case SpellType.GolemSpell:
                    Spells[i] = gameObject.AddComponent<GolemSpell>();

                    Spells[i].SpellType = SpellType.GolemSpell;

                    GolemSpell spellref = Spells[i] as GolemSpell;

                    LoadSystem.LoadRune(stats.Spells[i], spellref);

                    break;
                default:
                    break;
            }
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