using System.Collections;
using UnityEngine;

public class ChillAffect : MonoBehaviour
{
    [SerializeField] private int chillAffect;

    [SerializeField] private float duration;
    
    
    public virtual void CalculateStats(int level)
    {
        chillAffect = 25;
        float TempChillA = 25f;
        duration = 3.0f;

        float Temp;

        Temp = (float)Skills[(int)SkillType.Cryomancy].Level * .01f;
        TempChillA *= 1 + Temp;
        Hands[HandType].Stats.ChillAffect = (int)TempChillA;
        Hands[HandType].Stats.ChillDuration *= 1 + Temp;
    }

    public virtual IEnumerator CallStartChill(LivingEntities target)
    {
        yield break;
    }
}
