using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct Power
{
    public int PowerType;
    public int PowerID;

    public Power(Power power)
    {
        PowerType = power.PowerType;
        PowerID = power.PowerID;
    }
}

[Serializable]
public struct Skill
{
    public int Level;
    public ulong Exp;
    public ulong RExp;
}

[Serializable]
public struct EnemyWeaponData
{
    public WeaponType Type;
    
    public MaterialType PrimaryIds;
    public MaterialType SecondaryIds;
    public MaterialType TertiaryIds;

    public CatType WeaponCatayst;
}

[Serializable]
public struct AttributeStruct
{
    public int Ability;
    public int RegenAmount;
    public int APerLevel;

    public float APercentPerLevel;

    public long Current;
    public long Max;
    public long Reserved;
}

[Serializable]
public struct DamageMultipliers
{
    public float[] Melee;
    public float[] Spell;
    public float[] Ranged;
}

[Serializable]
public struct WeaponSpawns
{
    public Transform PrimarySpawn;
    public Transform SecoundarySpawn;
    public Transform TeritiarySpawn;
}

[Serializable]
public struct DialogueSet
{
    public string[] Dialogue;
}

[Serializable]
public struct Range
{
    public int min;
    public int max;
}

[Serializable]
public struct CraftingMaterials
{
    public int ResourceId;
    public int Amount;
}

[Serializable]
public struct SkillsText
{
    public Text SkillText;
    public Image SkillBar;
}

[Serializable]
public struct ItemAmount
{
    public int[] Amount;

    public string[] Item;

    public ItemAmount(int lenght)
    {
        Amount = new int[lenght];
        Item = new string[lenght];
    }
}

[Serializable]
public struct IntArray
{
    public int[] ints;

    public int this[int i]
    {
        get { return ints[i]; }
        set { ints[i] = value; }
    }
}

[Serializable] 
public struct FloatArray
{
    public float[] floats;

    public float this[int i]
    {
        get { return floats[i]; }
        set { floats[i] = value; }
    }
}

[Serializable]
public struct MaterialArray
{
    public Material[] materials;

    public Material this[int i]
    {
        get { return materials[i];}

        set { materials[i] = value; }
    }
}

[Serializable]
public struct ResourceList
{
    public GameObject[] Resources;

    public GameObject this[int i]
    {
        get { return Resources[i]; }

        set { Resources[i] = value; }
    }
}

[Serializable]
public struct RangesArray
{
    public DamageTypeStruct[] Ranges;

    public DamageTypeStruct this[int i]
    {
        get { return Ranges[i]; }

        set { Ranges[i] = value; }
    }
}

[Serializable]
public struct QuestReward
{
    public Item[] Rewards;
    public int[] Amount;
}

[Serializable]
public struct CompletedQuest
{
    public string Name;
}

[Serializable]
public struct CreatedItem
{
    public Item Item;
    public int Amount;

    public CreatedItem(Item item, int amount)
    {
        Item = item;
        Amount = amount;
    }
}
