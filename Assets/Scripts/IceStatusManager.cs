using UnityEngine;

public class IceStatusManager : MonoBehaviour
{
    [SerializeField] private ChillAffect statusAffect;


    public virtual void CalculateStats(int skillLevel)
    {
        _damage = 25;
        _chains = 3;
        chainLength = 4.5f;

        float Temp;

        Temp = (float)skillLevel * .01f;
        _damage += (int)(25 * Temp);
        _chains += (int)(3 * Temp);
        chainLength *= 1 + Temp;
        chainLength = Mathf.Round(chainLength * 10) * .1f;
    }

    public void StartChill(LivingEntities target)
    {

    }
}
