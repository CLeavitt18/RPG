using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerHolder : Item
{
    [SerializeField] private Power Power;


    public Power GetPower()
    {
        return Power;
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
