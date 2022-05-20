using System.Collections.Generic;
using UnityEngine;

public static class LoadSystem
{
    public static void LoadItem(WeaponStats FromWeapon, WeaponHolder ToWeapon)
    {
        ToWeapon.CritDamage = FromWeapon.CritDamage;
        ToWeapon.Type = FromWeapon.Type;
        ToWeapon.SetSkill((SkillType)FromWeapon.WeaponSkillType);
        ToWeapon.HandType = (HandType)FromWeapon.HandType;

        for (int i = 0; i < FromWeapon.DamageRanges.Length; i++)
        {
            ToWeapon.DamageRanges.Add(FromWeapon.DamageRanges[i]);
            ToWeapon.StatusChance.Add(FromWeapon.StatusChance[i]);
        }

        ToWeapon.ActionsPerSecond = FromWeapon.AttacksPerSecond;
        ToWeapon.SetWeight(FromWeapon.Weight);
        ToWeapon.SetValue(FromWeapon.Value);
        ToWeapon.SetName(FromWeapon.Name);
        ToWeapon.LifeSteal = FromWeapon.LifeSteal;
        ToWeapon.CurrentDurability = FromWeapon.CurrentDurability;
        ToWeapon.MaxDurability = FromWeapon.MaxDurability;
        ToWeapon.AttackAnimationName = FromWeapon.AttackAnimationName;
        ToWeapon.PwrAttackAnimationName = FromWeapon.PwrAttackAnimationName;
        ToWeapon.SetAmount(FromWeapon.Amount);

        Color color = new Color(
            FromWeapon.Rarity[0], 
            FromWeapon.Rarity[1],
            FromWeapon.Rarity[2],
            FromWeapon.Rarity[3]);

        ToWeapon.SetRarity(color);

        for (int i = 0; i < 3; i++)
        {
            ToWeapon.Materials[i] = FromWeapon.Materials[i];
        }

        ToWeapon.Material = PrefabIDs.prefabIDs.WeaponMaterials[FromWeapon.Materail];
        ToWeapon.Primary = PrefabIDs.prefabIDs.WeaponParts[FromWeapon.Primary];
        ToWeapon.Secoundary = PrefabIDs.prefabIDs.WeaponParts[FromWeapon.Secoundary];
        ToWeapon.Teritiary = PrefabIDs.prefabIDs.WeaponParts[FromWeapon.Teritiary];
        ToWeapon.Animator[0] = PrefabIDs.prefabIDs.Animators[FromWeapon.AnimatorId[0]];
        ToWeapon.Animator[1] = PrefabIDs.prefabIDs.Animators[FromWeapon.AnimatorId[1]];

        ToWeapon.SetWeaponState();
    }

    public static void LoadItem(ArmourStats FromArmour, ArmourHolder ToArmour)
    {
        ToArmour.Armour = FromArmour.Armour;
        ToArmour.CurrentDurability = FromArmour.CurrentDurablity;
        ToArmour.MaxDurability = FromArmour.MaxDurablity;
        ToArmour.ArmourType = (ArmourType)FromArmour.ArmourType;
        ToArmour.SetValue(FromArmour.Value);
        ToArmour.SetAmount(FromArmour.Amount);
        ToArmour.SetWeight(FromArmour.Weight);
        ToArmour.SkillType = (SkillType)FromArmour.SkillType;

        ToArmour.Resistences = new int[3];

        for (int i = 0; i < 3; i++)
        {
            ToArmour.Resistences[i] = FromArmour.Resistences[i];
        }

        for (int i = 0; i < FromArmour.Enchantments.Length; i++)
        {
            ToArmour.Enchantments.Add(FromArmour.Enchantments[i]);
        }

        ToArmour.SetEquiped(FromArmour.IsEquiped);

        ToArmour.SetName(FromArmour.Name);

        ToArmour.SetItem(null);

        ToArmour.SetArmourState();
    }

    public static void LoadItem(SpellHolderData FromSpell, SpellHolder ToSpell)
    {
        ToSpell.Spells = new Spell[FromSpell.SpellsData.Length];

        for (int i = 0; i < FromSpell.SpellsData.Length; i++)
        {
            SpellData spellData = FromSpell.SpellsData[i];

            if (spellData.SpellTypeId == (int)SpellType.None)
            {
                continue;
            }

            if (spellData.SpellTypeId == (int)SpellType.DamageSpell)
            {
                ToSpell.Spells[i] = ToSpell.gameObject.AddComponent<DamageSpell>();
            }
            else if (spellData.SpellTypeId == (int)SpellType.GolemSpell)
            {
                ToSpell.Spells[i] = ToSpell.gameObject.AddComponent<GolemSpell>();
            }

            LoadRune(spellData, ToSpell.Spells[i]);
        }

        ToSpell.ValueMulti = FromSpell.MaterialMulti;
        ToSpell.Type = (MaterialType)FromSpell.MaterialId;
        ToSpell.SkillType = (SkillType)FromSpell.SpellSkillType;
        ToSpell.SetAmount(FromSpell.Amount);
        ToSpell.SetName(FromSpell.Name);

        Color color = new Color(
            FromSpell.Rarity[0],
            FromSpell.Rarity[1],
            FromSpell.Rarity[2],
            FromSpell.Rarity[3]);

        ToSpell.SetRarity(color);

        ToSpell.SetSpellState();
    }

    public static void LoadItem(RuneHolderData FromRuneH, RuneHolder ToRuneH)
    {
        ToRuneH.SetName(FromRuneH.Name);
        ToRuneH.SetAmount(FromRuneH.Amount);

        Color color = new Color(
            FromRuneH.Rarity[0],
            FromRuneH.Rarity[1],
            FromRuneH.Rarity[2],
            FromRuneH.Rarity[3]);

        ToRuneH.SetRarity(color);

        if (FromRuneH.runeData.SpellTypeId == 0)
        {
            ToRuneH.spell = ToRuneH.gameObject.AddComponent<DamageSpell>();
        }
        else if (FromRuneH.runeData.SpellTypeId == 1)
        {
            ToRuneH.spell = ToRuneH.gameObject.AddComponent<GolemSpell>();
        }

        LoadRune(FromRuneH.runeData, ToRuneH.spell);
    }

    private static void LoadRune(SpellData FromRune, Spell ToRune)
    {
        if (ToRune is DamageSpell spellD)
        {
            spellD.DamageRanges = new List<DamageTypeStruct>();

            for (int x = 0; x < FromRune.StatArray2.Length; x++)
            {
                spellD.DamageRanges.Add(new DamageTypeStruct());
                {
                    spellD.DamageRanges[x].Type = (DamageTypeEnum)FromRune.StatArray2[x];
                    spellD.DamageRanges[x].LDamage = FromRune.StatArray3[x];
                    spellD.DamageRanges[x].HDamage = FromRune.StatArray4[x];
                }
            }

            spellD.StatusChance = new List<int>();

            for (int x = 0; x < FromRune.StatArray1.Length; x++)
            {
                spellD.StatusChance.Add(FromRune.StatArray1[x]);
            }

            spellD.CritDamage = FromRune.int0;

            spellD.CastsPerSecond = FromRune.CastRate;

            spellD.SpellAffect = PrefabIDs.prefabIDs.SpellAffects[FromRune.SpellAffectID];
        }
        else if (ToRune is GolemSpell spellG)
        {
            spellG.SpellType = (SpellType)FromRune.SpellTypeId;

            spellG.Activated = FromRune.bool0;

            spellG.Number = FromRune.int0;

            spellG.DamageRange = new DamageTypeStruct()
            {
                Type = (DamageTypeEnum)FromRune.StatArray0[0],
                LDamage = FromRune.StatArray0[1],
                HDamage = FromRune.StatArray0[2],
            };

            spellG.SpellAffect = PrefabIDs.prefabIDs.Minions[FromRune.SpellAffectID];
        }

        ToRune.Name = FromRune.Name;
        ToRune.Cost = FromRune.ManaCost;
        ToRune.CostType = (AttributesEnum)FromRune.CostType;
        ToRune.CastType = (CastType)FromRune.CastType;
        ToRune.Target = (CastTarget)FromRune.Target;
    }

    public static QuestHolder LoadItem(QuestData FromQuest, QuestHolder ToQuest)
    {
        ToQuest.HourAquired = FromQuest.HourAquired;
        ToQuest.MintueAquired = FromQuest.MintueAquired;
        ToQuest.DateAquired[0] = FromQuest.DateAquiredDay;
        ToQuest.DateAquired[1] = FromQuest.DateAquiredMonth;
        ToQuest.DateAquired[2] = FromQuest.DateAquiredMonth;
        ToQuest.CurrentQuestStep = FromQuest.CurrentQuestStep;

        return ToQuest;
    }

    public static void LoadItem(WeaponHolder FromWeapon, WeaponStats ToWeapon)
    {
        ToWeapon.CritDamage = FromWeapon.GetCrit();
        ToWeapon.Type = FromWeapon.GetWeaponType();
        ToWeapon.WeaponSkillType = (int)FromWeapon.GetSkill();
        ToWeapon.HandType = (int)FromWeapon.GetHandType();

        ToWeapon.DamageRanges = new DamageTypeStruct[FromWeapon.GetDamageRangesCount()];
        ToWeapon.StatusChance = new int[FromWeapon.GetStatusCount()];

        for (int i = 0; i < ToWeapon.DamageRanges.Length; i++)
        {
            ToWeapon.DamageRanges[i] = new DamageTypeStruct()
            { 
               LDamage = FromWeapon.GetLowerRange(i),
               HDamage = FromWeapon.GetUpperRange(i),
               Type = FromWeapon.GetDamageType(i)
            };
                
            ToWeapon.StatusChance[i] = FromWeapon.GetStatus(i);
        }

        ToWeapon.AttacksPerSecond = FromWeapon.GetAttackSpeed();
        ToWeapon.Weight = FromWeapon.GetWeight();
        ToWeapon.Value = FromWeapon.GetValue();
        ToWeapon.Name = FromWeapon.GetName();
        ToWeapon.LifeSteal = FromWeapon.GetLifeSteal();
        ToWeapon.CurrentDurability = FromWeapon.GetDurability();
        ToWeapon.MaxDurability = FromWeapon.GetMaxDurability();
        ToWeapon.AttackAnimationName = FromWeapon.GetAttackAnimationName();
        ToWeapon.PwrAttackAnimationName = FromWeapon.GetPwrAttackAnimationName();
        ToWeapon.Amount = FromWeapon.GetAmount();

        ToWeapon.Rarity = new float[4];

        ToWeapon.Rarity[0] = FromWeapon.GetRarity().r;
        ToWeapon.Rarity[1] = FromWeapon.GetRarity().g;
        ToWeapon.Rarity[2] = FromWeapon.GetRarity().b;
        ToWeapon.Rarity[3] = FromWeapon.GetRarity().a;

        ToWeapon.Materials = new MaterialType[3];

        for (int i = 0; i < 3; i++)
        {
            ToWeapon.Materials[i] = FromWeapon.GetMaterialType(i);
        }

        for (int y = 0; y < PrefabIDs.prefabIDs.WeaponMaterials.Length; y++)
        {
            if (PrefabIDs.prefabIDs.WeaponMaterials[y] == FromWeapon.GetMaterial())
            {
                ToWeapon.Materail = y;
                break;
            }
        }

        GameObject primary = FromWeapon.GetPrimary();
        GameObject secoundary = FromWeapon.GetSecoundary();
        GameObject teritiary = FromWeapon.GetTeritiary();

        for (int y = 0; y < PrefabIDs.prefabIDs.WeaponParts.Length; y++)
        {
            if (PrefabIDs.prefabIDs.WeaponParts[y] == primary)
            {
                ToWeapon.Primary = y;
                break;
            }
        }

        for (int y = 0; y < PrefabIDs.prefabIDs.WeaponParts.Length; y++)
        {
            if (PrefabIDs.prefabIDs.WeaponParts[y] == secoundary )
            {
                ToWeapon.Secoundary = y;
                break;
            }
        }

        for (int y = 0; y < PrefabIDs.prefabIDs.WeaponParts.Length; y++)
        {
            if (PrefabIDs.prefabIDs.WeaponParts[y] == teritiary)
            {
                ToWeapon.Teritiary = y;
                break;
            }
        }

        for (int i = 0; i < 2; i++)
        {
            RuntimeAnimatorController anime = FromWeapon.GetAnimationController(i);

            for (int x = 0; x < PrefabIDs.prefabIDs.Animators.Length; x++)
            {
                if (PrefabIDs.prefabIDs.Animators[x] == anime)
                {
                    ToWeapon.AnimatorId[i] = x;
                    break;
                }
            }
        }
    }

    public static void LoadItem(ArmourHolder FromArmour, ArmourStats ToArmour)
    {
        ToArmour.Armour = FromArmour.Armour;
        ToArmour.CurrentDurablity = FromArmour.CurrentDurability;
        ToArmour.MaxDurablity = FromArmour.MaxDurability;
        ToArmour.ArmourType = (int)FromArmour.ArmourType;
        ToArmour.Value = FromArmour.GetValue();
        ToArmour.Amount = FromArmour.GetAmount();
        ToArmour.Weight = FromArmour.GetWeight();
        ToArmour.SkillType = (int)FromArmour.SkillType;

        ToArmour.Rarity = new float[4];

        ToArmour.Rarity[0] = FromArmour.GetRarity().r;
        ToArmour.Rarity[1] = FromArmour.GetRarity().g;
        ToArmour.Rarity[2] = FromArmour.GetRarity().b;
        ToArmour.Rarity[3] = FromArmour.GetRarity().a;

        ToArmour.Resistences = new int[3];

        for (int i = 0; i < 3; i++)
        {
            ToArmour.Resistences[i] = FromArmour.Resistences[i];
        }

        ToArmour.Enchantments = new Power[FromArmour.Enchantments.Count];

        for (int i = 0; i < FromArmour.Enchantments.Count; i++)
        {
            ToArmour.Enchantments[i] = new Power();

            ToArmour.Enchantments[i].PowerType = FromArmour.Enchantments[i].PowerType;
            ToArmour.Enchantments[i].PowerID = FromArmour.Enchantments[i].PowerID;
        }

        ToArmour.IsEquiped = FromArmour.GetEquiped();

        ToArmour.Name = FromArmour.GetName();

        ToArmour.ItemId = 0;
    }

    public static void LoadItem(SpellHolder FromSpell, SpellHolderData ToSpell)
    {
        ToSpell.SpellsData = new SpellData[FromSpell.GetNumOfSpells()];

        for (int i = 0; i < FromSpell.Spells.Length; i++)
        {
            if (FromSpell.Spells[i] == null)
            {
                break;
            }

            Spell SpellH = FromSpell.Spells[i];

            ToSpell.SpellsData[i] = new SpellData();

            SpellData spellData = ToSpell.SpellsData[i];

            LoadRune(FromSpell.Spells[i], spellData);
        }

        ToSpell.MaterialMulti = FromSpell.ValueMulti;
        ToSpell.MaterialId = (int)FromSpell.Type;
        ToSpell.SpellSkillType = (int)FromSpell.SkillType;
        ToSpell.Amount = FromSpell.GetAmount();
        ToSpell.Name = FromSpell.GetName();

        ToSpell.Rarity = new float[4];

        ToSpell.Rarity[0] = FromSpell.GetRarity().r;
        ToSpell.Rarity[1] = FromSpell.GetRarity().g;
        ToSpell.Rarity[2] = FromSpell.GetRarity().b;
        ToSpell.Rarity[3] = FromSpell.GetRarity().a;
    }

    public static void LoadItem(RuneHolder FromRuneH, RuneHolderData ToRuneH)
    {
        ToRuneH.Amount = FromRuneH.GetAmount();
        ToRuneH.Name = FromRuneH.GetName();

        ToRuneH.Rarity = new float[4];

        ToRuneH.Rarity[0] = FromRuneH.GetRarity().r;
        ToRuneH.Rarity[1] = FromRuneH.GetRarity().g;
        ToRuneH.Rarity[2] = FromRuneH.GetRarity().b;
        ToRuneH.Rarity[3] = FromRuneH.GetRarity().a;

        ToRuneH.runeData = new SpellData();

        LoadRune(FromRuneH.spell, ToRuneH.runeData);
    }

    private static void LoadRune(Spell FromRune, SpellData ToRune)
    {
        if (FromRune is DamageSpell spellD)
        {
            ToRune.SpellTypeId = 0;

            ToRune.StatArray2 = new int[spellD.DamageRanges.Count];
            ToRune.StatArray3 = new int[spellD.DamageRanges.Count];
            ToRune.StatArray4 = new int[spellD.DamageRanges.Count];

            for (int x = 0; x < ToRune.StatArray2.Length; x++)
            {
                ToRune.StatArray2[x] = (int)spellD.DamageRanges[x].Type;
                ToRune.StatArray3[x] = spellD.DamageRanges[x].LDamage;
                ToRune.StatArray4[x] = spellD.DamageRanges[x].HDamage;
            }

            ToRune.StatArray1 = new int[spellD.StatusChance.Count];

            for (int x = 0; x < ToRune.StatArray1.Length; x++)
            {
                ToRune.StatArray1[x] = spellD.StatusChance[x];
            }

            ToRune.int0 = spellD.CritDamage;

            ToRune.CastRate = spellD.CastsPerSecond;

            for (int x = 0; x < PrefabIDs.prefabIDs.SpellAffects.Length; x++)
            {
                if (spellD.SpellAffect == PrefabIDs.prefabIDs.SpellAffects[x])
                {
                    ToRune.SpellAffectID = x;
                    break;
                }
            }
        }
        else if (FromRune is GolemSpell spellG)
        {
            ToRune.SpellTypeId = 1;

            ToRune.bool0 = spellG.Activated;

            ToRune.int0 = spellG.Number;

            ToRune.StatArray0 = new int[3];

            ToRune.StatArray0[0] = (int)spellG.DamageRange.Type;
            ToRune.StatArray0[1] = spellG.DamageRange.LDamage;
            ToRune.StatArray0[2] = spellG.DamageRange.HDamage;

            for (int x = 0; x < PrefabIDs.prefabIDs.Minions.Length; x++)
            {
                if (spellG.SpellAffect == PrefabIDs.prefabIDs.Minions[x])
                {
                    ToRune.SpellAffectID = x;
                    break;
                }
            }
        }

        ToRune.Name = FromRune.Name;
        ToRune.ManaCost = FromRune.Cost;
        ToRune.CostType = (int)FromRune.CostType;
        ToRune.CastType = (int)FromRune.CastType;
        ToRune.Target = (int)FromRune.Target;
    }

    public static void LoadItem(QuestHolder FromQuest, QuestData ToQuest)
    {
        ToQuest.HourAquired = FromQuest.HourAquired;
        ToQuest.MintueAquired = FromQuest.MintueAquired;
        ToQuest.DateAquiredDay = FromQuest.DateAquired[0];
        ToQuest.DateAquiredMonth = FromQuest.DateAquired[1];
        ToQuest.DateAquiredYear = FromQuest.DateAquired[2];
        ToQuest.CurrentQuestStep = FromQuest.CurrentQuestStep;
        ToQuest.Location = FromQuest.Location;
    }
}
