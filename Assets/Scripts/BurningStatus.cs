using System.Collections;
using UnityEngine;

public class BurningStatus : MonoBehaviour
{
    [SerializeField] private LivingEntities self;

    [SerializeField] private StatusTypeEnum _type;

    [SerializeField] private int _damage;
    [SerializeField] private int ticks;

    [SerializeField] private bool burningDamage100;
    [SerializeField] private bool x2Damge;

    [SerializeField] private float waitTime;
    [SerializeField] private float nextTickTime;

    private void OnEnable()
    {
        self = GetComponent<LivingEntities>();
    }

    public void CallStartAffect(LivingEntities origin, int damage)
    {
        StartCoroutine(StartAffect(origin, damage));
    }

    protected virtual IEnumerator StartAffect(LivingEntities origin, int damage)
    {
        FireStatusManager manager = GetComponent<FireStatusManager>();

        manager.SetIsBurning(true);
        manager.Setstacks(1);

        yield return new WaitForEndOfFrame();

        //Debug.Log("This is burning " + name);
        //Debug.Log("ticks = " + ticks);

        int damagePerTick = (int)Mathf.Ceil(((float)damage * ((float)_damage * .01f)));

        for (int i = 0; i < ticks; i++)
        {
            //Debug.Log("Burning");

            if (self.GetDead())
            {
                yield break;
            }

            GameObject BA = Instantiate(PrefabIDs.prefabIDs.BurnAffect, transform.position, transform.rotation, gameObject.transform);

            self.LoseAttribute(damagePerTick, AttributesEnum.Health);

            if (CompareTag(GlobalValues.PlayerTag))
            {
                PlayerUi.playerUi.SetPlayerAttributeUI(0);
            }

            yield return new WaitForSeconds(waitTime * .5f);

            Destroy(BA);

            yield return new WaitForSeconds(waitTime * .5f);

            self.CheckHealth();
        }

        manager.SetIsBurning(false);
        manager.Setstacks(-1);

        Destroy(this);
    }

    public virtual void SetStats(BurningStatus bs)
    {
        _damage = bs._damage;
        ticks = bs.ticks;
        waitTime = bs.waitTime;

        //Debug.Log("Stats set");
    }

    public virtual void CalculateStats(int skillLevel)
    {
        _damage = 10;
        ticks = 12;
        waitTime = .25f;

        float Temp;

        Temp = skillLevel * .01f;
        _damage += (int)(10 * Temp);
        ticks += (int)(12 * Temp);
        waitTime *= 1 - (Temp * .5f);

        //Check shrine affects
        if (burningDamage100)
        {
            SetBurningDamageTo100();
        }

        if (x2Damge)
        {
            SetDoubleBurningDamage();
        }
    }

    #region BoolSetters

    public void X2BurningDamage(bool state)
    {
        x2Damge = state;
    }

    public void BurningDamage100(bool state)
    {
        burningDamage100 = state;
    }

    #endregion

    private void SetBurningDamageTo100()
    {
        _damage = 100;
    }

    private void SetDoubleBurningDamage()
    {
        _damage *= 2;
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
