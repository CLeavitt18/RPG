using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : Item, IEquipable
{
    public SkillType SkillType { get { return _skillType; } set { _skillType = value; } }

    public HandType HandType { get { return _handType; } set { _handType = value; } }

    public int LifeSteal;
    public int PwrAttackDamage;
    public int CurrentDurability;
    public int MaxDurability;

    public int CritDamage { get { return _critDamage; } set { _critDamage = value; } }

    public List<int> StatusChance { get { return _statusChance; }set { _statusChance = value; } }
    
    public float ActionsPerSecond { get { return _actionsPerSecound; } set { _actionsPerSecound = value; } }

    public bool IsEquiped { get { return _isEquiped; } set { _isEquiped = value; } }
    
    public List<DamageTypeStruct> DamageRanges { get { return _damageRanges; } set { _damageRanges = value; } }


    [SerializeField] private SkillType _skillType;
    [SerializeField] private HandType _handType;

    [SerializeField] private int _critDamage;

    [SerializeField] private List<int> _statusChance;
    
    [SerializeField] private float _actionsPerSecound;

    [SerializeField] private bool _isEquiped;

    [SerializeField] private WeaponSpawns WeaponSpawn;

    [SerializeField] private WeaponSpawns[] Spawns;

    [SerializeField] private List<DamageTypeStruct> _damageRanges;

    public GameObject Primary;
    public GameObject Secoundary;
    public GameObject Teritiary;

    public Material Material;

    public MaterialType[] Materials = new MaterialType[3];

    public WeaponHitManager HitManagerRef;
    
    public WeaponType Type;

    public RuntimeAnimatorController[] Animator = new RuntimeAnimatorController[2];

    public string AttackAnimationName;
    public string PwrAttackAnimationName;


    public override void SpawnItem()
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;

        transform.parent = null;

        transform.position = Player.player.GetItemSpawn();

        Instantiate(Primary, WeaponSpawn.PrimarySpawn);
        Instantiate(Secoundary, WeaponSpawn.SecoundarySpawn);
        Instantiate(Teritiary, WeaponSpawn.TeritiarySpawn);

        gameObject.GetComponent<ItemInteractiable>().enabled = true;
    }

    public void SpawnItemForCombat(int HandType, LivingEntities parent)
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;

        GameObject Primary_GO = Instantiate(Primary, WeaponSpawn.PrimarySpawn);
        GameObject Secoundary_GO = Instantiate(Secoundary, WeaponSpawn.SecoundarySpawn);
        GameObject Teritiary_GO = Instantiate(Teritiary, WeaponSpawn.TeritiarySpawn);

        Primary_GO.transform.GetChild(0).GetComponent<MeshRenderer>().material = Material;

        HitManagerRef = WeaponSpawn.PrimarySpawn.GetChild(0).GetChild(0).gameObject.GetComponent<WeaponHitManager>();

        HitManagerRef.Stats.Parent = parent;

        Vector3 StartingPosition = Teritiary_GO.transform.GetChild(1).position;
        Vector3 HandPosition = HitManagerRef.Stats.Parent.GetHandPosition(HandType);

        Vector3 OffSet = (HandPosition - StartingPosition);

        Primary_GO.transform.position += OffSet;
        Secoundary_GO.transform.position += OffSet;
        Teritiary_GO.transform.position += OffSet;

        gameObject.GetComponent<ItemInteractiable>().enabled = false;
    }

    public override void StoreItem()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;

        try
        {
            Destroy(WeaponSpawn.PrimarySpawn.GetChild(0).gameObject);
        }
        catch
        {
            return;
        }

        Destroy(WeaponSpawn.SecoundarySpawn.GetChild(0).gameObject);
        Destroy(WeaponSpawn.TeritiarySpawn.GetChild(0).gameObject);

        HitManagerRef = null;
    }

    public void SetWeaponState()
    {
        try
        {
            WeaponSpawn = Spawns[(int)Type];

        }
        catch
        {
            return;
        }
        
        for (int i = 0; i < Spawns.Length; i++)
        {
            if (i != (int)Type)
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);
            }
        }

        Spawns = new WeaponSpawns[0];
        name = GetName();
    }

    public override bool Equals(Item Item)
    {   
        WeaponHolder weapon = Item as WeaponHolder;

        if (weapon == null)
        {
            return false;
        }

        if (name == weapon.name)
        {
            if (DamageRanges.Count == weapon.DamageRanges.Count)
            {
                for (int i = 0; i < DamageRanges.Count; i++)
                {
                    if (DamageRanges[i].HDamage != weapon.DamageRanges[i].HDamage || DamageRanges[i].Type != weapon.DamageRanges[i].Type)
                    {
                        return false;
                    }
                }
            }

            if (Secoundary == weapon.Secoundary)
            {
                if (Teritiary == weapon.Teritiary)
                {
                    if (CurrentDurability == weapon.CurrentDurability)
                    {
                        if (MaxDurability == weapon.MaxDurability)
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }
}
