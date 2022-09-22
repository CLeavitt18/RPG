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

    public void SetStats(DamageStats stats)
    {
        Parent = stats.Parent;

        SourceHand = stats.SourceHand;

        LifeSteal = stats.LifeSteal;

        Clear();

        for (int i = 0; i < stats.DamageTypes.Count; i++)
        {
            DamageTypes.Add(stats.DamageTypes[i]);
        }

        for (int i = 0; i < stats.DamageValues.Count; i++)
        {
            DamageValues.Add(stats.DamageValues[i]);
        }

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
    public bool attackFinsihed = true;

    public float NextAttack;
    public float ChannelTime;
}

[Serializable]
public class WeaponData
{
    public int LifeSteal;
    public int CritDamage;
    public int PwrAttackDamage;
    public int WeaponSkillType;
    public int HandType;
    public int Material;
    public int Primary;
    public int Secoundary;
    public int Teritiary;
    public int CurrentDurability;
    public int MaxDurability;
    public int Amount;
    public int Value;
    public int Weight;

    public int[] StatusChance;
    public int[] AnimatorId = new int[2];

    public DamageTypeStruct[] DamageRanges;
    
    public WeaponType Type;

    public MaterialType[] Materials;

    public float[] Rarity;

    public float AttacksPerSecond;

    public string Name;
    public string AttackAnimationName;
    public string PwrAttackAnimationName;
}

[Serializable]
public class ItemStats
{
    public int Amount = 1;
    public int Value;
    public int Weight;

    public string Name;

    public GameObject Item = null;

    public Color Rarity;
}

[Serializable]
public class WeaponStats : ItemStats
{
    public SkillType WeaponSkillType;
    public HandType HandType;
    public WeaponType Type;

    public MaterialType[] Materials = new MaterialType[3];
    
    public int LifeSteal;
    public int CritDamage;
    public int PwrAttackDamage;
    public int CurrentDurability;
    public int MaxDurability;

    public Material Material;
    
    public GameObject Primary;
    public GameObject Secoundary;
    public GameObject Teritiary;

    public List<int> StatusChance = new List<int>();

    public List<DamageTypeStruct> DamageRanges = new List<DamageTypeStruct>();

    public RuntimeAnimatorController[] Animator = new RuntimeAnimatorController[2];

    public float ActionsPerSecond;

    public string AttackAnimationName;
    public string PwrAttackAnimationName;
}

[Serializable]
public class ArmourData
{
    public int Armour;
    public int CurrentDurability;
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
    public bool IsShield;

    public string Name;
}

[Serializable]
public class ArmourStats : ItemStats
{
    public int Armour;
    public int CurrentDurability;
    public int MaxDurability;

    public ArmourType ArmourType;
    public SkillType SkillType;

    public int[] Resistences = new int[3];

    public Power[] Enchantments = new Power[0];

    public bool IsEquiped;
}

[Serializable]
public class SpellHolderData
{
    public int handType;
    public int SpellSkillType;
    public int Amount;
    public int MaterialId;
    public int MaterialMulti;

    public float[] Rarity;

    public SpellData[] SpellsData;

    public string Name;
}

[Serializable]
public class SpellHolderStats : ItemStats
{
    public HandType handType;
    public SkillType SpellSkillType;
    public MaterialType Type;

    public int MaterialMulti;

    public SpellData[] Spells = new SpellData[3];
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
    public int ManaCost;
    public int CostType;
    public int SkillType;
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
public class SpellStats
{
    public string Name;

    public CastType CastType;

    public AttributesEnum CostType;

    public SpellType SpellType;

    public SkillType SkillType;

    public GameObject SpellAffect;

    public int ManaCost;

    public float CastRate;
}

[Serializable]
public class RuneHolderStats : ItemStats
{
    public Spell spell = null;
}

[Serializable]
public class DamageSpellStats : SpellStats
{
    public int CritDamage;

    public List<DamageTypeStruct> ranges;

    public List<int> StatusChances;
}

[Serializable]
public class GolemSpellStats : SpellStats
{
    public int number;

    public bool activated;

    public DamageTypeStruct range;
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

    public bool complete;
    
    public bool[] itemCompletes;
}

[Serializable]
public class QuestItemCompleteConditon
{
    public QuestItemHolder item;
    public int amount;
    public bool complete;
}

[Serializable]
public class QuestEnemyCompleteConditon
{
    public AIController enemy;
    public int amount;
    public bool complete;
}