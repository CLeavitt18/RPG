using UnityEngine;

public class TorchHolder : Item
{
    public override void SpawnItem()
    {
        base.SpawnItem();
    }

    public void SpawnItemForUse(Transform location)
    {
        Transform place = Instantiate(GetItem(), location).transform;

        Vector3 start = place.GetChild(0).position;
        Vector3 end = location.position;

        Vector3 offset = end - start;

        place.position += offset;
    }

    public override void StoreItem()
    {
        base.StoreItem();
    }

    public override bool Equals(Item Item)
    {
        if (Item == null || CompareTag(Item.tag) == false)
        {
            return false;
        }

        return true;
    }
}
