
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

        if (Name == Item.Name)
        {
            return true;
        }

        return false;
    }
}
