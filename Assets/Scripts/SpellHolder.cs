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
        /*SpellHolder spell;

        try
        {
            spell = (SpellHolder)Item;
        }
        catch
        {
            return false;
        }

        if (name == spell.name)
        {
            return true;
        }*/

        return false;
    }

    public int GetNumOfSpells()
    {
        return numOfSpells;
    }
}