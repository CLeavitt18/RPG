using System.Collections;
using UnityEngine;

public class ChainAffect : MonoBehaviour
{
    [SerializeField] StatusTypeEnum type;

    [SerializeField] int damage;
    [SerializeField] int chains;

    [SerializeField] float chainLength;

    public virtual void CalculateStats(int skillLevel)
    {
        damage = 25;
        chains = 3;
        chainLength = 4.5f;

        float Temp;

        Temp = (float)skillLevel * .01f;
        damage += (int)(25 * Temp);
        chains += (int)(3 * Temp);
        chainLength *= 1 + Temp;
        chainLength = Mathf.Round(chainLength * 10) * .1f;
    }

    public void CallStartAffect(LivingEntities origin, int damage)
    {
        StartCoroutine(StartAffect(origin, damage));
    }

    protected virtual IEnumerator StartAffect(LivingEntities orgin, int damage)
    {
        yield break;
    }
}
