using UnityEngine;

public class SpellHolder : Item
{
    public HandType HandType;

    public SkillType SkillType;

    public Spell[] Spells = new Spell[3];

    public MaterialType Type;

    public int ValueMulti;

    [SerializeField] private int numOfSpells;


    public override void SpawnItem()
    {
        base.SpawnItem();
    }

    public override void StoreItem()
    {
        base.StoreItem();
    }

    public void SetSpellState()
    {
        name = GetName();

        numOfSpells = 0;

        for (int i = 0; i < Spells.Length; i++)
        {
            if (Spells[i] != null)
            {
                numOfSpells++;
            }
            else
            {
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

    public int GetNumOfSpells()
    {
        return numOfSpells;
    }
}