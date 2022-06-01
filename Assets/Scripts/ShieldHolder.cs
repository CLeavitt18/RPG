using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHolder : ArmourHolder
{
    [SerializeField] private RuntimeAnimatorController[] animatorController = new RuntimeAnimatorController[2];

    public void SpawnShield(int handType, LivingEntities parent)
    {
        
    }
}
