using System.Collections;
using UnityEngine;

public class ChainAffect : MonoBehaviour
{

    [SerializeField] StatusTypeEnum _type;

    [SerializeField] LivingEntities self;

    [SerializeField] int _damage;
    [SerializeField] int _chains;

    [SerializeField] float chainLength;


    private void OnEnable()
    {
        self = GetComponent<LivingEntities>();
    }

    public void CallStartAffect(LivingEntities origin, int damage, int chains)
    {
        StartCoroutine(StartAffect(origin, damage, chains));
    }

    protected virtual IEnumerator StartAffect(LivingEntities origin, int damage, int chains)
    {
        yield return new WaitForEndOfFrame();

        chains--;

        int damagePerChain = (int)Mathf.Ceil(((float)damage * (_damage * .01f)));

        self.LoseAttribute(damagePerChain, AttributesEnum.Health);

        float ClosestTarget = Mathf.Infinity;
        AIController BestTarget = null;

        GameObject[] Enemies = GameObject.FindGameObjectsWithTag(self.tag);

        foreach (GameObject Targets in Enemies)
        {
            float Distance = Vector3.Distance(Targets.transform.position, transform.position);

            if (Distance < ClosestTarget &&
                Distance <= chainLength &&
                Targets != gameObject &&
                Targets.GetComponent<AIController>().GetDead() == false)
            {
                ClosestTarget = Distance;
                BestTarget = Targets.GetComponent<AIController>();
            }
        }

        if (BestTarget == null)
        {
            self.CheckHealth();
            Destroy(this);
            yield break;
        }

        GameObject LC = Instantiate(PrefabIDs.prefabIDs.LightningChain, gameObject.transform.position, gameObject.transform.rotation);
        LC.transform.localScale = new Vector3(LC.transform.localScale.x, LC.transform.localScale.y, ClosestTarget + .5f);
        Vector3 MidPoint = (gameObject.transform.parent.transform.position - BestTarget.transform.parent.position) * .5f;
        LC.transform.position -= MidPoint;

        Vector3 Direction = (BestTarget.transform.position - LC.transform.position);
        Quaternion LookRotation = Quaternion.LookRotation(new Vector3(Direction.x, 0, Direction.z));
        LC.transform.rotation = Quaternion.Slerp(LC.transform.rotation, LookRotation, 360f);

        yield return new WaitForSeconds(.20f);

        Destroy(LC);

        if (chains > 0 && BestTarget != null)
        {
            BestTarget.StatusManger.StartChain(BestTarget, damagePerChain, chains);
        }

        self.CheckHealth();

        Destroy(this);
    }

    public virtual void SetStats(ChainAffect affect)
    {
        _damage = affect._damage;
        _chains = affect._chains;
        chainLength = affect.chainLength;
    }

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
    
    public int GetChains()
    {
        return _chains;
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
