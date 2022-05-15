using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneRoller : MonoBehaviour
{
    [SerializeField] private BaseSpell[] baseSpells;

    [SerializeField] private Catalyst[] cats;

    [SerializeField] private GameObject runeHolder;

    [SerializeField] private DropTable[] CostTypeTable;
    [SerializeField] private DropTable[] CatTable;

    public Item RollRune()
    {
        int chance = Random.Range((int)SpellType.DamageSpell, (int)SpellType.None);

        SpellType spellType;
        AttributesEnum costType;
        CastType castType = CastType.Channelled;
        CastTarget castTarget = CastTarget.Other;
        int damageType;
        int cat_id;
        int level;

        spellType = (SpellType)chance;

        switch (spellType)
        {
            case SpellType.DamageSpell:
                break;
            case SpellType.GolemSpell:
                castType = CastType.Aura;
                castTarget = CastTarget.Self;
                break;
            default:
                break;
        }

        chance = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

        if (chance < CostTypeTable[0].Chances[0])
        {
            costType = AttributesEnum.Mana;
        }
        else if (chance < CostTypeTable[0].Chances[1])
        {
            costType = AttributesEnum.Health;
        }
        else
        {
            costType = AttributesEnum.Stamina;
        }

        if (spellType == SpellType.DamageSpell)
        {
            chance = Random.Range(0, 2);

            castType = (CastType)chance;
            castTarget = CastTarget.Other;
        }
        else if (spellType == SpellType.GolemSpell)
        {
            castType = CastType.Aura;
            castTarget = CastTarget.Self;
        }

        chance = Random.Range(0, 4);

        damageType = chance;

        cat_id = damageType * 6;

        chance = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

        for (int i = 0; i < CatTable[0].Chances.Length; i++)
        {
            if (chance < CatTable[0].Chances[i])
            {
                cat_id += i;
                break;
            }
        }

        level = Random.Range(1, Player.player.GetLevel() * 4 + 1);

        if (spellType == SpellType.GolemSpell)
        {
            damageType = 0;
            castType = CastType.Channelled;
            cat_id = 0;
        }

        Item rune = CreateRune(spellType, costType, castType, castTarget, damageType, cat_id, level);

        return rune;
    }

    public Item CreateRune
        (
        SpellType spellType,
        AttributesEnum costType,
        CastType castType,
        CastTarget castTareget,
        int damageType,
        int cat_id,
        int level
        )
    {

        Catalyst cat = cats[cat_id];

        int id = (int)spellType;
        int castId = (int)castType;
        int rarityId = cat_id % 6;

        Item item = Instantiate(runeHolder).GetComponent<Item>();

        Spell rune = null;

        DamageTypeStruct damage;

        switch (spellType)
        {
            case SpellType.DamageSpell:
                DamageSpell dRune = item.gameObject.AddComponent<DamageSpell>();

                rune = dRune;

                dRune.DamageRanges = new List<DamageTypeStruct>();
                dRune.StatusChance = new List<int>();

                damage = new DamageTypeStruct(baseSpells[id].Ranges[castId][damageType], cat.CatMultis[damageType]);

                dRune.DamageRanges.Add(damage);

                dRune.StatusChance.Add(60);

                break;
            case SpellType.GolemSpell:

                GolemSpell gRune = item.gameObject.AddComponent<GolemSpell>();

                rune = gRune;

                damage = new DamageTypeStruct(baseSpells[id].Ranges[castId][damageType], cat.CatMultis[damageType]);

                gRune.DamageRange = damage;

                castType = CastType.Aura;

                gRune.Number = 1;
                break;
            default:
                break;
        }


        switch (damageType)
        {
            case 0:
                rune.SkillType = SkillType.Geomancy;
                break;
            case 1:
                rune.SkillType = SkillType.Pyromancy;
                break;
            case 2:
                rune.SkillType = SkillType.Astromancy;
                break;
            default:
                rune.SkillType = SkillType.Cryomancy;
                break;
        }

        rune.SpellType = spellType;
        rune.CastType = castType;
        rune.Target = castTareget;
        rune.CostType = costType;
        rune.SpellAffect = baseSpells[id].SpellAffects[castId][damageType];
        rune.Cost = baseSpells[id].ManaCost[castId][damageType];
        rune.CastsPerSecond = baseSpells[id].CastsPerSecond[castId][damageType];

        RuneHolder runeH = item as RuneHolder;

        runeH.spell = rune;

        string name = rune.SpellAffect.name;

        for (int i = 0; i < name.Length; i++)
        {
            if (name[i] == '(')
            {
                break;
            }

            runeH.Name += name[i];
        }

        runeH.spell.Name = runeH.Name;
        item.Rarity = GlobalValues.rarities[rarityId];

        runeH.Name += " Rune";
        runeH.name = runeH.Name;

        return item;
    }
}
