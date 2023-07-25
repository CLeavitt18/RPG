using UnityEngine;
using UnityEngine.AI;

public class Minion : AI
{
    public LivingEntities Owner;

    public GolemSpell SourceSpell;

    public DamageTypeEnum Type;

    public EntityType OwnerType;

    //minimum distance the minion will be allowed to be away from the owner
    [SerializeField] private float ProtectRange;


    public override void BehaviuorOnUpdate()
    {
        float distance;

        float lookRad = entity.GetLookRad();

        Transform target = entity.Target;

        if (target == null || target.GetComponent<LivingEntities>().GetDead())
        {
            FindNewTarget();
            target = entity.Target;
        }

        distance = Vector3.Distance(target.position, transform.parent.position);

        if (target != Owner.transform && distance <= lookRad) 
        {
            distance = Vector3.Distance(target.position, Owner.transform.position);

            if (distance <= ProtectRange)
            {
                entity.BehaviuorOnUpdate();
                return;
            }
        }

        TrackOwner();
    }

    private void TrackOwner()
    {
        float distance = Vector3.Distance(Owner.transform.position, transform.parent.position);

        if (distance >= ProtectRange)
        {
            entity.SetPath(Owner.transform);
        }
        else
        {
            FaceOwner();
        }
    }

    public void FindNewTarget()
    {
        Transform currTarget = null;
        float currDistance = 0;
        EntityType type;

        LivingEntities[] entites = FindObjectsOfType<LivingEntities>();

        foreach (LivingEntities entity in entites)
        {
            type = entity.GetType();

            if (entity.GetDead() ||
                Owner == entity ||
                (entity.CompareTag(GlobalValues.MinionTag) && entity.GetComponent<Minion>().Owner == Owner) ||
                entity == this)
            {
                continue;
            }

            float distance = Vector3.Distance(entity.transform.position, transform.position);

            if (distance < currDistance ||
                currDistance == 0)
            {
                if (Owner is AIController)
                {
                    if (entity is AIController && (!(type == EntityType.Minion) ||
                       ((entity as AIController).GetController() as Minion).Owner != Player.player))
                    {
                        continue;
                    }
                }
                else
                {
                    if ((type == EntityType.NPC && (entity as AIController).GetMode() != Behaviuor.Hostile) ||
                        (type == EntityType.Minion && ((entity as AIController).GetController() as Minion).Owner == this))
                    {
                        continue;
                    }
                }

                int chance = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

                if (chance >= entity.GetThreatLevel())
                {
                    currTarget = entity.transform;
                    currDistance = distance;
                }
            }
        }

        if (currTarget == null)
        {
            entity.Target = Owner.transform;
        }
        else
        {
            entity.Target = currTarget;
        }
    }

    public void CheckAvailable(Transform target)
    {
        if (entity.Target == null)
        {
            entity.Target = target;
            return;
        }

        float distance = Vector3.Distance(entity.Target.position, transform.parent.position);

        if (distance > entity.GetLookRad())
        {
            entity.Target = target;
        }
    }

    public void Move(Vector3 postion)
    {
        entity.SetAgentActive(false);

        transform.parent.position = postion;

        entity.SetAgentActive(true);
    }

    public void FaceOwner()
    {
        Vector3 direction = (Owner.transform.position - transform.parent.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.parent.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public bool SetStats(bool priority)
    {
        entity.SetStats(priority);

        for (int i = 0; i < 2; i++)
        {
            if (entity.GetHeldItem(i) == null)
            {
                continue;
            }

            SetWeaponStats(i);
        }

        return true;
    }

    public void SetWeaponStats(int HandType)
    {
        float multi = 1f + (entity.GetLevel() - 1f) * .05f;

        int matCatMulti = SourceSpell.gameObject.GetComponent<SpellHolder>().GetValueMulti();

        WeaponHolder fist = entity.GetHeldItem(HandType).GetComponent<WeaponHolder>();

        /*for (int i = 0; i < fist.GetDamageRangesCount(); i++)
        {
            fist.DamageRanges[i].LDamage = (int)(fist.GetLowerRange(i) * (multi + matCatMulti));
            fist.DamageRanges[i].HDamage = (int)(fist.GetUpperRange(i) * (multi + matCatMulti));
        }*/
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, ProtectRange);
    }

    public override void CallDeath(bool Animate)
    {
        int cost = SourceSpell.CalculateManaCost(Owner.GetIntelligence());

        //Debug.Log("cost to be refunded = " + cost);

        Owner.UnReserveAttribute((int)cost, SourceSpell.GetCostType());

        SourceSpell.Alive--;

        if (SourceSpell.Alive == 0)
        {
            SourceSpell.Activated = false;
        }

        Owner.RemoveMinion(this);

        Destroy(transform.parent.gameObject);
    }

    public bool LoadMinion(MinionData Data)
    {
        entity.LoadEntity(Data);

        transform.parent.gameObject.GetComponent<NavMeshAgent>().enabled = false;

        Vector3 Rotation;
        Vector3 Position;
        entity.LoadEntity(Data);

        Position.x = Data.Position[0];
        Position.y = Data.Position[1];
        Position.z = Data.Position[2];

        transform.parent.position = Position;

        Rotation.x = Data.Rotation[0];
        Rotation.y = Data.Rotation[1];
        Rotation.z = Data.Rotation[2];

        Rotation = Vector3.MoveTowards(transform.parent.transform.rotation.eulerAngles, Rotation, 360);
        transform.parent.rotation = Quaternion.Euler(Rotation);

        SetStats(true);

        transform.parent.gameObject.GetComponent<NavMeshAgent>().enabled = true;

        if (Data.IsDead)
        {
            CallDeath(false);
        }

        return true;
    }
}
