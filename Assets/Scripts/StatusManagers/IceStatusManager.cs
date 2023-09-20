using UnityEngine;

public class IceStatusManager : MonoBehaviour
{
    [SerializeField] private ChillAffect statusAffect;

    [SerializeField] private bool chilled;
    [SerializeField] private bool frozen;

    public void RunCalcs(int level)
    {
        statusAffect.CalculateStats(level);
    }


    public void StartChill(LivingEntities target)
    {
        if (chilled && frozen)
        {
            return;
        }

        ChillAffect status = null;

        switch (statusAffect.GetStatusType())
        {
            case StatusTypeEnum.BaseType:
                status = target.gameObject.AddComponent<ChillAffect>();
                break;
            case StatusTypeEnum.AdvancedBase:
                break;
            case StatusTypeEnum.SecondType:
                break;
            case StatusTypeEnum.AdvancedSecondType:
                break;
            case StatusTypeEnum.ThirdType:
                break;
            case StatusTypeEnum.AdvacnedThirdType:
                break;
            default:
                break;
        }

        status.SetStats(statusAffect);
        status.CallStartAffect(GetComponent<LivingEntities>());
    }

    public bool GetChilled()
    {
        return chilled;
    }

    public void SetChilled(bool state)
    {
        chilled = state;
    }

    public bool GetFrozen()
    {
        return frozen;
    }

    public void SetFrozen(bool state)
    {
        frozen = state;
    }

    public int GetChillAffect()
    {
        return statusAffect.GetChillAffect();
    }

    public float GetChillDuration()
    {
        return statusAffect.GetChillDuration();
    }
}
