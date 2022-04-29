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

    public virtual void StartChain(LivingEntities target, int damage)
    {

    }
}
