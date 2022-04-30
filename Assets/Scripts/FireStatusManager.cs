using UnityEngine;

public class FireStatusManager : MonoBehaviour
{
    [SerializeField] private BurningStatus statusAffect;

    [SerializeField] private int stacks;

    [SerializeField] private bool canStack;
    [SerializeField] private bool burning;

    public void StartBurning(LivingEntities target, int damage)
    {
        if (burning && !canStack)
        {
            return;
        }

        BurningStatus status = null;

        switch (statusAffect.GetStatusType())
        {
            case StatusTypeEnum.BaseType:
                status = target.gameObject.AddComponent<BurningStatus>();
                //Debug.Log("Burning Started on Target " + target.name);
                break;
            case StatusTypeEnum.AdvancedBase:
                status = target.gameObject.AddComponent<ReflectBurning>();
                break;
            case StatusTypeEnum.SecondType:
                break;
            case StatusTypeEnum.AdvancedSecondType:
                break;
            case StatusTypeEnum.ThirdType:
                break;
            case StatusTypeEnum.AdvacnedThirdType:
                break;
        }

        status.SetStats(statusAffect);
        status.CallStartAffect(GetComponent<LivingEntities>(), damage);
    }

    public void RunCalc(int level)
    {
        statusAffect.CalculateStats(level);
    }

    public void CanReflect()
    {
        Destroy(statusAffect);

        statusAffect = gameObject.AddComponent<ReflectBurning>();
        statusAffect.CalculateStats(GetComponent<LivingEntities>().GetSkillLevel((int)SkillType.Pyromancy));
    }

    public void SetIsBurning(bool state)
    {
        burning = state;
    }

    public void Setstacks(int num)
    {
        stacks += num;
    }

    public bool GetBurning()
    {
        return burning;
    }

    public bool GetCanStack()
    {
        return canStack;
    }

    public int GetStacks()
    {
        return stacks;
    }
}
