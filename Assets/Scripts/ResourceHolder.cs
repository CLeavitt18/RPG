
public class ResourceHolder : Item
{
    public override void SpawnItem()
    {
        base.SpawnItem();
    }

    public override void StoreItem()
    {
        base .StoreItem();
    }

    public override bool Equals(Item Item)
    {
        if (Item == null)
        {
            return false;
        }

        if (GetName() == Item.GetName())
        {
            return true;
        }

        return false;
    }
}
