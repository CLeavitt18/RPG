using UnityEngine;

public class StatusManager : MonoBehaviour
{
    [SerializeField] private FireStatusManager fireManager;

    [SerializeField] private LightningStatusManager lightningManager;

    [SerializeField] private IceStatusManager iceManager;


    [SerializeField] private LivingEntities self;

    public void OnEnable()
    {
        self = GetComponent<LivingEntities>();
    }

    public void RunCalculs()
    {
        fireManager.RunCalc(self.GetSkillLevel((int)SkillType.Pyromancy));
        lightningManager.RunCalc(self.GetSkillLevel((int)SkillType.Astromancy));
        iceManager.RunCalcs(self.GetSkillLevel((int)SkillType.Cryomancy));
    }

    public void StartBurning(LivingEntities target, int damage)
    {
        //Debug.Log("Burning status called " + gameObject.ToString());
        fireManager.StartBurning(target, damage);
    }

    public void StartChain(LivingEntities target, int damage, int chains)
    {
        lightningManager.StartChain(target, damage, chains);
    }

    public void StartChill(LivingEntities target)
    {
        iceManager.StartChill(target);
    }

    public int GetBurnDamage()
    {
        return fireManager.GetBurnDamage();
    }

    public int GetTicks()
    {
        return fireManager.GetTicks();
    }

    public float GetWaitTime()
    {
        return fireManager.GetWaitTime();
    }

    public int GetChainDamage()
    {
        return lightningManager.GetChainDamage();
    }

    public int GetChains()
    {
        return lightningManager.GetChains();
    }

    public float GetChainLength()
    {
        return lightningManager.GetChainLength();
    }

    public int GetChillAffect()
    {
        return iceManager.GetChillAffect();
    }

    public float GetChillDuration()
    {
        return iceManager.GetChillDuration();
    }
}
