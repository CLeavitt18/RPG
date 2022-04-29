using UnityEngine;

public class Key : Item
{
    private void OnEnable()
    {
        name = Name;
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
