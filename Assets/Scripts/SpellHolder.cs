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
        name = GetName();

        numOfSpells = 0;

        /*switch (runes[i].SpellType)
            {
                case SpellType.DamageSpell:
                    DamageSpell dSpell = runes[i] as DamageSpell;

                    dSpell.Spells[i] = spellH.gameObject.AddComponent<DamageSpell>();

                    DamageSpell spellRef = spellH.GetRune(i) as DamageSpell;

                    spellRef.DamageRanges = new List<DamageTypeStruct>();
                    spellRef.StatusChance = new List<int>();

                    for (int x  = 0; x < dSpell.DamageRanges.Count; x++)
                    {
                        damageType = new DamageTypeStruct(dSpell.DamageRanges[x], mats[mat_id].Multi);

                        spellRef.DamageRanges.Add(damageType);
                        spellRef.StatusChance.Add(dSpell.StatusChance[x]);
                    }

                    break;
                case SpellType.GolemSpell:
                    GolemSpell gSpell = runes[i] as GolemSpell;

                    spellH.Spells[i] = spellH.gameObject.AddComponent<GolemSpell>();

                    GolemSpell spellref = spellH.GetRune(i) as GolemSpell;

                    damageType = new DamageTypeStruct(gSpell.DamageRange, mats[mat_id].Multi);

                    spellref.DamageRange = damageType;
                    spellref.Number = gSpell.Number;

                    break;
                default:
                    break;
            }

            spellH.Spells[i].SpellType = runes[i].SpellType;
            spellH.Spells[i].CastType = runes[i].CastType;
            spellH.Spells[i].Target = runes[i].Target;
            spellH.Spells[i].CostType = runes[i].CostType;
            spellH.Spells[i].SpellAffect = runes[i].SpellAffect;
            spellH.Spells[i].Cost = runes[i].Cost * mats[mat_id].Multi;
            spellH.Spells[i].CastsPerSecond = runes[i].CastsPerSecond;
            spellH.Spells[i].SkillType = runes[i].SkillType;

            spellH.Spells[i].Name = runes[i].Name;*/

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