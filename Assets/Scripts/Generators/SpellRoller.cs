using System.Collections.Generic;
using UnityEngine;

public class SpellRoller : MonoBehaviour
{
    [SerializeField] private MatMults[] mats;

    [SerializeField] private DropTable[] matDropTable;

    public Item RollSpell()
    {
        int mat_id = 0;

        int numOfRunes = Random.Range(1, 4);

        Spell[] runes = new Spell[3];

        for (int i = 0; i < numOfRunes; i++)
        {
            runes[i] = GetComponent<RuneRoller>().RollRune().GetComponent<RuneHolder>().spell;
        }

        int chance = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

        for (int i = 0; i < matDropTable[0].Chances.Length; i++)
        {
            if (chance <= matDropTable[0].Chances[i])
            {
                mat_id = i;
                break;
            }
        }

        Item Item = CreateSpell(mat_id, runes);

        return Item;
    }

    public Item CreateSpell(int mat_id, Spell[] runes, bool cleanUp = true)
    {
        SpellHolder spellH = Instantiate(PrefabIDs.prefabIDs.SpellHolder).GetComponent<SpellHolder>();

        SpellHolderStats stats = new SpellHolderStats();

        //spellH.Rarity = (runes[0].gameObject.GetComponent<RuneHolder>()).Rarity;
        stats.Type = (MaterialType)mat_id;

        stats.Name = ((MaterialType)mat_id).ToString() + " Spell Focus";
        stats.Spells = runes;

        int numRune = 0;
        int rarityIdsTotal = 0;

        for (int i = 0; i < 3; i++)
        {
            if (runes[i] == null)
            {
                continue;
            }
            
            numRune++;

            switch (runes[i].SpellType)
            {
                case SpellType.DamageSpell:
                    DamageSpell dSpell = runes[i] as DamageSpell;

                    DamageTypeStruct damageType;

                    for (int x = 0; x < dSpell.DamageRanges.Count; x++)
                    {
                        damageType = new DamageTypeStruct(dSpell.DamageRanges[x], mats[mat_id].Multi);

                        dSpell.DamageRanges[x] = damageType;
                    }
                    break;
                case SpellType.GolemSpell:
                    GolemSpell gSpell = runes[i] as GolemSpell;

                    gSpell.DamageRange = new DamageTypeStruct(gSpell.DamageRange, mats[mat_id].Multi);

                    break;
                default:
                    break;
            }

            runes[i].Cost = runes[i].Cost * mats[mat_id].Multi;

            for(int x = 0; x > GlobalValues.rarities.Length; x++)
            {
                if(runes[i].GetComponent<RuneHolder>().GetRarity() == GlobalValues.rarities[i])
                {
                    rarityIdsTotal += i;
                }
            }
        }

        stats.MaterialMulti = mats[mat_id].Multi;

        if(numRune != 0)
        {
            stats.Rarity = GlobalValues.rarities[(int)((float)rarityIdsTotal / numRune)];
        }
        else
        {
            stats.Rarity = GlobalValues.rarities[0];
        }

        spellH.SetSpellState(stats);
    
        if (cleanUp)
        {
            for (int i = 0; i < 3; i++)
            {
                if (runes[i] == null)
                {
                    continue;
                }

                Destroy(runes[i].gameObject);
            }
        }

        return spellH;
    }
}
