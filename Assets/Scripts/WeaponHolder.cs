using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : Item
{
    [SerializeField] private SkillType SkillType;

    [SerializeField] private HandType HandType;

    [SerializeField] private int LifeSteal;
    [SerializeField] private int PwrAttackDamage;
    [SerializeField] private int CurrentDurability;
    [SerializeField] private int MaxDurability;
    [SerializeField] private int CritDamage;

    [SerializeField] private List<int> StatusChance;
    
    [SerializeField] private float ActionsPerSecond;
    
    [SerializeField] private List<DamageType> DamageRanges;

    [SerializeField] private WeaponSpawns WeaponSpawn;

    [SerializeField] private WeaponSpawns[] Spawns;

    [SerializeField] private GameObject Primary;
    [SerializeField] private GameObject Secoundary;
    [SerializeField] private GameObject Teritiary;

    [SerializeField] private Material Material;

    [SerializeField] private MaterialType[] Materials = new MaterialType[3];

    [SerializeField] private WeaponHitManager HitManagerRef;
    
    [SerializeField] private WeaponType Type;

    [SerializeField] private RuntimeAnimatorController[] Animator = new RuntimeAnimatorController[2];

    [SerializeField] private string AttackAnimationName;
    [SerializeField] private string PwrAttackAnimationName;


    public void Attack(bool state, DamageStats Damage = null)
    {
        if (HitManagerRef == null)
        {
            return;
        }

        HitManagerRef.GetComponent<Collider>().enabled = state;
        
        if (state)
        {
            HitManagerRef.Stats.SetStats(Damage);
        }
        else
        {
            HitManagerRef.SetAlreadyHitFalse();
        }
    }

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

        Vector3 OffSet = HandPosition - StartingPosition;

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

    public void SetWeaponState(WeaponStats stats, bool fromMinion = false)
    {
        if(fromMinion)
        {
            DamageRanges.Add(stats.DamageRanges[0]);

            return;
        }
        
        if (Primary != null)
        {
            Debug.Log("Weapon State already set");
            return;
        }

        SkillType = stats.WeaponSkillType;
        HandType = stats.HandType;
        Type = stats.Type;

        Material = stats.Material;

        LifeSteal = stats.LifeSteal;
        CritDamage = stats.CritDamage;
        PwrAttackDamage = stats.PwrAttackDamage;

        Primary = stats.Primary;
        Secoundary = stats.Secoundary;
        Teritiary = stats.Teritiary;

        CurrentDurability = stats.CurrentDurability;
        MaxDurability = stats.MaxDurability;

        for (int i = 0; i < stats.StatusChance.Count; i++)
        {
            StatusChance.Add(stats.StatusChance[i]);
        }

        for (int i = 0; i < stats.DamageRanges.Count; i++)
        {
            DamageRanges.Add(stats.DamageRanges[i]);
        }

        Animator[0] = stats.Animator[0];
        Animator[1] = stats.Animator[1];
        
        Materials[0] = stats.Materials[0];
        Materials[1] = stats.Materials[1];
        Materials[2] = stats.Materials[2];

        ActionsPerSecond = stats.ActionsPerSecond;

        AttackAnimationName = stats.AttackAnimationName;
        PwrAttackAnimationName = stats.PwrAttackAnimationName;

        base.SetStats(stats);

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

    public void DecrementDurability()
    {
        if (CurrentDurability == 0)
        {
            return;
        }

        CurrentDurability--;
    }

    public SkillType GetSkill()
    {
        return SkillType;
    }

    public HandType GetHandType()
    {
        return HandType;
    }

    public int GetLifeSteal()
    {
        return LifeSteal;
    }

    public int GetPowerAttack()
    {
        return PwrAttackDamage;
    }

    public int GetDurability()
    {
        return CurrentDurability;
    }

    public int GetMaxDurability()
    {
        return MaxDurability;
    }

    public int GetCrit()
    {
        return CritDamage;
    }

    public int GetStatusCount()
    {
        return StatusChance.Count;
    }

    public int GetStatus(int id)
    {
        return StatusChance[id];
    }

    public float GetAttackSpeed()
    {
        return ActionsPerSecond;
    }

    public int GetDamageRangesCount()
    {
        return DamageRanges.Count;
    }

    public int GetLowerRange(int id)
    {
        return DamageRanges[id].LDamage;
    }

    public int GetUpperRange(int id)
    {
        return DamageRanges[id].HDamage;
    }

    public DamageTypeEnum GetDamageType(int id)
    {
        return DamageRanges[id].Type;
    }

    public GameObject GetPrimary()
    {
        return Primary;
    }

    public GameObject GetSecoundary()
    {
        return Secoundary;
    }

    public GameObject GetTeritiary()
    {
        return Teritiary;
    }

    public Material GetMaterial()
    {
        return Material;
    }

    public MaterialType GetMaterialType(int id)
    {
        return Materials[id];
    }

    public WeaponHitManager GetHitManager()
    {
        return HitManagerRef;
    }

    public WeaponType GetWeaponType()
    {
        return Type;
    }

    public RuntimeAnimatorController GetAnimationController(int id)
    {
        return Animator[id];
    }

    public string GetAttackAnimationName()
    {
        return AttackAnimationName;
    }

    public string GetPwrAttackAnimationName()
    {
        return PwrAttackAnimationName;
    }
}
