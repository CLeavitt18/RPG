using UnityEngine;

public class RuneHolder : Item
{
    [SerializeField] private Spell _spell = null;

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
        if (GetName() == Item.GetName())
        {
            if (_spell.Equals((Item as RuneHolder)._spell))
            {
                return true;
            }
        }

        return false;
    }

    public Spell GetSpell()
    {
        return _spell;
    }

    public void SetSpell(Spell spell)
    {
        if (_spell != null)
        {
            return;
        }

        _spell = spell;
    }
}
