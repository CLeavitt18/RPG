using System.Collections.Generic;
using UnityEngine;

public static class LoadSystem
{
    public static void LoadItem(WeaponData FromWeapon, WeaponHolder ToWeapon)
    {
        WeaponStats stats = new WeaponStats();

        stats.CritDamage = FromWeapon.CritDamage;
        stats.PwrAttackDamage = FromWeapon.PwrAttackDamage;
        stats.Type = FromWeapon.Type;
        stats.WeaponSkillType = (SkillType)FromWeapon.WeaponSkillType;
        stats.HandType = (HandType)FromWeapon.HandType;

        for (int i = 0; i < FromWeapon.DamageRanges.Length; i++)
        {
            stats.DamageRanges.Add(FromWeapon.DamageRanges[i]);
            stats.StatusChance.Add(FromWeapon.StatusChance[i]);
        }

        stats.ActionsPerSecond = FromWeapon.AttacksPerSecond;
        stats.Weight = FromWeapon.Weight;
        stats.Value = FromWeapon.Value;
        stats.Name = FromWeapon.Name;
        stats.LifeSteal = FromWeapon.LifeSteal;
        stats.CurrentDurability = FromWeapon.CurrentDurability;
        stats.MaxDurability = FromWeapon.MaxDurability;
        stats.AttackAnimationName = FromWeapon.AttackAnimationName;
        stats.PwrAttackAnimationName = FromWeapon.PwrAttackAnimationName;
        stats.Amount = FromWeapon.Amount;

        Color color = new Color(
            FromWeapon.Rarity[0], 
            FromWeapon.Rarity[1],
            FromWeapon.Rarity[2],
            FromWeapon.Rarity[3]);

        stats.Rarity = color;

        for (int i = 0; i < 3; i++)
        {
            stats.Materials[i] = FromWeapon.Materials[i];
        }

        stats.Material = PrefabIDs.prefabIDs.WeaponMaterials[FromWeapon.Material];
        stats.Primary = PrefabIDs.prefabIDs.WeaponParts[FromWeapon.Primary];
        stats.Secoundary = PrefabIDs.prefabIDs.WeaponParts[FromWeapon.Secoundary];
        stats.Teritiary = PrefabIDs.prefabIDs.WeaponParts[FromWeapon.Teritiary];
        stats.Animator[0] = PrefabIDs.prefabIDs.Animators[FromWeapon.AnimatorId[0]];
        stats.Animator[1] = PrefabIDs.prefabIDs.Animators[FromWeapon.AnimatorId[1]];

        ToWeapon.SetWeaponState(stats);
    }

    public static void LoadItem(ArmourData FromArmour, ArmourHolder ToArmour)
    {
        ArmourStats stats = new ArmourStats();

        stats.Armour = FromArmour.Armour;
        stats.CurrentDurability = FromArmour.CurrentDurability;
        stats.MaxDurability = FromArmour.MaxDurablity;
        stats.ArmourType = (ArmourType)FromArmour.ArmourType;
        stats.Value = FromArmour.Value;
        stats.Amount = FromArmour.Amount;
        stats.Weight = FromArmour.Weight;
        stats.SkillType = (SkillType)FromArmour.SkillType;

        if (ToArmour is ShieldHolder)
        {
            stats.Item = PrefabIDs.prefabIDs.Armour[0];
        }

        stats.Resistences = new ResistenceType[FromArmour.Resistences.Length];

        for (int i = 0; i < FromArmour.Resistences.Length; i++)
        {
            stats.Resistences[i].Type = FromArmour.Resistences[i].Type;
            stats.Resistences[i].resistence = FromArmour.Resistences[i].resistence;
        }

        stats.Enchantments = new Power[FromArmour.Enchantments.Length];

        for (int i = 0; i < FromArmour.Enchantments.Length; i++)
        {
            stats.Enchantments[i] = new Power(FromArmour.Enchantments[i]);
        }

        stats.IsEquiped = FromArmour.IsEquiped;

        stats.Name = FromArmour.Name;

        ToArmour.SetStats(stats);
    }

    public static void LoadItem(SpellHolderData FromSpell, SpellHolder ToSpell)
    {
        SpellHolderStats stats = new SpellHolderStats();
  
        SpellData spellData;

        for (int i = 0; i < 3; i++)
        {
            spellData = FromSpell.SpellsData[i];

            if (spellData.SpellTypeId == (int)SpellType.None)
            {
                stats.Spells[i] = null;

                continue;
            }
            
            stats.Spells[i] = spellData;
        }

        stats.MaterialMulti = FromSpell.MaterialMulti;
        stats.Type = (MaterialType)FromSpell.MaterialId;
        stats.SpellSkillType = (SkillType)FromSpell.SpellSkillType;
        stats.Amount = FromSpell.Amount;
        stats.Name = FromSpell.Name;

        Color color = new Color(
            FromSpell.Rarity[0],
            FromSpell.Rarity[1],
            FromSpell.Rarity[2],
            FromSpell.Rarity[3]);

        stats.Rarity = color;

        ToSpell.SetSpellState(stats);
    }

    public static void LoadItem(RuneHolderData FromRuneH, RuneHolder ToRuneH)
    {
        RuneHolderStats stats = new RuneHolderStats();

        stats.Name = FromRuneH.Name;

        stats.Amount = FromRuneH.Amount;

        Color color = new Color(
            FromRuneH.Rarity[0],
            FromRuneH.Rarity[1],
            FromRuneH.Rarity[2],
            FromRuneH.Rarity[3]);

        stats.Rarity = color;

        if (FromRuneH.runeData.SpellTypeId == 0)
        {
            stats.spell = ToRuneH.gameObject.AddComponent<DamageSpell>();
        }
        else if (FromRuneH.runeData.SpellTypeId == 1)
        {
            stats.spell = ToRuneH.gameObject.AddComponent<GolemSpell>();
        }

        ToRuneH.SetStats(stats);

        LoadRune(FromRuneH.runeData, ToRuneH.GetSpell());
    }

    public static void LoadRune(SpellData FromRune, Spell ToRune)
    {
        SpellStats stats = null;

        if (ToRune is DamageSpell)
        {
            stats = new DamageSpellStats();

            DamageSpellStats statsD = stats as DamageSpellStats;

            statsD.ranges = new List<DamageType>();

            for (int x = 0; x < FromRune.StatArray2.Length; x++)
            {
                statsD.ranges.Add(new DamageType());
                {
                    statsD.ranges[x].Type = (DamageTypeEnum)FromRune.StatArray2[x];
                    statsD.ranges[x].LDamage = FromRune.StatArray3[x];
                    statsD.ranges[x].HDamage = FromRune.StatArray4[x];
                }
            }

            statsD.StatusChances = new List<int>();

            for (int x = 0; x < FromRune.StatArray1.Length; x++)
            {
                statsD.StatusChances.Add(FromRune.StatArray1[x]);
            }

            statsD.CritDamage = FromRune.int0;

            statsD.CastRate = FromRune.CastRate;

            statsD.SpellAffect = PrefabIDs.prefabIDs.SpellAffects[FromRune.SpellAffectID];
        }
        else if (ToRune is GolemSpell)
        {
            stats = new GolemSpellStats();

            GolemSpellStats spellG = stats as GolemSpellStats;

            spellG.SpellType = (SpellType)FromRune.SpellTypeId;

            spellG.activated = FromRune.bool0;

            spellG.number = FromRune.int0;

            spellG.range = new DamageType()
            {
                Type = (DamageTypeEnum)FromRune.StatArray0[0],
                LDamage = FromRune.StatArray0[1],
                HDamage = FromRune.StatArray0[2],
            };

            spellG.SpellAffect = PrefabIDs.prefabIDs.Minions[FromRune.SpellAffectID];
        }

        stats.Name = FromRune.Name;
        stats.ManaCost = FromRune.ManaCost;
        stats.CostType = (AttributesEnum)FromRune.CostType;
        stats.CastType = (CastType)FromRune.CastType;
        stats.SkillType = (SkillType)FromRune.SkillType; 

        ToRune.SetStats(stats);
    }

    public static QuestHolder LoadItem(QuestData FromQuest, QuestHolder ToQuest)
    {
        ToQuest.HourAquired = FromQuest.HourAquired;
        ToQuest.MintueAquired = FromQuest.MintueAquired;
        ToQuest.DateAquired[0] = FromQuest.DateAquiredDay;
        ToQuest.DateAquired[1] = FromQuest.DateAquiredMonth;
        ToQuest.DateAquired[2] = FromQuest.DateAquiredMonth;
        ToQuest.SetComplete(FromQuest.complete);
        ToQuest.SetAllCompletes(FromQuest.itemCompletes);
        ToQuest.CurrentQuestStep = FromQuest.CurrentQuestStep;


        return ToQuest;
    }

    public static void LoadItem(WeaponHolder FromWeapon, WeaponData ToWeapon)
    {
        ToWeapon.CritDamage = FromWeapon.GetCrit();
        ToWeapon.PwrAttackDamage = FromWeapon.GetPowerAttack();
        ToWeapon.Type = FromWeapon.GetWeaponType();
        ToWeapon.WeaponSkillType = (int)FromWeapon.GetSkill();
        ToWeapon.HandType = (int)FromWeapon.GetHandType();

        ToWeapon.DamageRanges = new DamageType[FromWeapon.GetDamageRangesCount()];
        ToWeapon.StatusChance = new int[FromWeapon.GetStatusCount()];

        for (int i = 0; i < ToWeapon.DamageRanges.Length; i++)
        {
            ToWeapon.DamageRanges[i] = new DamageType()
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
                ToWeapon.Material = y;
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

    public static void LoadItem(ArmourHolder FromArmour, ArmourData ToArmour)
    {
        ToArmour.Armour = FromArmour.GetArmour();
        ToArmour.CurrentDurability = FromArmour.GetCurrentDurability();
        ToArmour.MaxDurablity = FromArmour.GetMaxDurability();
        ToArmour.ArmourType = (int)FromArmour.GetArmourType();
        ToArmour.Value = FromArmour.GetValue();
        ToArmour.Amount = FromArmour.GetAmount();
        ToArmour.Weight = FromArmour.GetWeight();
        ToArmour.SkillType = (int)FromArmour.GetSkillType();

        if (FromArmour is ShieldHolder)
        {
            ToArmour.IsShield = true;
        }

        ToArmour.Rarity = new float[4];

        ToArmour.Rarity[0] = FromArmour.GetRarity().r;
        ToArmour.Rarity[1] = FromArmour.GetRarity().g;
        ToArmour.Rarity[2] = FromArmour.GetRarity().b;
        ToArmour.Rarity[3] = FromArmour.GetRarity().a;

        ToArmour.Resistences = new ResistenceType[FromArmour.GetResistenceCount()];

        for (int i = 0; i < ToArmour.Resistences.Length; i++)
        {
            ToArmour.Resistences[i].resistence = FromArmour.GetResistence(i);
            ToArmour.Resistences[i].Type = FromArmour.GetResistenceType(i);
        }

        ToArmour.Enchantments = new Power[FromArmour.GetEnchantCount()];

        for (int i = 0; i < ToArmour.Enchantments.Length; i++)
        {
            ToArmour.Enchantments[i] = new Power(FromArmour.GetEnchantment(i));
        }

        ToArmour.IsEquiped = FromArmour.GetEquiped();

        ToArmour.Name = FromArmour.GetName();

        ToArmour.ItemId = 0;
    }

    public static void LoadItem(SpellHolder FromSpell, SpellHolderData ToSpell)
    {
        ToSpell.SpellsData = new SpellData[3];

        for (int i = 0; i < 3; i++)
        {
            if (FromSpell.GetRune(i) == null)
            {
                ToSpell.SpellsData[i] = new SpellData();
                ToSpell.SpellsData[i].SpellTypeId = (int)SpellType.None;
                continue;
            }

            Spell SpellH = FromSpell.GetRune(i);

            ToSpell.SpellsData[i] = new SpellData();

            SpellData spellData = ToSpell.SpellsData[i];

            LoadRune(FromSpell.GetRune(i), spellData);
        }

        ToSpell.MaterialMulti = FromSpell.GetValueMulti();
        ToSpell.MaterialId = (int)FromSpell.GetMaterialType();
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

        LoadRune(FromRuneH.GetSpell(), ToRuneH.runeData);
    }

    public static void LoadRune(Spell FromRune, SpellData ToRune)
    {
        if (FromRune is DamageSpell spellD)
        {
            ToRune.SpellTypeId = (int)SpellType.DamageSpell;

            ToRune.StatArray2 = new int[spellD.DamageRanges.Count];
            ToRune.StatArray3 = new int[spellD.DamageRanges.Count];
            ToRune.StatArray4 = new int[spellD.DamageRanges.Count];

            for (int x = 0; x < ToRune.StatArray2.Length; x++)
            {
                ToRune.StatArray2[x] = (int)spellD.DamageRanges[x].Type;
                ToRune.StatArray3[x] = spellD.DamageRanges[x].LDamage;
                ToRune.StatArray4[x] = spellD.DamageRanges[x].HDamage;
            }

            ToRune.StatArray1 = new int[spellD.GetDamageTypeCount()];

            for (int x = 0; x < ToRune.StatArray1.Length; x++)
            {
                ToRune.StatArray1[x] = spellD.GetStatusChance(x);
            }

            ToRune.int0 = spellD.GetCritDamage();

            ToRune.CastRate = spellD.GetCastRate();

            GameObject spellAffect = spellD.GetSpellAffect();

            for (int x = 0; x < PrefabIDs.prefabIDs.SpellAffects.Length; x++)
            {
                if (spellAffect == PrefabIDs.prefabIDs.SpellAffects[x])
                {
                    ToRune.SpellAffectID = x;
                    break;
                }
            }
        }
        else if (FromRune is GolemSpell spellG)
        {
            ToRune.SpellTypeId = (int)SpellType.GolemSpell;

            ToRune.bool0 = spellG.Activated;

            ToRune.int0 = spellG.Number;

            ToRune.StatArray0 = new int[3];

            ToRune.StatArray0[0] = (int)spellG.DamageRange.Type;
            ToRune.StatArray0[1] = spellG.DamageRange.LDamage;
            ToRune.StatArray0[2] = spellG.DamageRange.HDamage;

            GameObject spellAffect = spellG.GetSpellAffect();

            for (int x = 0; x < PrefabIDs.prefabIDs.Minions.Length; x++)
            {
                if (spellAffect == PrefabIDs.prefabIDs.Minions[x])
                {
                    ToRune.SpellAffectID = x;
                    break;
                }
            }
        }

        ToRune.Name = FromRune.GetName();
        ToRune.ManaCost = FromRune.GetCost();
        ToRune.CostType = (int)FromRune.GetCostType();
        ToRune.CastType = (int)FromRune.GetCastType();
        ToRune.SkillType = (int)FromRune.GetSkillType();
    }

    public static void LoadItem(QuestHolder FromQuest, QuestData ToQuest)
    {
        ToQuest.HourAquired = FromQuest.HourAquired;
        ToQuest.MintueAquired = FromQuest.MintueAquired;
        ToQuest.DateAquiredDay = FromQuest.DateAquired[0];
        ToQuest.DateAquiredMonth = FromQuest.DateAquired[1];
        ToQuest.DateAquiredYear = FromQuest.DateAquired[2];
        ToQuest.CurrentQuestStep = FromQuest.CurrentQuestStep;
        ToQuest.complete = FromQuest.GetComplete();
        ToQuest.itemCompletes = FromQuest.GetAllCompletes();
        ToQuest.Location = FromQuest.Location;
    }
}
