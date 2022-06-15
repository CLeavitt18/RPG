using System.Collections.Generic;
using UnityEngine;

public class PrefabIDs : MonoBehaviour
{
    public static PrefabIDs prefabIDs;

    public GameObject WeaponHolder;
    public GameObject ArmourHolder;
    public GameObject ShieldHolder;
    public GameObject SpellHolder;
    public GameObject RuneHolder;
    public GameObject Gold;
    public GameObject LightningChain;
    public GameObject BurnAffect;
    public GameObject ChilledAffect;

    public RuntimeAnimatorController[] ShieldAnimators;

    public GameObject[] Minions;
    public GameObject[] Items;
    public GameObject[] WeaponParts;
    public GameObject[] SpellAffects;
    public GameObject[] Potions;
    public GameObject[] CraftingMaterials;
    public GameObject[] Deposits;
    public GameObject[] Quests;

    public GameObject[] Armour;

    public Material[] WeaponMaterials;

    public RuntimeAnimatorController[] Animators;

    public void Awake()
    {
        if (prefabIDs == null)
        {
            prefabIDs = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
