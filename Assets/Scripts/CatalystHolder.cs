using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatalystHolder : Item
{
    public Catalyst Catalyst;

    public void OnEnable()
    {
        gameObject.name = Name;
    }

    public override void SpawnItem()
    {

    }

    public override void StoreItem()
    {

    }

    public override bool Equals(Item Item)
    {
        return base.Equals(Item);
    }
}
