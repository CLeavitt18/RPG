using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStatusManager : MonoBehaviour
{
    [SerializeField] private ChainAffect statusAffect;

    public void RunCalc(int level)
    {
        statusAffect.CalculateStats(level);
    }

    public virtual void StartChain(LivingEntities target, int damage, int chains)
    {
        ChainAffect status = null;

        switch (statusAffect.GetStatusType())
        {
            case StatusTypeEnum.BaseType:
                status = target.gameObject.AddComponent<ChainAffect>();
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

        if (chains == 0)
        {
            status.CallStartAffect(GetComponent<LivingEntities>(), damage, status.GetChains());
        }
        else
        {
            status.CallStartAffect(GetComponent<LivingEntities>(), damage, chains);
        }

    }

    public int GetChainDamage()
    {
        return statusAffect.GetChainDamage();
    }

    public int GetChains()
    {
        return statusAffect.GetChains();
    }

    public float GetChainLength()
    {
        return statusAffect.GetChainLength();
    }
}
