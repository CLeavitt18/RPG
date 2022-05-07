

public class RuneHolder : Item
{
    public Spell spell;

    public override void StoreItem()
    {
        base.StoreItem();
    }

    public override void SpawnItem()
    {
        base.SpawnItem();
    }

    public override bool Equals(Item Item)
    {
        if (Name == Item.Name)
        {
            if (spell.Equals((Item as RuneHolder).spell))
            {
                return true;
            }
        }

        return false;
    }
}