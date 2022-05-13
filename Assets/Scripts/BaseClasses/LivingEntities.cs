﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntities : MonoBehaviour
{
    [SerializeField] private MasteryType masteryType;

    [SerializeField] private EntityType Type;

    [SerializeField] private AttributeStruct[] Attributes = new AttributeStruct[3];

    [SerializeField] private int BaseArmour;
    [SerializeField] private int Accuracy;
    [SerializeField] private int Level;
    [SerializeField] private int BurningStacks;
    [SerializeField] protected int RunningStaminaCost;
    [Range(GlobalValues.MinRoll, GlobalValues.MaxRoll), SerializeField] protected int ThreatLevel;

    [SerializeField] private int Armour;

    [Range(-95, 95), SerializeField] private byte[] Resistences = new byte[3];

    [SerializeField] protected float RayDistance;
    [SerializeField] private float BaseSpeed;
    [SerializeField] protected float Speed;
    [SerializeField] private float ActionSpeed = 1.0f;
    [SerializeField] protected float RunStaminaDegenRate;

    [SerializeField] protected Transform RaySpawn;

    [SerializeField] private ArmourHolder[] EquipedArmour = new ArmourHolder[GlobalValues.Armourpieces];

    [SerializeField] protected Skill[] Masteries = new Skill[GlobalValues.Masteries];
    [SerializeField] protected Skill[] Skills = new Skill[GlobalValues.Skills];

    [SerializeField] protected Hand[] Hands = new Hand[2];

    [SerializeField] private List<int>[] Powers;

    [SerializeField] protected bool Dead;
    [SerializeField] protected bool Confused;
    [SerializeField] public bool Running;

    [SerializeField] private bool[] IsEmmune = new bool[4];

    [SerializeField] public Inventory Inventory;

    [SerializeField] public StatusManager StatusManger;

    [SerializeField] protected List<AIController> Minions;

    [SerializeField] private float NextRegen;

    [HideInInspector]

    [SerializeField] protected float NextStaminaDegen;

    [SerializeField] protected RaycastHit Hit;

    #region AttackFunctions
    protected IEnumerator Attack(int HandType)
    {
        if (Time.time >= Hands[HandType].NextAttack)
        {
            //if shrine is active make stats a copy of the other hands damagestats class

            Hand hand = Hands[HandType];

            hand.HasAttacked = true;

            int[] StatusChances;

            WeaponHitManager PrimaryRef;
            WeaponHolder Weapon;

            PrimaryRef = hand.HeldItem.GetComponent<WeaponHolder>().HitManagerRef;
            Weapon = hand.HeldItem.GetComponent<WeaponHolder>();

            StatusChances = new int[Weapon.StatusChance.Count];

            hand.Animator.speed = Weapon.ActionsPerSecond * ActionSpeed;

            hand.Stats.SourceHand = HandType;

            hand.Stats.LifeSteal = Weapon.LifeSteal;

            for (int i = 0; i < Weapon.StatusChance.Count; i++)
            {
                StatusChances[i] = Weapon.StatusChance[i];

                if (Powers[1].Contains(2) && i != 0)
                {
                    if (Weapon.DamageRanges[i].Type == DamageTypeEnum.Fire)
                    {
                        StatusChances[i] = 100;
                    }
                    else
                    {
                        StatusChances[i] = 0;
                    }
                }

                if (Powers[1].Contains(5) && Weapon.DamageRanges[i].Type == DamageTypeEnum.Fire)
                {
                    StatusChances[i] = 0;
                }
            }

            for (int i = 0; i < Weapon.DamageRanges.Count; i++)
            {
                hand.Stats.DamageValues.Add(CalculateMeleeDamage(Weapon, HandType, Weapon.DamageRanges[i].Type, i));
                hand.Stats.DamageTypes.Add(Weapon.DamageRanges[i].Type);

                int Chance;
                Chance = Random.Range(1, 101);

                if (Chance <= StatusChances[i] && hand.Stats.DamageValues[i] > 0)
                {
                    hand.Stats.Status.Add(true);
                }
                else
                {
                    hand.Stats.Status.Add(false);
                }
            }
            
            if (hand.ChannelTime >= .2f)
            {
                float cost = Weapon.Weight / 100f;
                cost *= 1 + (Mathf.Floor(Attributes[(int)Abilities.Strenght].Ability * .5f)) * .05f;

                if (LoseAttribute((int)cost, AttributesEnum.Stamina))
                {
                    hand.Stats.DamageValues[0] = (int)(hand.Stats.DamageValues[0] * (1 + Weapon.PwrAttackDamage * .01f));
                    hand.NextAttack = Time.time + (1f / Weapon.ActionsPerSecond * ActionSpeed * 1.25f);
                    hand.Animator.speed = Weapon.ActionsPerSecond * ActionSpeed * 1.25f;
                    hand.Animator.SetTrigger(Weapon.PwrAttackAnimationName);
                }
                else
                {
                    hand.HasAttacked = true;
                    hand.ChannelTime = 0;

                    PrimaryRef.Stats.Clear();
                    hand.Stats.Clear();

                    yield break;
                }
            }
            else
            {
                hand.NextAttack = Time.time + (1f / Weapon.ActionsPerSecond * ActionSpeed);
                hand.Animator.speed = Weapon.ActionsPerSecond * ActionSpeed;
                hand.Animator.SetTrigger(Weapon.AttackAnimationName);
            }

            hand.ChannelTime = 0;

            PrimaryRef.Stats = new DamageStats(hand.Stats);

            hand.Stats.Clear();

            yield return new WaitForSeconds(hand.Animator.speed);

            hand.Animator.speed = Weapon.ActionsPerSecond * ActionSpeed;

            PrimaryRef.Stats.Clear();
        }
    }

    protected void ChargeAttack(int HandType, bool fromEnemy = false)
    {
        //Debug.Log("Charging Attack on hand " + HandType);

        Hand hand = Hands[HandType];

        if (hand.ChannelTime >= .2f || fromEnemy)
        {
            StartCoroutine(Attack(HandType));

            return;
        }

        if (hand.ChannelTime < .2f)
        {
            hand.ChannelTime += Time.deltaTime;
        }
    }

    protected void Shoot()
    {

    }

    protected void Cast(int HandType, Spell SpellH)
    {
        float TempManaCost;
        float TempChannelTime;
        float TempNextCast;

        CastType TempCastType;
        AttributesEnum TempCostType;

        Hand hand = Hands[HandType];

        TempManaCost = SpellH.Cost;
        TempChannelTime = hand.ChannelTime;
        TempNextCast = hand.NextAttack;
        TempCastType = SpellH.CastType;
        TempCostType = SpellH.CostType;

        TempManaCost *= 1 + ((Attributes[2].Ability * GlobalValues.SPDamIntInterval) * GlobalValues.SPDamPerInt);

        switch (TempCastType)
        {
            case CastType.Channelled:
                if (TempChannelTime >= TempNextCast)
                {
                    if (SpellH is DamageSpell)
                    {
                        hand.NextAttack = 1f / (SpellH as DamageSpell).CastsPerSecond;

                        hand.ChannelTime = 0;

                        if (!LoseAttribute((int)TempManaCost, TempCostType))
                        {
                            return;
                        }

                        CreateSpell(HandType, SpellH);
                    }
                }
                else
                {
                    hand.ChannelTime += Time.deltaTime;
                }
                break;
            case CastType.Instant:
                if (Time.time >= TempNextCast)
                {
                    if (SpellH is DamageSpell)
                    {
                        hand.NextAttack = (1f / (SpellH as DamageSpell).CastsPerSecond) + Time.time;

                        if (!LoseAttribute((int)TempManaCost, TempCostType))
                        {
                            return;
                        }
                    }

                    CreateSpell(HandType, SpellH);
                }
                break;
            case CastType.Charged:
                break;
            case CastType.Touch:
                break;
            case CastType.Aura:

                if (SpellH is GolemSpell gspell)
                {
                    if (gspell.Activated)
                    {
                        return;
                    }

                    hand.NextAttack = 0;
                    hand.ChannelTime = 0;

                    if (!ReserveAttribute((int)TempManaCost, TempCostType))
                    {
                        Debug.Log("Returned from lose attribute");
                        return;
                    }
                }

                CreateSpell(HandType, SpellH);

                break;
        }
    }

    protected void Cast(int HandType, bool State, Spell SpellH)
    {
        float TempManaCost;

        Hand hand = Hands[HandType];

        TempManaCost = SpellH.Cost;

        TempManaCost *= 1 + ((Attributes[2].Ability * GlobalValues.SPDamIntInterval) * GlobalValues.SPDamPerInt);

        if ((int)TempManaCost > Attributes[2].Current)
        {
            return;
        }

        if (State == true && hand.ChannelTime >= (1 / hand.ActionsPerSecond))
        {
            CreateSpell(HandType, SpellH);
            hand.ChannelTime = 0;
            Attributes[2].Current -= (int)(TempManaCost);
        }
        else if (hand.ChannelTime < (1 / hand.ActionsPerSecond))
        {
            hand.ChannelTime += Time.deltaTime;
        }
    }
    #endregion

    #region CreateFunctions
    protected void CreateWeapon(int HandType)
    {
        Hand hand = Hands[HandType]; 

        WeaponHolder weapon = hand.HeldItem.GetComponent<WeaponHolder>();

        hand.Stats.Parent = this;

        hand.HeldItem.transform.position = hand.WeaponSpawn.position;
        hand.HeldItem.transform.parent = hand.WeaponSpawn;

        hand.HeldItem.transform.rotation = hand.WeaponSpawn.rotation;

        weapon.SpawnItemForCombat(HandType, this);

        hand.Animator.enabled = true;

        hand.Animator.runtimeAnimatorController = weapon.Animator[HandType];
        hand.Animator.speed = weapon.ActionsPerSecond * ActionSpeed;
    }

    protected void CreateSpell(int HandType, Spell SpellH)
    {
        GameObject Spell;

        Hand hand = Hands[HandType];

        if (SpellH is DamageSpell damageSpell)
        {
            Spell = Instantiate(SpellH.SpellAffect, hand.WeaponSpawn.position, hand.WeaponSpawn.rotation, hand.WeaponSpawn);

            HitManager SpellRef = Spell.GetComponent<HitManager>();

            for (int i = 0; i < damageSpell.DamageRanges.Count; i++)
            {
                hand.Stats.DamageValues.Add(CalculateSpellDamage(damageSpell, HandType, damageSpell.DamageRanges[i].Type, i));
                hand.Stats.DamageTypes.Add(damageSpell.DamageRanges[i].Type);
                
                int Chance;
                Chance = Random.Range(1, 101);

                if (Chance <= damageSpell.StatusChance[i])
                {
                    hand.Stats.Status.Add(true);
                }
                else
                {
                    hand.Stats.Status.Add(false);
                }
            }

            SpellRef.Stats = new DamageStats(hand.Stats)
            {
                Parent = this,
            };

            if (SpellRef is ChannelSpellHitManager manager)
            {
                manager.SetLifeTime(SpellH);
            }

            hand.Stats.Clear();

            Spell.SetActive(true);
        }
        else if (SpellH is GolemSpell gSpell)
        {
            for (int i = 0; i < gSpell.Number; i++)
            {
                Spell = Instantiate(SpellH.SpellAffect, hand.WeaponSpawn.position, hand.WeaponSpawn.rotation);

                Minion minion = Spell.transform.GetComponentInChildren<Minion>();

                minion.Owner = this;

                minion.SourceSpell = SpellH as GolemSpell;

                minion.SetStats(false);

                Minions.Add(minion.entity);

                gSpell.Alive++;
            }

            gSpell.Activated = true;
        }
    }
    #endregion

    #region UpDateEquipedItems
    public void EquipItem(Item Item, int HandType)
    {
        Item.GetComponent<IEquipable>().IsEquiped = true;

        Hand hand = Hands[HandType];

        switch (Item.tag)
        {
            case GlobalValues.WeaponTag:
                WeaponHolder weaponH = Item.GetComponent<WeaponHolder>();

                if (weaponH.HandType == global::HandType.TwoHanded)
                {
                    if (Hands[0].HeldItem != null && Hands[0].HeldItem != Item)
                    {
                        UnequipItem(Hands[0].HeldItem);
                    }

                    if (Hands[1].HeldItem != null && Hands[1].HeldItem != Item)
                    {
                        UnequipItem(Hands[1].HeldItem);
                    }

                    Hands[0].State = AttackType.Melee;
                    Hands[1].State = AttackType.Melee;

                    Hands[0].HeldItem = Item;
                    Hands[1].HeldItem = Item;

                    //HandType = 0;
                }
                else
                {
                    if (hand.HeldItem != null && hand.HeldItem != Item)
                    {
                        UnequipItem(hand.HeldItem);
                    }

                    hand.State = AttackType.Melee;

                    hand.HeldItem = Item;
                }

                CreateWeapon(HandType);

                for (int i = 0; i < weaponH.DamageRanges.Count; i++)
                {
                    MeleeDamageMulti(HandType, weaponH.DamageRanges[i].Type);
                }
                break;
            case GlobalValues.ArmourTag:
                ArmourHolder newArmour = Item.GetComponent<ArmourHolder>();

                if (EquipedArmour[(int)newArmour.ArmourType] != null)
                {
                    UnequipItem(EquipedArmour[(int)newArmour.ArmourType]);
                }

                EquipedArmour[(int)newArmour.ArmourType] = newArmour;

                UpdateArmour();
                InventoryUi.playerUi.SetPlayerEquipedIndicators();
                break;
            case GlobalValues.SpellTag:
                SpellHolder SpellH = Item.GetComponent<SpellHolder>();

                if (SpellH.HandType == global::HandType.TwoHanded)
                {
                    if (Hands[0].HeldItem != null && Hands[0].HeldItem != Item)
                    {
                        UnequipItem(Hands[0].HeldItem);
                    }

                    if (Hands[1].HeldItem != null && Hands[1].HeldItem != Item)
                    {
                        UnequipItem(Hands[1].HeldItem);
                    }

                    Hands[0].State = AttackType.Melee;
                    Hands[2].State = AttackType.Melee;

                    Hands[0].HeldItem = Item;
                    Hands[1].HeldItem = Item;

                    HandType = 0;
                }
                else
                {
                    if (hand.HeldItem != null && hand.HeldItem != Item)
                    {
                        UnequipItem(hand.HeldItem);
                    }

                    hand.State = AttackType.Spell;

                    hand.HeldItem = Item;
                }

                //Create Spell Object in world

                //Calculate Spell Damage multis
                for (int i = 0; i < SpellH.Spells.Length; i++)
                {
                    if (SpellH.Spells[i] is DamageSpell)
                    {
                        DamageSpell dSpell = SpellH.Spells[i] as DamageSpell;

                        for (int x = 0; x < dSpell.DamageRanges.Count; x++)
                        {
                            SpellDamageMulti(HandType, dSpell.DamageRanges[x].Type);
                        }
                    }
                }

                break;
            case GlobalValues.JewlryTag:
                break;
            case GlobalValues.TorchTag:
                hand.HeldItem = Item;

                ((TorchHolder)Item).SpawnItemForUse(hand.HandLocation);

                hand.State = AttackType.None;
                break;
            default:
                break;
        }

        if (CompareTag(GlobalValues.PlayerTag))
        {
            switch (Hands[0].State)
            {
                case AttackType.None:
                    switch (Hands[1].State)
                    {
                        case AttackType.None:
                            break;
                        case AttackType.Melee:
                            masteryType = MasteryType.OneHandedMelee;
                            break;
                        default:
                            masteryType = MasteryType.OneHandedSpell;
                            break;
                    }
                    break;
                case AttackType.Melee:
                    switch (Hands[1].State)
                    {
                        case AttackType.None:
                            masteryType = MasteryType.OneHandedMelee;
                            break;
                        case AttackType.Melee:

                            if (Hands[0].HeldItem == Hands[1].HeldItem)
                            {
                                masteryType = MasteryType.TwoHandedMelee;
                            }
                            else
                            {
                                masteryType = MasteryType.DualWieldingMelee;
                            }

                            break;
                        default:
                            masteryType = MasteryType.MeleeSpell;
                            break;
                    }
                    break;
                case AttackType.Ranged:

                    masteryType = MasteryType.Bow;

                    break;
                default:
                    switch (Hands[1].State)
                    {
                        case AttackType.None:
                            masteryType = MasteryType.OneHandedSpell;
                            break;
                        case AttackType.Melee:
                            masteryType = MasteryType.MeleeSpell;
                            break;
                        default:
                            if (Hands[0].HeldItem == Hands[1].HeldItem)
                            {
                                masteryType = MasteryType.TwoHandedSpell;
                            }
                            else
                            {
                                masteryType = MasteryType.DualWieldingSpell;
                            }
                            break;
                    }
                    break;
            }
        }
    }

    public void UnequipItem(Item Item)
    {
        Item.GetComponent<IEquipable>().IsEquiped = false;

        Item.GetComponent<Item>().StoreItem();

        switch (Item.tag)
        {
            case GlobalValues.WeaponTag:
                if (Hands[0].HeldItem == Item)
                {
                    if (Hands[0].HeldItem == Hands[1].HeldItem)
                    {
                        Hands[1].Animator.enabled = false;
                        Hands[1].HeldItem = null;
                    }

                    Hands[0].Animator.enabled = false;
                    Hands[0].HeldItem = null;
                }
                else if (Hands[1].HeldItem == Item)
                {
                    Hands[1].Animator.enabled = false;
                    Hands[1].HeldItem = null;
                }
                break;
            case GlobalValues.ArmourTag:
                EquipedArmour[(int)Item.GetComponent<ArmourHolder>().ArmourType] = null;
                UpdateArmour();
                break;
            case GlobalValues.SpellTag:
                if (Hands[0].HeldItem == Item)
                {
                    Hands[0].HeldItem = null;
                }
                else if (Hands[1].HeldItem == Item)
                {
                    Hands[1].HeldItem = null;
                }

                SpellHolder SpellH = Item.GetComponent<SpellHolder>();

                int lenght = SpellH.Spells.Length;

                for (int i = 0; i < lenght; i++)
                {
                    if (SpellH.Spells[i] != null && SpellH.Spells[i] is GolemSpell)
                    {
                        GolemSpell golemSpell = SpellH.Spells[i] as GolemSpell;

                        int id = 0;
                        int count = Minions.Count;

                        for (int x = 0; x < count; x++)
                        {
                            if ((Minions[id].Controller as Minion).SourceSpell == golemSpell)
                            {
                                (Minions[id].Controller as Minion).CallDeath(false);
                                Minions.RemoveAt(id);
                            }
                            else
                            {
                                id++;
                            }
                        }

                        golemSpell.Activated = false;
                    }
                }

                break;
            case GlobalValues.JewlryTag:
                break;
            case GlobalValues.TorchTag:
                for (int i = 0; i < 2; i++)
                {
                    if (Hands[i].HeldItem == Item)
                    {
                        Destroy(Hands[i].HandLocation.GetChild(1).gameObject);
                        Hands[i].HeldItem = null;
                        break;
                    }
                }
                break;
        }

        Item.transform.parent = Inventory.InventroyHolder;
    }

    protected void UpdateArmour()
    {
        Armour = BaseArmour;

        for (int i = 0; i < EquipedArmour.Length; i++)
        {
            if (EquipedArmour[i] != null)
            {
                int tempArmour = EquipedArmour[i].Armour * (1 + ((int)EquipedArmour[i].SkillType) / 100);

                Armour += tempArmour;
            }
        }
    }
    #endregion

    #region DamageMultis
    protected void MeleeDamageMulti(int HandType, DamageTypeEnum Type)
    {
        float PercentFromLevel = 0;

        switch (Type)
        {
            case DamageTypeEnum.Physical:
                float StrenghtMeleeMulti;

                StrenghtMeleeMulti = (Mathf.Floor(Attributes[0].Ability * .5f)) * .05f;

                PercentFromLevel = (float)Skills[(int)Hands[HandType].HeldItem.GetComponent<WeaponHolder>().SkillType].Level * .01f;

                Hands[HandType].DamageMultis.Melee[(int)Type] = 1;

                Hands[HandType].DamageMultis.Melee[(int)Type] += StrenghtMeleeMulti + PercentFromLevel;
                break;
            case DamageTypeEnum.Fire:
                PercentFromLevel = (float)Skills[(int)SkillType.Pyromancy].Level * .01f;
                break;
            case DamageTypeEnum.Lightning:
                PercentFromLevel = (float)Skills[(int)SkillType.Astromancy].Level * .01f;
                break;
            case DamageTypeEnum.Ice:
                PercentFromLevel = (float)Skills[(int)SkillType.Cryomancy].Level * .01f;
                break;
            case DamageTypeEnum.Soul:
                PercentFromLevel = (float)Skills[(int)SkillType.Syromancy].Level * .01f;
                break;
        }

        if (Type != DamageTypeEnum.Physical)
        {

            Hands[HandType].DamageMultis.Melee[(int)Type] = 1;

            Hands[HandType].DamageMultis.Melee[(int)Type] += PercentFromLevel;

        }
    }

    protected void SpellDamageMulti(int HandType, DamageTypeEnum Type)
    {
        float PercentFromLevel = 0;
        float PercentFromInt;

        Hand hand = Hands[HandType];

        PercentFromInt = (Mathf.Floor(Attributes[2].Ability * GlobalValues.SPDamIntInterval)) * GlobalValues.SPDamPerInt;

        switch (Type)
        {
            case DamageTypeEnum.Physical:
                PercentFromLevel = (float)Skills[(int)SkillType.Geomancy].Level * .01f;
                break;
            case DamageTypeEnum.Fire:
                PercentFromLevel = (float)Skills[(int)SkillType.Pyromancy].Level * .01f;
                break;
            case DamageTypeEnum.Lightning:
                PercentFromLevel = (float)Skills[(int)SkillType.Astromancy].Level * .01f;
                break;
            case DamageTypeEnum.Ice:
                PercentFromLevel = (float)Skills[(int)SkillType.Cryomancy].Level * .01f;
                break;
            default:
                PercentFromLevel = (float)Skills[(int)SkillType.Syromancy].Level * .01f;
                break;
        }

        hand.DamageMultis.Spell[(int)Type] = 1;
        hand.DamageMultis.Spell[(int)Type] += PercentFromInt + PercentFromLevel;
    }
    #endregion

    #region CalculateDamage
    private int CalculateMeleeDamage(WeaponHolder Weapon, int HandType, DamageTypeEnum DamageType, int id)
    {
        float TempDamage;

        TempDamage = Random.Range(Weapon.DamageRanges[id].LDamage, Weapon.DamageRanges[id].HDamage + 1);

        TempDamage *= Hands[HandType].DamageMultis.Melee[(int)DamageType];

        switch (DamageType)
        {
            case DamageTypeEnum.Physical:
                int CriticalChance = Random.Range(0, 101);

                if (CriticalChance <= Weapon.StatusChance[0])
                {
                    TempDamage *= 1 + (Weapon.CritDamage * .01f);
                    //Debug.Log("Critical " + TempDamage);
                }
                break;
            case DamageTypeEnum.Fire:
                if (Powers[1].Contains(1))
                {
                    //Descreased Fire Damage with no burning damage penalty
                    TempDamage *= 0.25f;
                }

                if (Powers[1].Contains(0))
                {
                    //checks which burning mulit to use with reflect burning to self
                    if (Powers[1].Contains(4) && BurningStacks > 1)
                    {
                        //More Fire Damage Per Stack on self 
                        //craeted from having burning can stack and reflect burning to self
                        TempDamage *= 1 + (.5f * BurningStacks);
                    }
                    else
                    {
                        //Refelcted Shrine Effect Damage Multi without burning can stack shrine
                        TempDamage *= 1.5f;
                    }
                }
                break;
            case DamageTypeEnum.Lightning:
                break;
            case DamageTypeEnum.Ice:
                break;
            case DamageTypeEnum.Soul:
                break;
        }

        if (Weapon.MaxDurability != 0)
        {
            TempDamage *= (float)Weapon.CurrentDurability / (float)Weapon.MaxDurability;
        }

        return (int)TempDamage;
    }

    private int CalculateSpellDamage(DamageSpell Spell, int HandType, DamageTypeEnum DamageType, int id)
    {
        float TempDamage;

        TempDamage = Random.Range(Spell.DamageRanges[id].LDamage, Spell.DamageRanges[id].HDamage + 1);

        TempDamage *= Hands[HandType].DamageMultis.Spell[(int)DamageType];

        switch (DamageType)
        {
            case DamageTypeEnum.Physical:
                break;
            case DamageTypeEnum.Fire:
                break;
            case DamageTypeEnum.Lightning:
                break;
            case DamageTypeEnum.Ice:
                break;
            case DamageTypeEnum.Soul:
                break;
        }

        return (int)TempDamage;
    }
    #endregion

    #region UpdateAttributes
    public virtual int TakeDamage(DamageStats stats)
    {
        if (Attributes[(int)AttributesEnum.Health].Current <= 0)
        {
            return 0;
        }

        int TotalDamage = 0;

        int damageType;

        for (int i = 0; i < stats.DamageValues.Count; i++)
        {
            damageType = (int)stats.DamageTypes[i];

            if (stats.DamageValues[i] == 0 || (i < 4 && IsEmmune[damageType]))
            {
                continue;
            }

            if (i == 0)
            {
                stats.DamageValues[i] -= Armour;

                if (stats.DamageValues[i] < 0)
                {
                    stats.DamageValues[i] = 0;
                }
            }
            else if (i < 4)
            {
                float ProtectionValue = Resistences[i - 1] * .01f;
                stats.DamageValues[i] = (int)(stats.DamageValues[i] - (stats.DamageValues[i] * ProtectionValue));
            }

            if (CompareTag(GlobalValues.EnemyTag) && stats.Parent == Player.player)
            {
                SkillType skill;

                switch (damageType)
                {
                    case 0:
                        skill = Player.player.Hands[stats.SourceHand].HeldItem.GetComponent<IEquipable>().SkillType;
                        break;
                    case 1:
                        skill = SkillType.Pyromancy;
                        break;
                    case 2:
                        skill = SkillType.Astromancy;
                        break;
                    case 3:
                        skill = SkillType.Cryomancy;
                        break;
                    default:
                        skill = SkillType.Syromancy;
                        break;
                }

                if (stats.DamageValues[i] > Attributes[0].Current)
                {
                    Player.player.GainExp(Attributes[0].Current, (int)skill);
                }
                else
                {
                    Player.player.GainExp(stats.DamageValues[i], (int)skill);
                }
            }
            else if (CompareTag(GlobalValues.PlayerTag))
            {
                //Logic for gaining heavy/light armour exp
                for (int x = 0; x < Player.player.EquipedArmour.Length; x++)
                {
                    if (Player.player.EquipedArmour[x] != null)
                    {
                        long Exp;

                        ArmourHolder ArmourH = Player.player.EquipedArmour[x].GetComponent<ArmourHolder>();

                        if (i == 0)
                        {
                            long tempArmour = (long)(ArmourH.Armour * (1 + ((double)Player.player.Skills[(int)ArmourH.SkillType].Level / 100)));
                            Exp = tempArmour;
                        }
                        else
                        {
                            int Id = i - 1;

                            if (ArmourH.Resistences[Id] == 0)
                            {
                                continue;
                            }

                            Exp = (long)(stats.DamageValues[i] * ((double)ArmourH.Resistences[Id] / Player.player.Resistences[Id]));
                        }

                        Player.player.GainExp(Exp, (int)ArmourH.SkillType);
                    }
                }
            }

            TotalDamage += stats.DamageValues[i];
        }

        LoseAttribute(TotalDamage, AttributesEnum.Health);

        return TotalDamage;
    }

    public virtual bool GainAttribute(int Amount, AttributesEnum attribute)
    {
        if (Dead || Attributes[(int)attribute].Current == Attributes[(int)attribute].Max - Attributes[(int)attribute].Reserved)
        {
            return false;
        }

        if (Attributes[(int)attribute].Current + Amount <= Attributes[(int)attribute].Max - Attributes[(int)attribute].Reserved)
        {
            Attributes[(int)attribute].Current += Amount;
        }
        else
        {
            Attributes[(int)attribute].Current = Attributes[(int)attribute].Max - Attributes[(int)attribute].Reserved;
        }

        return true;
    }

    public virtual bool UnReserveAttribute(int Amount, AttributesEnum attribute)
    {
        Attributes[(int)attribute].Reserved -= Amount;

        if (GainAttribute(Amount, attribute))
        {
            return true;
        }

        return false;
    }

    public virtual bool LoseAttribute(int amount, AttributesEnum attribute)
    {
        if (Attributes[(int)attribute].Current >= amount || attribute == AttributesEnum.Health)
        {
            Attributes[(int)attribute].Current -= amount;
            return true;
        }

        return false;
    }

    public virtual bool ReserveAttribute(int amount, AttributesEnum attribute)
    {
        if (LoseAttribute(amount, attribute))
        {
            Attributes[(int)attribute].Reserved += amount;

            return true;
        }

        return false;
    }
    #endregion

    #region BaseCalcs
    public virtual void CalculateSpeed()
    {
        float tempSpeed = BaseSpeed * ActionSpeed;

        if (Inventory.CurrentCarryWeight >= Inventory.MaxCarryWeight)
        {
            float WeightMulti = Inventory.CurrentCarryWeight - Inventory.MaxCarryWeight;

            WeightMulti *= .01f;
            WeightMulti *= .05f;

            tempSpeed *= WeightMulti;
        }

        Speed = tempSpeed;
    }

    protected virtual void CalculateAttribute(int attribute)
    {
        float TempAttribute;

        //added attribute per level
        float multi1 = 0;
        //percent attribute per level
        float multi2 = 0;

        float AbilityMulti;
        float AbilityPercent;

        switch (attribute)
        {
            //Health
            case 0:
                multi1 = 3.0f;
                multi2 = .06f;
                break;
            //Stamina
            case 1:
                multi1 = 8.0f;
                multi2 = .09f;
                break;
            //Mana
            case 2:
                multi1 = 12.0f;
                multi2 = .12f;
                break;
            default:
                break;
        }

        AbilityMulti = Mathf.Floor(Attributes[attribute].Ability * .2f) * multi1;
        AbilityPercent = Mathf.Floor(Attributes[attribute].Ability * .14f) * multi2;

        TempAttribute = ((Level * Attributes[attribute].APerLevel) + AbilityMulti) *
            (1 + ((Level * Attributes[attribute].APercentPerLevel) + AbilityPercent));

        Attributes[attribute].Max = (long)TempAttribute;
        Attributes[attribute].Current = Attributes[attribute].Max;
    }

    protected virtual void CalculateWeight()
    {
        int tempWeight = 14500;

        int strenghtMulti = ((int)Mathf.Floor(Attributes[0].Ability / 10f)) * 500;

        tempWeight += strenghtMulti;

        Inventory.MaxCarryWeight = tempWeight;
    }
    #endregion

    public virtual void CheckHealth()
    {
        
    }

    protected void ResetPowers()
    {
        Powers = new List<int>[6];

        for (int i = 0; i < Powers.Length; i++)
        {
            Powers[i] = new List<int>();
        }
    }

    public void LoadEntity(LivingEntitiesData Data)
    {
        InventoryData iData = Data.inventoryData;
        
        Level = Data.Level;

        for (int i = 0; i < 3; i++)
        {
            Attributes[i].Current = Data.Attributes[i].Current;
            Attributes[i].Max = Data.Attributes[i].Max;
            Attributes[i].Reserved = Data.Attributes[i].Reserved;
        }

        if (Inventory.AllItems.Count != 0)
        {
            int Count = Inventory.AllItems.Count;

            for (int i = 0; i < Count; i++)
            {
                Destroy(Inventory.AllItems[i]);
            }

            Inventory.AllItems.Clear();
            Inventory.StartIds = new int[GlobalValues.MiscStart + 1];
        }

        if (iData.NumOfWeapons > 0)
        {
            for (int i = 0; i < iData.NumOfWeapons; i++)
            {
                WeaponHolder weapon = Instantiate(PrefabIDs.prefabIDs.WeaponHolder, Inventory.InventroyHolder).GetComponent<WeaponHolder>();

                LoadSystem.LoadItem(iData.Weapons[i], weapon);

                Inventory.AddItem(weapon, false, iData.Weapons[i].Amount);
            }

            if (Data.CurrentMainHandID == 1)
            {
                EquipItem(Inventory.AllItems[Data.CurrentWeaponID], 0);
            }

            if (Data.CurrentOffHandID == 1)
            {

                EquipItem(Inventory.AllItems[Data.CurrentOffWeaponID], 1);
            }
        }

        if (iData.NumOfArmour > 0)
        {
            for (int i = 0; i < iData.NumOfArmour; i++)
            {
                ArmourHolder armour = Instantiate(PrefabIDs.prefabIDs.ArmourHolder, Inventory.InventroyHolder).GetComponent<ArmourHolder>();

                LoadSystem.LoadItem(iData.Armour[i], armour);

                Inventory.AddItem(armour, false, iData.Armour[i].Amount);

                if (iData.Armour[i].IsEquiped)
                {
                    EquipItem(armour, 0);
                }
            }
        }

        if (iData.NumOfSpells > 0)
        {
            for (int i = 0; i < iData.NumOfSpells; i++)
            {
                SpellHolder spell = Instantiate(PrefabIDs.prefabIDs.SpellHolder, Inventory.InventroyHolder).GetComponent<SpellHolder>();

                LoadSystem.LoadItem(iData.Spells[i], spell);

                Inventory.AddItem(spell, false, iData.Spells[i].Amount);
            }

            if (Data.CurrentMainHandID == 2)
            {
                EquipItem(Inventory.AllItems[Data.CurrentWeaponID], 0);
            }

            if (Data.CurrentOffHandID == 2)
            {
                EquipItem(Inventory.AllItems[Data.CurrentOffWeaponID], 1);
            }
        }

        if (iData.NumOfRunes > 0)
        {
            for (int i = 0; i < iData.NumOfRunes; i++)
            {
                RuneHolder rune = Instantiate(PrefabIDs.prefabIDs.RuneHolder, Inventory.InventroyHolder).GetComponent<RuneHolder>(); 

                LoadSystem.LoadItem(iData.Runes[i], rune);

                Inventory.AddItem(rune, false, iData.Runes[i].Amount);
            }
        }

        if (iData.NumOfPotions > 0)
        {
            for (int i = 0; i < iData.NumOfPotions; i++)
            {
                Consumable potion = Instantiate(PrefabIDs.prefabIDs.Potions[iData.Potions[i].ResourceId], Inventory.InventroyHolder).GetComponent<Consumable>();
                potion.Amount = iData.Potions[i].Amount;

                Inventory.AddItem(potion, false, iData.Potions[i].Amount);
            }
        }

        if (iData.NumOfResources > 0)
        {
            for (int i = 0; i < iData.NumOfResources; i++)
            {
                ResourceHolder HResource = Instantiate(PrefabIDs.prefabIDs.CraftingMaterials[iData.Resources[i].ResourceId], Inventory.InventroyHolder).GetComponent<ResourceHolder>();
                HResource.Amount = iData.Resources[i].Amount;

                Inventory.AddItem(HResource, false, iData.Resources[i].Amount);
            }
        }

        if (iData.NumOfMisc > 0)
        {
            for (int i = 0; i < iData.NumOfMisc; i++)
            {
                Item misc = Instantiate(PrefabIDs.prefabIDs.Items[iData.Misc[i].ResourceId], Inventory.InventroyHolder).GetComponent<Item>();
                misc.Amount = iData.Misc[i].Amount;

                Inventory.AddItem(misc, false, iData.Misc[i].Amount);
            }
        }
    }

    #region Getters
    public new EntityType GetType()
    {
        return Type;
    }

    public MasteryType GetMastery()
    {
        return masteryType;
    }

    public long GetCurrentHealth()
    {
        return Attributes[0].Current;
    }

    public long GetCurrentStamina()
    {
        return Attributes[1].Current;
    }

    public long GetCurrentMana()
    {
        return Attributes[2].Current;
    }

    public long GetMaxHealth()
    {
        return Attributes[0].Max;
    }

    public long GetMaxStamina()
    {
        return Attributes[1].Max;
    }

    public long GetMaxMana()
    {
        return Attributes[2].Max;
    }

    public long GetResrvedAttribute(int id)
    {
        return Attributes[id].Reserved;
    }

    public long GetCurrentAttribute(int id)
    {
        return Attributes[id].Current;
    }

    public long GetMaxAttribute(int id)
    {
        return Attributes[id].Max;
    }

    public int GetStrength()
    {
        return Attributes[0].Ability;
    }

    public int GetDexterity()
    {
        return Attributes[1].Ability;
    }

    public int GetIntelligence()
    {
        return Attributes[2].Ability;
    }

    public int GetAbility(int id)
    {
        return Attributes[id].Ability;
    }

    public int GetAccuracy()
    {
        return Accuracy;
    }

    public int GetLevel()
    {
        return Level;
    }

    public int GetThreatLevel()
    {
        return ThreatLevel;
    }

    public int GetArmour()
    {
        return Armour;
    }

    public int GetFireResistence()
    {
        return Resistences[0];
    }

    public int GetLightningResistence()
    {
        return Resistences[1];
    }

    public int GetIceResistence()
    {
        return Resistences[2];
    }

    public int GetResistnce(int id)
    {
        return Resistences[id];
    }

    public bool GetDead()
    {
        return Dead;
    }

    public ArmourHolder GetEquipedArmour(int id)
    {
        return EquipedArmour[id];
    }

    public int GetMasteryLevel(int id)
    {
        return Masteries[id].Level;
    }

    public ulong GetMasteryExp(int id)
    {
        return Masteries[id].Exp;
    }

    public ulong GetMasteryRExp(int id)
    {
        return Masteries[id].RExp;
    }

    public int GetSkillLevel(int id)
    {
        return Skills[id].Level;
    }

    public ulong GetSkillExp(int id)
    {
        return Skills[id].Exp;
    }

    public ulong GetSkillRExp(int id)
    {
        return Skills[id].RExp;
    }

    public Item GetHeldItem(int handType)
    {
        return Hands[handType].HeldItem;
    }

    public Vector3 GetHandPosition(int handType)
    {
        return Hands[handType].HandLocation.position;
    }

    public DamageStats GetStats(int handType)
    {
        return Hands[handType].Stats;
    }

    public float[] GetMeleeMultis(int handType)
    {
        return Hands[handType].DamageMultis.Melee;
    }

    public float[] GetRangedMultis(int handType)
    {
        return Hands[handType].DamageMultis.Ranged;
    }

    public float[] GetSpellMultis(int handType)
    {
        return Hands[handType].DamageMultis.Spell;
    }

    public GameObject GetHit()
    {
        return Hit.collider.gameObject;
    }

    public float GetActionSpeed()
    {
        return ActionSpeed;
    }

    public bool GetPower(int type, int id)
    {
        return Powers[type].Contains(0);
    }
    #endregion

    #region Setters
    protected void SetCurrentAttribute(int id, long amount)
    {
        Attributes[id].Current = amount;
    }

    protected void SetMaxAttribute(int id, long amount)
    {
        Attributes[id].Max = amount;
    }

    protected void SetStrength(int amount)
    {
        Attributes[0].Ability = amount;
    }

    protected void SetDexterity(int amount)
    {
        Attributes[1].Ability = amount;
    }

    protected void SetIntelligence(int amount)
    {
        Attributes[2].Ability = amount;
    }

    protected void SetAbility(int id, int amount)
    {
        Attributes[id].Ability = amount;
    }

    public void SetLevel(int level)
    {
        Level = level;
    }

    public void ReduceActionSpeed(float reduction)
    {
        ActionSpeed -= reduction;

        CalculateSpeed();
    }

    public void SetActionSpeedDefault()
    {
        ActionSpeed = 1;

        CalculateSpeed();
    }
    #endregion
}
