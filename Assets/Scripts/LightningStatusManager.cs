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
       ChainAffect status = target.gameObject.AddComponent<ChainAffect>();

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
}
