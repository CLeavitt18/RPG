
public class QuestItemHolder : Item
{
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
        if (Item == null)
        {
            return false;
        }

        if (Item is QuestItemHolder questItem)
        {
            if (questItem.GetName() == GetName())
            {
                return true;
            }
        }
        
        return false;
    }
}
