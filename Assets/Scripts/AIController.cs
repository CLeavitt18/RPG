using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIController : LivingEntities
{
    public Behaviuor Mode;
    
    public AI Controller;

    public EntityScaler ScalingValues;

    public float LookRad;
    public float StoppingDistance;
    public float AttackRange;
    public float AttackRollWaitTime;

    public bool IsHumanoid = true;

    public Animator animator;

    public string HitAnimationTriggerName;
    public string DeathAnimationBoolName;
    public string Name;

    public Transform Target;

    [SerializeField] protected NavMeshAgent Agent;

    protected NavMeshPath Path;

    protected float NextRoll;


    public void OnEnable()
    {
        Agent = gameObject.transform.parent.gameObject.GetComponent<NavMeshAgent>();
        RayDistance = AttackRange;
        gameObject.transform.parent.gameObject.name = Name;
        gameObject.name = Name;
        Path = new NavMeshPath();
        Inventory.InventroyHolder = GameObject.Find("NPCInventoryHolder").transform;
    }

    public bool SetStats(bool priority)
    {
        if (!priority)
        {
            switch (GetType())
            {
                case EntityType.Enemy:
                case EntityType.NPC:
                    SetLevel(Player.player.GetLevel());
                    break;
                case EntityType.Minion:
                    SetLevel((Controller as Minion).Owner.GetLevel());
                    break;
                default:
                    break;
            }


            for (int i = 0; i < 3; i++)
            {
                CalculateAttribute(i);
            }

            if (GetType() != EntityType.Minion)
            {
                CreateEnemyWeapon();
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    if (GetHeldItem(i) == null)
                    {
                        continue;
                    }

                    MeleeDamageMulti(i, (Controller as Minion).Type);
                }
            }
        }

        ResetPowers();

        CalculateStats();
        CalculateWeight();
        StatusManger.RunCalculs();
        CalculateSpeed();

        return true;
    }

    public void CreateEnemyWeapon()
    {
        int ID = 0;

        int level = GetLevel();

        for (int i = 1; i < ScalingValues.ThreshHolds.Length; i++)
        {
            if (level == ScalingValues.ThreshHolds[i - 1])
            {
                ID = i - 1;
                break;
            }
            else if ((level > ScalingValues.ThreshHolds[i - 1] && level < ScalingValues.ThreshHolds[i]) ||
                i == ScalingValues.ThreshHolds.Length - 1)
            {
                ID = i;
                break;
            }
        }

        EnemyWeaponData weaponData = ScalingValues.WeaponData[ID];

        WeaponType WeaponType = weaponData.Type;

        int primary = (int)weaponData.PrimaryIds;
        int secoundary = (int)weaponData.SecondaryIds;
        int tertiary = (int)weaponData.TertiaryIds;
        int cat = (int)weaponData.WeaponCatayst;

        Item WeaponH = Roller.roller.CreateWeapon(WeaponType, primary, secoundary, tertiary, cat).GetComponent<Item>();

        Inventory.AddItem(WeaponH, false, 1);

        EquipItem(WeaponH, 0);

        if (GetMastery() == MasteryType.DualWieldingMelee)
        {
            weaponData = ScalingValues.OffWeaponData[ID];

            WeaponType = weaponData.Type;

            primary = (int)weaponData.PrimaryIds;
            secoundary = (int)weaponData.SecondaryIds;
            tertiary = (int)weaponData.TertiaryIds;
            cat = (int)weaponData.WeaponCatayst;

            WeaponH = Roller.roller.CreateWeapon(WeaponType, primary, secoundary, tertiary, cat).GetComponent<Item>();

            Inventory.AddItem(WeaponH, false, 1);

            EquipItem(WeaponH, 1);
        }
    }

    protected void Update()
    {
        if (Dead || Time.timeScale == 0 || Agent.enabled == false)
        {
            return;
        }

        switch (Mode)
        {
            case Behaviuor.Hostile:
                Controller.BehaviuorOnUpdate();
                break;
            case Behaviuor.Passive:
                break;
            case Behaviuor.Neutrel:
                break;
            default:
                break;
        }
    }

    //Only Called from classes that inherante from AI
    public void BehaviuorOnUpdate()
    {
        if (Target == null || Target.GetComponent<LivingEntities>().GetDead())
        {
            FindNewTarget();
        }

        float Distance = Vector3.Distance(Target.position, gameObject.transform.parent.position);

        Ray DetectionRay = new Ray(RaySpawn.position, RaySpawn.forward);

        if (Physics.Raycast(DetectionRay, out Hit, 40))
        {
            if (Hit.collider.gameObject == Target.gameObject || Hit.collider.CompareTag(GlobalValues.ShieldTag))
            {
                if (GetPath(Path, transform.position, Target.position, NavMesh.AllAreas) == true)
                {
                    Agent.SetPath(Path);
                }
            }
        }

        if (Distance <= LookRad)
        {
            if (GetPath(Path, transform.position, Target.position, NavMesh.AllAreas) == true)
            {
                if (GetPathLenght(Path) <= LookRad)
                {
                    Agent.SetPath(Path);
                }
            }
        }

        if (Distance <= Agent.stoppingDistance + 0.5f)
        {
            FaceTarget();
        }

        if (Distance <= AttackRange && Time.time >= NextRoll)
        {
            Ray AttackRay = new Ray(RaySpawn.position, RaySpawn.forward);

            if (Physics.Raycast(AttackRay, out Hit, RayDistance))
            {
                if (Hit.collider.gameObject == Target.gameObject || Hit.collider.CompareTag(GlobalValues.ShieldTag))
                {
                    int Chance;

                    for (int i = 0; i < 2; i++)
                    {
                        Chance = Random.Range(0, 101);

                        if (Chance <= GetAccuracy() && Hands[i].HeldItem != null)
                        {
                            StartCoroutine(Attack(i));
                            Debug.Log("Attack called " + tag);
                        }
                    }

                    NextRoll = Time.time + AttackRollWaitTime;
                }
            }
        }
    }

    public void FindNewTarget()
    {
        Transform currTarget = null;

        float currDistance = 0;

        EntityType type;

        LivingEntities[] entities = FindObjectsOfType<LivingEntities>();

        foreach (LivingEntities entity in entities)
        {
            type = entity.GetType();

            if (entity.GetDead() ||
                (type == EntityType.Minion && ((entity as AIController).Controller  as Minion).Owner is AIController) ||
                (entity is AIController && type == EntityType.Enemy))
            {
                continue;
            }

            float distance = Vector3.Distance(transform.position, entity.transform.position);

            if (distance < currDistance || currDistance == 0)
            {
                currTarget = entity.transform;
                currDistance = distance;
            }
        }

        Target = currTarget;
    }

    public void SetPath()
    {
        if (GetPath(Path, transform.position, Target.position, NavMesh.AllAreas))
        {
            Agent.SetPath(Path);
        }
    }

    public void SetPath(Transform target)
    {
        if (GetPath(Path, transform.position, target.position, NavMesh.AllAreas))
        {
            Agent.SetPath(Path);
        }
    }

    public bool GetPath(NavMeshPath Path, Vector3 FromPos, Vector3 ToPos, int PassableMask)
    {
        Path.ClearCorners();

        return NavMesh.CalculatePath(FromPos, ToPos, PassableMask, Path);
    }

    public float GetPathLenght(NavMeshPath Path)
    {
        float Lenght = 0.0f;

        if ((Path.status != NavMeshPathStatus.PathInvalid) && (Path.corners.Length > 1))
        {
            for (int i = 1; i < Path.corners.Length; i++)
            {
                Lenght += Vector3.Distance(Path.corners[i - 1], Path.corners[i]);
            }
        }

        return Lenght;
    }

    public void FaceTarget()
    {
        Vector3 direction = (Target.position - gameObject.transform.parent.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        transform.parent.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public override int TakeDamage(DamageStats stats, bool shieldHit)
    {
        int TotalDamage = base.TakeDamage(stats, shieldHit);

        animator.SetTrigger(HitAnimationTriggerName);
        CheckHealth();

        if (Mode == Behaviuor.Neutrel)
        {
            Mode = Behaviuor.Hostile;

            if (Controller is NPC)
            {
                Player.player.SetMinionBehaviuors(this);
            }
        }

        return TotalDamage;
    }

    public override void CalculateSpeed()
    {
        base.CalculateSpeed();

        Agent.speed = Speed;
    }

    public override void CheckHealth()
    {
        if (Dead)
        {
            return;
        }

        if (GetCurrentHealth() <= 0)
        {
            Controller.CallDeath(true);
        }
    }

    public void CallDeath(bool Animate)
    {
        StartCoroutine(Death(Animate));
    }

    protected IEnumerator Death(bool Animate)
    {
        Dead = true;
        gameObject.transform.parent.gameObject.GetComponent<NavMeshAgent>().enabled = false;

        if (Animate)
        {
            animator.SetTrigger(DeathAnimationBoolName);
        }

        yield return new WaitForSecondsRealtime(animator.speed);

        animator.speed = 0;

        for (int i = 0; i < 2; i++)
        {
            if (Hands[i].HeldItem != null)
            {
                Hands[i].Animator.enabled = false;

                Hands[i].HeldItem.GetComponent<Item>().StoreItem();
            }
        }

        gameObject.GetComponent<AIController>().enabled = false;
    }

    public void CalculateStats()
    {
        int level = GetLevel();
        int ability = 0;

        for (int i = 0; i < 3; i++)
        {
            ability = (level * ScalingValues.AbilitiesPerLevel[i]) + ScalingValues.BaseAbilities[i];

            SetAbility(i, ability);
        }

        for (int i = 0; i < Skills.Length; i++)
        {
            Skills[i].Level = (level * ScalingValues.SkillWeights[i]) + ScalingValues.BaseSkills[i];
        }

        for (int i = 0; i < Masteries.Length; i++)
        {
            Masteries[i].Level = (level * ScalingValues.MasteryWeights[i]) + ScalingValues.BaseMasteries[i];
        }
    }

    protected override void CalculateAttribute(int attribute)
    {
        base.CalculateAttribute(attribute);

        double multi = GetLevel() * 0.40;

        long max = (long)(GetMaxAttribute(attribute) * multi);

        SetMaxAttribute(attribute, max);
        SetCurrentAttribute(attribute, max);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, LookRad);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, StoppingDistance);

        Gizmos.color = Color.magenta;
        Vector3 Direction = (RaySpawn.TransformDirection(Vector3.forward * RayDistance));
        Gizmos.DrawRay(RaySpawn.position, Direction);

        Gizmos.color = Color.green;
        Direction = RaySpawn.TransformDirection(Vector3.forward * 40);
        Gizmos.DrawRay(RaySpawn.position, Direction);
    }

    public void SetAgentActive(bool state)
    {
        Agent.enabled = state;
    }
}
