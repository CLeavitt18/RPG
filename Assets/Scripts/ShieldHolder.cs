using UnityEngine;

public class ShieldHolder : ArmourHolder
{
    [SerializeField] private GameObject leftHandModel;

    [SerializeField] private RuntimeAnimatorController[] animatorController = new RuntimeAnimatorController[2];

    public void SpawnShield(int handType, LivingEntities parent)
    {
        Transform shield = null;

        if (handType == 0)
        {
            shield = Instantiate(GetItem(), transform).transform;
        }
        else
        {
            shield = Instantiate(leftHandModel, transform).transform;
        }

        shield.GetComponent<ShieldHitDetector>().SetState(GetArmour(), parent);
    }
}
