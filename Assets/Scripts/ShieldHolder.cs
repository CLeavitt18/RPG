using UnityEngine;

public class ShieldHolder : ArmourHolder
{
    [SerializeField] private GameObject leftHandModel;

    [SerializeField] private RuntimeAnimatorController[] animatorController = new RuntimeAnimatorController[2];

    [SerializeField] private ShieldHitDetector detector;

    public void SpawnShield(int handType, LivingEntities parent)
    {
        if (handType == 0)
        {
            detector = Instantiate(GetItem(), transform).GetComponent<ShieldHitDetector>();
        }
        else
        {
            detector = Instantiate(leftHandModel, transform).GetComponent<ShieldHitDetector>();
        }

        detector.SetState(GetArmour(), parent);
    }

    public void SetState(bool state)
    {
        detector.SetProtecting(state);
    }

    public RuntimeAnimatorController GetAnimatorController(int id)
    {
        return animatorController[id];
    }

    public bool GetHit()
    {
        return detector.GetHit();
    }

    public bool GetProtecting()
    {
        return detector.GetProtecting();
    }
}
