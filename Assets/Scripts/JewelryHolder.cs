using UnityEngine;
using System.Collections.Generic;

public class JewelryHolder : Item, IEquipable
{
    public bool IsEquiped { get { return _isEquiped; } set { _isEquiped = value; } }

    public SkillType SkillType { get { return _skillType; } set { _skillType = value; } }

    [SerializeField] private bool _isEquiped;

    [SerializeField] private SkillType _skillType;

    public int PowerLimit;

    public List<PowerHolder> Powers;

    public override void SpawnItem()
    {
        base.SpawnItem();
    }
    public override void StoreItem()
    {
        base.StoreItem();
    }

    public override bool Equals(Item Item)
    {
        return base.Equals(Item);
    }
}
