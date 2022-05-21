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

    public void SetSpellState()
    {
        name = GetName();

        numOfSpells = 0;

        for (int i = 0; i < 3; i++)
        {
            if (Spells[i] != null)
            {
                numOfSpells++;
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