using UnityEngine;

public class TorchHolder : Item, IEquipable
{
    public bool IsEquiped { get; set; }

    public SkillType SkillType { get; set; }

    public override void SpawnItem()
    {
        base.SpawnItem();
    }

    public void SpawnItemForUse(Transform location)
    {
        Transform place = Instantiate(_Item, location).transform;

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
        return base.Equals(Item);
    }
}
