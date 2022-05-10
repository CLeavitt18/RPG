using UnityEngine;

public static class GlobalValues
{
    public const string PlayerFolder = "/Player";
    public const string LevelFolder = "/Levels/";
    public const string NPCFolder = "/NPC";
    public const string EnemyFolder = "/Enemy";
    public const string MinionFolder = "/Minion";
    public const string ContainerFolder = "/Container";
    public const string DoorFolder = "/Door";
    public const string DepositFolder = "/Deposit";
    public const string SaveExtension = ".save";
    public const string TempExtension = ".temp";
    public const string PlayerTag = "Player";
    public const string EnemyTag = "Enemy";
    public const string NPCTag = "NPC";
    public const string MinionTag = "Minion";
    public const string ContainerTag = "Container";
    public const string ResourceDepositTag = "ResourceDeposit";
    public const string DoorTag = "Door";
    public const string WeaponTag = "Weapon";
    public const string ArmourTag = "Armour";
    public const string SpellTag = "Spell";
    public const string RuneTag = "Rune";
    public const string PotionTag = "Potion";
    public const string ResourceTag = "Resource";
    public const string JewlryTag = "Jewlry";
    public const string KeyTag = "Key";
    public const string GoldTag = "Gold";
    public const string MiscTag = "Misc";

    public static readonly string[] AttackInputs = { "Fire1", "Fire2", "1", "2", "3", "4" };

    public const int MinRoll = 1;
    public const int MaxRoll = 1001;
    public const int LevelProgressMax = 7;
    public const int Masteries = 10;
    public const int Skills = 17;
    public const int Armourpieces = 9;
    public const int LevelCap = 100;
    public const int APointsPerLevel = 10;

    public const float MDamStrInterval = 0.5f;
    public const float MDamPerStr = 0.05f;
    public const float RDamDexInterval = 0.50f;
    public const float RDamPerDex = 0.045f;
    public const float SPDamIntInterval = 0.50f;
    public const float SPDamPerInt = 0.055f;

    public static readonly Color baseRarity = new Color(0.50f, 0.50f, 0.50f, 1.00f);
    public static readonly Color common = new Color(0.72f, 0.72f, 0.72f, 1.00f);
    public static readonly Color uncommon = new Color(0.50f, 0.36f, 1.00f, 0.50f);
    public static readonly Color magic = new Color(0.09f, 0.87f, 0.05f, 0.50f);
    public static readonly Color rare = new Color(0.99f, 0.92f, 0.00f, 0.50f);
    public static readonly Color legendary = new Color(0.89f, 0.22f, 0.5f, 0.5f);
}
