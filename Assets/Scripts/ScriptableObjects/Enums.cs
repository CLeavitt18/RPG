public enum StatusTypeEnum : byte
{
    BaseType,
    AdvancedBase,
    SecondType,
    AdvancedSecondType,
    ThirdType,
    AdvacnedThirdType
}

public enum EntityType : byte
{
    Player,
    Enemy,
    NPC,
    Minion
}

public enum MasteryType : byte
{
    OneHandedMelee,
    OneHandedRanged,
    OneHandedSpell,
    MeleeShield,
    RangedShield,
    SpellShield,
    MeleeRanged,
    MeleeSpell,
    RangedSpell,
    DualWieldingMelee,
    DualWieldingRanged,
    DualWieldingSpell,
    TwoHandedMelee,
    TwoHandedRanged,
    TwoHandedSpell,
    None
}

public enum SkillType : byte
{
    Blade,
    Blunt,
    Archery,
    Geomancy,
    Pyromancy,
    Astromancy,
    Cryomancy,
    Syromancy,
    LightArmour,
    MediumArmour,
    HeavyArmour,
    Blocking,
    Smithing,
    Enchanting,
    SpellCrafting,
    Brewing,
    Cooking,
    None
}

public enum AttackType : byte
{
    None,
    Melee,
    Ranged,
    Spell,
    Shield
}

public enum WeaponType : byte
{
    Sword,
    Axe,
    Dagger,
    GreatSword
}

public enum HandType : byte
{
    OneHanded,
    TwoHanded
}

public enum CastType : byte
{
    Channelled,
    Instant,
    Charged,
    Touch,
    Aura
}

public enum CastTarget : byte
{
    Other,
    Self
}

public enum SpellType : byte
{
    DamageSpell,
    GolemSpell,
    None
}

public enum ArmourType : byte
{
    Helmet,
    RightPauldron,
    LeftPauldron,
    ChestPlate,
    RightGauntlet,
    LeftGauntlet,
    Leggings,
    RightBoot,
    LeftBoot,
    Shield
}

public enum RingType : byte
{
    RightThumb,
    LeftThumb,
    RightPointer,
    LeftPointer,
    RightMiddle,
    LeftMiddle,
    RightRing,
    LeftRing,
    RightPinkie,
    LeftPinkie
}

public enum JewlryType : byte
{
    Ring,
    Amulet,
    Belt
}

public enum MaterialType : byte
{
    Bone,
    Wood,
    Tin,
    Copper,
    Bronze,
    Iron,
    Sliver,
    Steel,
    Cobalt,
    Mithril,
    Ebony
}

public enum CatType : byte
{
    T1Phys,
    T2Phys,
    T3Phys,
    T4Phys,
    T5Phys,
    T6Phys,
    T1Fire,
    T2Fire,
    T3Fire,
    T4Fire,
    T5Fire,
    T6Fire,
    T1Lightning,
    T2Lightning,
    T3Lightning,
    T4Lightning,
    T5Lightning,
    T6Lightning,
    T1Ice,
    T2Ice,
    T3Ice,
    T4Ice,
    T5Ice,
    T6Ice,
}

public enum DamageTypeEnum : byte
{
    Physical,
    Fire,
    Lightning,
    Ice,
    Soul
}

public enum Behaviuor : byte
{
    Hostile,
    Passive,
    Neutrel,
    Aggressive,
    Protective
}

public enum AttributesEnum : byte
{
    Health,
    Stamina,
    Mana
}

public enum Abilities : byte
{
    Strenght,
    Dexterity,
    Intelligence
}

public enum PlayerState : byte
{
    Loading,
    Active,
    InInventoy,
    InJournal,
    InStats,
    InContainer,
    InConvoe,
    InStore,
    Paused,
    Sleeping
}

public enum NPCChatState : byte
{
    None,
    Quest,
    TurnInQuest,
    Store,
    Training
}

public enum QuestCompleteType : byte
{
    Item,
    Boss
}

public enum UiType : byte
{
    Stats,
    Quests,
    Inventory,
    Sleep,
    LevelUp
}

public enum UiState : byte
{
    Container,
    Store,
    Player,
    Entity
}

public enum InventoryState : byte
{
    Weapons,
    Armour,
    Spells,
    Runes,
    Potions,
    Resources,
    Misc,
    AllItems,
    Quest
}

public enum PauseUiState : byte
{
    Paused,
    Saving,
    Loading
}

public enum SlotState : byte
{
    Item,
    Quest
}
