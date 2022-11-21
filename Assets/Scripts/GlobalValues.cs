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
    public const string ShieldTag  = "Shield";
    public const string SpellTag = "Spell";
    public const string RuneTag = "Rune";
    public const string PotionTag = "Potion";
    public const string ResourceTag = "Resource";
    public const string JewlryTag = "Jewlry";
    public const string KeyTag = "Key";
    public const string GoldTag = "Gold";
    public const string MiscTag = "Misc";
    public const string TorchTag = "Torch";

    public const string LevelText = "Level";
    public const string RightText = "Right";
    public const string LeftText = "Left";
    public const string HandText = "Hand";
    public const string SkillsText = "Skills";
    public const string MasteriesText = "Masteries";
    public const string AbilitiesText = "Abilities";
    public const string DefensesText = "Defenses";
    public const string ArmourText = "Armour";
    public const string ResistanceText = "Resistance";
    public const string FireText = "Fire";
    public const string LightningText = "Lightning";
    public const string IceText = "Ice";
    public const string OffensesText = "Offenses";
    public const string AttackText = "Attack";
    public const string PerText = "Per";
    public const string SecondText = "Second";
    public const string LifeText = "Life";
    public const string StealText = "Steal";
    public const string MeleeText = "Melee";
    public const string ManaText = "Mana";
    public const string CostText = "Cost";
    public const string CastText = "Case";
    public const string TypeText = "Type";
    public const string RateText = "Rate";
    public const string MultipliersText = "Multipliers";
    public const string CritText = "Critical";
    public const string BurnText = "Burn";
    public const string ChainText = "Chain";
    public const string ChillText = "Chill";
    public const string ChanceText = "Chance";
    public const string DamageText = "Damage";
    public const string TicksText = "Ticks";
    public const string TimeText = "Time";
    public const string BetweenText = "Between";
    public const string LengthText = "Length";
    public const string AffectText = "Affect";
    public const string DurationText = "Duration";
    public const string ReducedText = "Reduced";
    public const string ActionText = "Action";
    public const string SpeedText = "Speed";
    public const string NumberText = "Number";
    public const string PowerText = "Power";
    public const string EnterText = "Enter";
    public const string OpenText = "Open";
    public const string TalkText = "Talk";
    public const string LockedText = "(Locked)";
    public const string EmptyText = "(Empty)";
    public const string UseText = "Use";
    public const string BreakLineSmall = "\n_____________________________________";
    public const string BreakLineMid =   "\n___________________________________________";
    public const string BreakLineLarge = "\n___________________________________________________";
    public const string BreakLineExLarge = "\n________________________________________________________";
    public const string ToText= " to ";
    public const string OfText= " of ";
    public const string SlotText= "Slot";
    public const string InterationKey = "E";

    public static readonly string[] AttackInputs = { "Fire1", "Fire2", "1", "2", "3", "4" };

    public const int MinRoll = 1;
    public const int MaxRoll = 1001;
    public const int Masteries = 15;
    public const int Skills = 17;
    public const int Armourpieces = 9;
    public const int LevelCap = 100;
    public const int APointsPerLevel = 10;
    public const int ArmourStart = 0;
    public const int SpellStart = 1;
    public const int RuneStart = 2;
    public const int PotionStart = 3;
    public const int ResourceStart = 4;
    public const int MiscStart = 5;


    public const float MDamStrInterval = 0.5f;
    public const float MDamPerStr = 0.05f;
    public const float RDamDexInterval = 0.50f;
    public const float RDamPerDex = 0.045f;
    public const float SPDamIntInterval = 0.50f;
    public const float SPDamPerInt = 0.055f;
    public const float ChargeAttackTime = 0.3f;

    public static readonly Color[] rarities = {new Color(0.72f, 0.72f, 0.72f, 0.50f), //base color
                                               new Color(0.50f, 0.50f, 0.50f, 0.50f), //common color
                                               new Color(0.00f, 0.36f, 1.00f, 0.50f), //uncommon color
                                               new Color(0.00f, 1.00f, 0.36f, 0.50f), //magic color
                                               new Color(0.94f, 0.80f, 0.00f, 0.50f), //rare color
                                               new Color(1.00f, 0.25f, 0.25f, 0.50f)}; //legendary color

}
