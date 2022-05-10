using UnityEngine;

public class GainPotion : Consumable
{
    public int LowerRange;
    public int UpperRange;

    public AttributesEnum Type;

    public override void Effect()
    {
        int amount = Random.Range(LowerRange, UpperRange + 1);

        long current = 0;
        long max = 0;

        switch (Type)
        {
            case AttributesEnum.Health:
                current = PotionHolder.GetCurrentHealth();
                max = PotionHolder.GetMaxHealth();
                break;
            case AttributesEnum.Stamina:
                current = PotionHolder.GetCurrentStamina();
                max = PotionHolder.GetMaxStamina();
                break;
            case AttributesEnum.Mana:
                current = PotionHolder.GetCurrentMana();
                max = PotionHolder.GetMaxMana();
                break;
        }

        if (current == max)
        {
            return;
        }

        PotionHolder.GainAttribute(amount, Type);

        PotionHolder.Inventory.RemoveItem(this, 1);
        InventoryUi.playerUi.CallSetInventory(InventoryUi.playerUi.Mode);
    }

    protected override bool Equality(Item Item)
    {
        GainPotion potion;

        if (Item == null)
        {
            return false;
        }

        try
        {
            potion = Item as GainPotion;
        }
        catch
        {

            return false;
        }

        if (LowerRange == potion.LowerRange)
        {
            if (UpperRange == potion.UpperRange)
            {
                return true;
            }
        }

        return false;
    }
}
