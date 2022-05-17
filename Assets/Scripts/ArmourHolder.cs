using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourHolder : Item, IEquipable
{
    public int Armour;
    public int CurrentDurability;
    public int MaxDurability;

    [Range(-95, 95)] public int[] Resistences = new int[3];

    public ArmourType ArmourType;

    public List<Power> Enchantments;

    public SkillType SkillType { get { return _skillType; } set { _skillType = value; } }

    public bool IsEquiped { get { return _isEquiped; } set { _isEquiped = value; } }

    [SerializeField] private SkillType _skillType;

    [SerializeField] private bool _isEquiped;

    public override void SpawnItem()
    {
        base.SpawnItem();
    }

    public override void StoreItem()
    {
        base.StoreItem();
    }

    public void SetArmourState()
    {
        name = GetName();
    }

    public override bool Equals(Item Item)
    {
        ArmourHolder armourH = Item as ArmourHolder;

        if (armourH == null)
        {
            return false;
        }

        if (armourH.GetName() == GetName())
        {
            if (armourH.Armour == Armour)
            {
                if (armourH.Enchantments.Count == Enchantments.Count)
                {
                    for (int i = 0; i < Enchantments.Count; i++)
                    {
                        if (Enchantments[i].PowerType != armourH.Enchantments[i].PowerType || Enchantments[i].PowerID != armourH.Enchantments[i].PowerID)
                        {
                            return false;
                        }
                    }

                    if (armourH._skillType == SkillType)
                    {
                        if (armourH.GetWeight() == GetWeight())
                        {
                            return true;
                        }
                    }
                }

            }
        }

        return false;
    }
}
