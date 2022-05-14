using System.Collections;
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

        Spell[] runes = new Spell[numOfRunes];

        for (int i = 0; i < numOfRunes; i++)
        {
            runes[i] = Roller.roller.runeRoller.RollRune().GetComponent<RuneHolder>().spell;
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

        //spellH.Rarity = (runes[0].gameObject.GetComponent<RuneHolder>()).Rarity;
        spellH.Type = (MaterialType)mat_id;

        spellH.Name = spellH.Type.ToString() + " Spell Focus";

        DamageTypeStruct damageType;

        int numRune = 0;
        int rarityIdsTotal = 0;

        for (int i = 0; i < runes.Length; i++)
        {
            if (runes[i] == null)
            {
                break;
            }
            
            numRune++;

            for(int x = 0; x > GlobalValues.rarities.Length; x++)
            {
                if(runes[i].GetComponent<RuneHolder>().Rarity == GlobalValues.rarities[i])
                {
                    rarityIdsTotal += i;
                }
            }

            switch (runes[i].SpellType)
            {
                case SpellType.DamageSpell:
                    DamageSpell dSpell = runes[i] as DamageSpell;

                    spellH.Spells[i] = spellH.gameObject.AddComponent<DamageSpell>();

                    DamageSpell spellRef = spellH.Spells[i] as DamageSpell;

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

                    GolemSpell spellref = spellH.Spells[i] as GolemSpell;

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

            spellH.Spells[i].Name = runes[i].Name;
        }

        spellH.ValueMulti = mats[mat_id].Multi;
        spellH.SetSpellState();

        if(numRune != 0)
        {
            spellH.Rarity = GlobalValues.rarities[(int)((float)rarityIdsTotal / numRune)];
        }
        else
        {
            spellH.Rarity = GlobalValues.rarities[0];
        }


        if (cleanUp)
        {
            for (int i = 0; i < runes.Length; i++)
            {
                Destroy(runes[i].gameObject);
            }
        }

        return spellH;
    }
}
