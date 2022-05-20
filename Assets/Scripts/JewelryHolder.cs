using UnityEngine;
using System.Collections.Generic;

public class JewelryHolder : Item
{
    public SkillType SkillType;

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
