using System.Collections.Generic;
using UnityEngine;

public class SpellHolder : Item
{
    [SerializeField] private HandType HandType;
    
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
        //Create some way of checking if the stats have not been set yet
        //Make a gamobject for the _item that can be spawn when the spell is equiped
        //Check if its null or not null return out becasue the spells stats has already beeen set before

        numOfSpells = 0;

        for (int i = 0; i < 3; i++)
        {
            if (stats.Spells[i] == null)
            {
                continue;
            }

            numOfSpells++;

            switch ((SpellType)stats.Spells[i].SpellTypeId)
            {
                case SpellType.DamageSpell:
                    Spells[i] = gameObject.AddComponent<DamageSpell>();
                    break;
                case SpellType.GolemSpell:
                    Spells[i] = gameObject.AddComponent<GolemSpell>();
                    break;
                default:
                    break;
            }

            LoadSystem.LoadRune(stats.Spells[i], Spells[i]);
        }

        base.SetStats(stats);
    }

    public override bool Equals(Item Item)
    {
        if (Item is SpellHolder spell) 
        {
            if (GetName() == spell.GetName() && HandType == spell.HandType)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (Spells[i] == null && spell.Spells[i] == null)
                    {
                        continue;
                    }

                    if (Spells[i] == null || spell.Spells[i] == null || Spells[i].Equals(spell.Spells[i]) == false)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        return false;
    }

    public HandType GetHandType()
    {
        return HandType;
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