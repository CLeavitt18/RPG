using System.Collections;

public class ReflectBurning : BurningStatus
{
    protected override IEnumerator StartAffect(LivingEntities origin, int damage)
    {
        if (origin != GetComponent<LivingEntities>())
        {
            origin.StatusManger.StartBurning(origin, damage);
        }

        return base.StartAffect(origin, damage);
    }

    public override void SetStats(BurningStatus bs)
    {
        SetType(StatusTypeEnum.AdvancedBase);

        base.SetStats(bs);
    }
}
