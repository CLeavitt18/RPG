
public class Consumable : Item, IConsumable
{
    public string Description;

    public LivingEntities PotionHolder;

    public void OnEnable()
    {
        name = Name;
    }


    

    public void Action()
    {
        Effect();
    }

    public virtual void Effect()
    {
        
    }

    protected virtual bool Equality(Item Item)
    {
        return false;
    }

    public override bool Equals(Item Item)
    {
        return Equality(Item);
    }

    public override void SpawnItem()
    {
        base.SpawnItem();
    }

    public override void StoreItem()
    {
        base.StoreItem();
    }
}
