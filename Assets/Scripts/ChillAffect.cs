using System.Collections;
using UnityEngine;

public class ChillAffect : MonoBehaviour
{
    [SerializeField] private StatusTypeEnum _type;

    [SerializeField] private LivingEntities self;

    [SerializeField] private int chillAffect;

    [SerializeField] private float duration;


    private void OnEnable()
    {
        self = GetComponent<LivingEntities>();
    }

    public void CallStartAffect(LivingEntities origin)
    {
        StartCoroutine(StartAffect(origin));
    }

    public virtual IEnumerator StartAffect(LivingEntities origin)
    {
        IceStatusManager manager = GetComponent<IceStatusManager>();

        manager.SetChilled(true);

        GameObject ChilledAffectTemp = Instantiate(PrefabIDs.prefabIDs.ChilledAffect, transform.position, transform.rotation, gameObject.transform);

        self.ReduceActionSpeed((float)chillAffect * .01f);

        yield return new WaitForSeconds(duration);

        manager.SetChilled(false);
        Destroy(ChilledAffectTemp);

        self.SetActionSpeedDefault();

        Destroy(this);
    }

    public virtual void SetStats(ChillAffect affect)
    {
        chillAffect = affect.chillAffect;
        duration = affect.duration;
    }

    public virtual void CalculateStats(int level)
    {
        float TempChillA = 25f;
        chillAffect = 25;
        duration = 3.0f;

        float Temp = level * .01f;

        TempChillA *= 1 + Temp;
        chillAffect = (int)TempChillA;
        duration *= 1 + Temp;
    }

    protected void SetType(StatusTypeEnum type)
    {
        _type = type;
    }

    public StatusTypeEnum GetStatusType()
    {
        return _type;
    }
}
