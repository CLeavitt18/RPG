using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DamageStats
{
    public LivingEntities Parent;

    public int SourceHand;

    public int LifeSteal;

    public List<DamageTypeEnum> DamageTypes;
    public List<int> DamageValues;
    public List<bool> Status;

    public DamageStats()
    {
        Parent = null;

        SourceHand = 0;

        LifeSteal = 0;

        DamageTypes = new List<DamageTypeEnum>();
        DamageValues = new List<int>();
        Status = new List<bool>();
    }

    public DamageStats(DamageStats stats)
    {
        Parent = stats.Parent;

        SourceHand = stats.SourceHand;

        LifeSteal = stats.LifeSteal;

        DamageTypes = new List<DamageTypeEnum>();

        for (int i = 0; i < stats.DamageTypes.Count; i++)
        {
            DamageTypes.Add(stats.DamageTypes[i]);
        }

        DamageValues = new List<int>();

        for (int i = 0; i < stats.DamageValues.Count; i++)
        {
            DamageValues.Add(stats.DamageValues[i]);
        }

        Status = new List<bool>();

        for (int i = 0; i < stats.Status.Count; i++)
        {
            Status.Add(stats.Status[i]);
        }
    }

    public void Clear()
    {
        DamageTypes.Clear();
        DamageValues.Clear();
        Status.Clear();
    }
}

[Serializable]
public class DamageTypeStruct
{
    public DamageTypeEnum Type;
    public int LDamage;
    public int HDamage;

    public DamageTypeStruct()
    {
        Type = DamageTypeEnum.Physical;
        LDamage = 0;
        HDamage = 0;
    }

    public DamageTypeStruct(DamageTypeStruct dTStruct, int multi)
    {
        Type = dTStruct.Type;
        LDamage = dTStruct.LDamage * multi;
        HDamage = dTStruct.HDamage * multi;
    }
}

[Serializable]
public class Hand
{
    public AttackType State;

    public DamageMultipliers DamageMultis;

    public Transform HandLocation;
    public Transform WeaponSpawn;

    public Animator Animator;

    public Item HeldItem;

    public DamageStats Stats;

    public bool HasAttacked;

    public float NextAttack;
    public float ChannelTime;
    public float ActionsPerSecond;
}

[Serializable]
public class ItemData
{
    int Amount;

    string Name;
}


[Serializable]
public class WeaponStats
{
    public int LifeSteal;
    public int CritDamage;
    public int WeaponSkillType;
    public int HandType;
    public int Materail;
    public int Primary;
    public int Secoundary;
    public int Teritiary;
    public int CurrentDurability;
    public int MaxDurability;
    public int AltAnimatorId;
    public int Amount;
    public int Value;
    public int Weight;

    public int[] StatusChance;

    public DamageTypeStruct[] DamageRanges;

    public int[] AnimatorId = new int[2];
    
    public WeaponType Type;

    public MaterialType[] Materials;

    public float[] Rarity;

    public float AttacksPerSecond;

    public string Name;
    public string AttackAnimationName;
    public string PwrAttackAnimationName;
}

[Serializable]
public class ArmourStats
{
    public int Armour;
    public int CurrentDurablity;
    public int MaxDurablity;
    public int ArmourType;
    public int Value;
    public int Amount;
    public int Weight;
    public int SkillType;
    public int ItemId;

    public float[] Rarity;

    public int[] Resistences = new int[3];

    public Power[] Enchantments;

    public bool IsEquiped;

    public string Name;
}

[Serializable]
public class SpellHolderData
{
    public int SpellSkillType;
    public int Amount;
    public int MaterialId;
    public int MaterialMulti;

    public float[] Rarity;

    public SpellData[] SpellsData;

    public string Name;
}
[Serializable]
public class RuneHolderData
{
    public SpellData runeData;

    public int Amount;

    public string Name;

    public float[] Rarity;
}

[Serializable]
public class SpellData
{
    public string Name;

    public int CastType;
    public int Target;
    public int ManaCost;
    public int CostType;
    public int SpellAffectID;
    public int SpellTypeId;

    public int int0;

    public bool bool0;

    public float CastRate;

    public int[] StatArray0;
    public int[] StatArray1;
    public int[] StatArray2;
    public int[] StatArray3;
    public int[] StatArray4;
}

[Serializable]
public class QuestData
{
    public int QuestId;
    public int HourAquired;
    public int MintueAquired;
    public int DateAquiredDay;
    public int DateAquiredMonth;
    public int DateAquiredYear;
    public int CurrentQuestStep;

    public string Location;
}
