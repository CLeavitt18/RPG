using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItemHolder : Item
{
    public void OnEnable()
    {
        gameObject.name = Name;
    }
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
