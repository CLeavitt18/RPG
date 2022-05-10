using System.Collections.Generic;
using UnityEngine;

public static class LoadSystem
{
    public static void LoadItem(WeaponStats FromWeapon, WeaponHolder ToWeapon)
    {
        ToWeapon.CritDamage = FromWeapon.CritDamage;
        ToWeapon.Type = FromWeapon.Type;
        ToWeapon.SkillType = (SkillType)FromWeapon.WeaponSkillType;
        ToWeapon.HandType = (HandType)FromWeapon.HandType;

        for (int i = 0; i < FromWeapon.DamageRanges.Length; i++)
        {
            ToWeapon.DamageRanges.Add(FromWeapon.DamageRanges[i]);
            ToWeapon.StatusChance.Add(FromWeapon.StatusChance[i]);
        }

        ToWeapon.ActionsPerSecond = FromWeapon.AttacksPerSecond;
        ToWeapon.Weight = FromWeapon.Weight;
        ToWeapon.Value = FromWeapon.Value;
        ToWeapon.Name = FromWeapon.Name;
        ToWeapon.LifeSteal = FromWeapon.LifeSteal;
        ToWeapon.CurrentDurability = FromWeapon.CurrentDurability;
        ToWeapon.MaxDurability = FromWeapon.MaxDurability;
        ToWeapon.AttackAnimationName = FromWeapon.AttackAnimationName;
        ToWeapon.PwrAttackAnimationName = FromWeapon.PwrAttackAnimationName;
        ToWeapon.Amount = FromWeapon.Amount;

        Color color = new Color(
            FromWeapon.Rarity[0], 
            FromWeapon.Rarity[1],
            FromWeapon.Rarity[2],
            FromWeapon.Rarity[3]);

        ToWeapon.Rarity = color;

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
        ToArmour.Value = FromArmour.Value;
        ToArmour.Amount = FromArmour.Amount;
        ToArmour.Weight = FromArmour.Weight;
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

        ToArmour.IsEquiped = FromArmour.IsEquiped;

        ToArmour.Name = FromArmour.Name;

        ToArmour._Item = null;

        ToArmour.SetArmourState();
    }

    public static void LoadItem(SpellHolderData FromSpell, SpellHolder ToSpell)
    {
        ToSpell.Spells = new Spell[FromSpell.SpellsData.Length];

        for (int i = 0; i < FromSpell.SpellsData.Length; i++)
        {
            SpellData spellData = FromSpell.SpellsData[i];

            if (spellData.SpellTypeId == 0)
            {
                ToSpell.Spells[i] = ToSpell.gameObject.AddComponent<DamageSpell>();
            }
            else if (spellData.SpellTypeId == 1)
            {
                ToSpell.Spells[i] = ToSpell.gameObject.AddComponent<GolemSpell>();
            }

            LoadRune(spellData, ToSpell.Spells[i]);
        }

        ToSpell.ValueMulti = FromSpell.MaterialMulti;
        ToSpell.Type = (MaterialType)FromSpell.MaterialId;
        ToSpell.SkillType = (SkillType)FromSpell.SpellSkillType;
        ToSpell.Amount = FromSpell.Amount;
        ToSpell.Name = FromSpell.Name;

        Color color = new Color(
            FromSpell.Rarity[0],
            FromSpell.Rarity[1],
            FromSpell.Rarity[2],
            FromSpell.Rarity[3]);

        ToSpell.Rarity = color;

        ToSpell.SetSpellState();
    }

    public static void LoadItem(RuneHolderData FromRuneH, RuneHolder ToRuneH)
    {
        ToRuneH.Name = FromRuneH.Name;
        ToRuneH.Amount = FromRuneH.Amount;

        Color color = new Color(
            FromRuneH.Rarity[0],
            FromRuneH.Rarity[1],
            FromRuneH.Rarity[2],
            FromRuneH.Rarity[3]);

        ToRuneH.Rarity = color;

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
        ToWeapon.CritDamage = FromWeapon.CritDamage;
        ToWeapon.Type = FromWeapon.Type;
        ToWeapon.WeaponSkillType = (int)FromWeapon.SkillType;
        ToWeapon.HandType = (int)FromWeapon.HandType;

        ToWeapon.DamageRanges = new DamageTypeStruct[FromWeapon.DamageRanges.Count];
        ToWeapon.StatusChance = new int[FromWeapon.StatusChance.Count];

        for (int i = 0; i < FromWeapon.DamageRanges.Count; i++)
        {
            ToWeapon.DamageRanges[i] = FromWeapon.DamageRanges[i];
            ToWeapon.StatusChance[i] = FromWeapon.StatusChance[i];
        }

        ToWeapon.AttacksPerSecond = FromWeapon.ActionsPerSecond;
        ToWeapon.Weight = FromWeapon.Weight;
        ToWeapon.Value = FromWeapon.Value;
        ToWeapon.Name = FromWeapon.Name;
        ToWeapon.LifeSteal = FromWeapon.LifeSteal;
        ToWeapon.CurrentDurability = FromWeapon.CurrentDurability;
        ToWeapon.MaxDurability = FromWeapon.MaxDurability;
        ToWeapon.AttackAnimationName = FromWeapon.AttackAnimationName;
        ToWeapon.PwrAttackAnimationName = FromWeapon.PwrAttackAnimationName;
        ToWeapon.Amount = FromWeapon.Amount;

        ToWeapon.Rarity = new float[4];

        ToWeapon.Rarity[0] = FromWeapon.Rarity.r;
        ToWeapon.Rarity[1] = FromWeapon.Rarity.g;
        ToWeapon.Rarity[2] = FromWeapon.Rarity.b;
        ToWeapon.Rarity[3] = FromWeapon.Rarity.a;

        ToWeapon.Materials = new MaterialType[3];

        for (int i = 0; i < 3; i++)
        {
            ToWeapon.Materials[i] = FromWeapon.Materials[i];
        }

        for (int y = 0; y < PrefabIDs.prefabIDs.WeaponMaterials.Length; y++)
        {
            if (PrefabIDs.prefabIDs.WeaponMaterials[y] == FromWeapon.Material)
            {
                ToWeapon.Materail = y;
                break;
            }
        }

        for (int y = 0; y < PrefabIDs.prefabIDs.WeaponParts.Length; y++)
        {
            if (PrefabIDs.prefabIDs.WeaponParts[y] == FromWeapon.Primary)
            {
                ToWeapon.Primary = y;
                break;
            }
        }

        for (int y = 0; y < PrefabIDs.prefabIDs.WeaponParts.Length; y++)
        {
            if (PrefabIDs.prefabIDs.WeaponParts[y] == FromWeapon.Secoundary)
            {
                ToWeapon.Secoundary = y;
                break;
            }
        }

        for (int y = 0; y < PrefabIDs.prefabIDs.WeaponParts.Length; y++)
        {
            if (PrefabIDs.prefabIDs.WeaponParts[y] == FromWeapon.Teritiary)
            {
                ToWeapon.Teritiary = y;
                break;
            }
        }

        for (int i = 0; i < 2; i++)
        {
            for (int x = 0; x < PrefabIDs.prefabIDs.Animators.Length; x++)
            {
                if (PrefabIDs.prefabIDs.Animators[x] == FromWeapon.Animator[i])
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
        ToArmour.Value = FromArmour.Value;
        ToArmour.Amount = FromArmour.Amount;
        ToArmour.Weight = FromArmour.Weight;
        ToArmour.SkillType = (int)FromArmour.SkillType;

        ToArmour.Rarity = new float[4];

        ToArmour.Rarity[0] = FromArmour.Rarity.r;
        ToArmour.Rarity[1] = FromArmour.Rarity.g;
        ToArmour.Rarity[2] = FromArmour.Rarity.b;
        ToArmour.Rarity[3] = FromArmour.Rarity.a;

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

        ToArmour.IsEquiped = FromArmour.IsEquiped;

        ToArmour.Name = FromArmour.Name;

        ToArmour.ItemId = 0;
    }

    public static void LoadItem(SpellHolder FromSpell, SpellHolderData ToSpell)
    {
        ToSpell.SpellsData = new SpellData[FromSpell.Spells.Length];

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
        ToSpell.Amount = FromSpell.Amount;
        ToSpell.Name = FromSpell.Name;

        ToSpell.Rarity = new float[4];

        ToSpell.Rarity[0] = FromSpell.Rarity.r;
        ToSpell.Rarity[1] = FromSpell.Rarity.g;
        ToSpell.Rarity[2] = FromSpell.Rarity.b;
        ToSpell.Rarity[3] = FromSpell.Rarity.a;
    }

    public static void LoadItem(RuneHolder FromRuneH, RuneHolderData ToRuneH)
    {
        ToRuneH.Amount = FromRuneH.Amount;
        ToRuneH.Name = FromRuneH.Name;

        ToRuneH.Rarity = new float[4];

        ToRuneH.Rarity[0] = FromRuneH.Rarity.r;
        ToRuneH.Rarity[1] = FromRuneH.Rarity.g;
        ToRuneH.Rarity[2] = FromRuneH.Rarity.b;
        ToRuneH.Rarity[3] = FromRuneH.Rarity.a;

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
