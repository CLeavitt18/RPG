using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private int Value;
    [SerializeField] private int Amount;
    [SerializeField] private int Weight;

    [SerializeField] private GameObject _Item;

    [SerializeField] private Color Rarity;

    [SerializeField] private string Name;


    protected void OnEnable() 
    {
        name = Name;
        Rarity = GlobalValues.rarities[0];
    }

    public virtual void SpawnItem()
    {
        Debug.Log("Method not implmented");
    }

    public virtual void StoreItem()
    {
        Debug.Log("Method not implmented");
    }

    public virtual bool Equals(Item Item)
    {
        Debug.Log("Method not implmented");

        return false;
    }

    #region Setters

    public void SetValue(int value)
    {
        Value = value;
    }

    public void SetAmount(int amount)
    {
        Amount = amount;
    }

    public void SetWeight(int weight)
    {
        Weight = weight;
    }

    public void SetItem(GameObject item) 
    {
        _Item = item;
    }

    public void SetRarity(Color rarity)
    {
        Rarity = rarity;
    }

    public void SetName(string name)
    {
        Name = name;
        this.name = name;
    }

    #endregion

    #region Getters

    public int GetValue()
    {
        return Value;
    }

    public int GetAmount()
    {
        return Amount;
    }

    public int GetWeight()
    {
        return Weight;
    }

    public Color GetRarity()
    {
        return Rarity;
    }

    public GameObject GetItem()
    {
        return _Item;
    }

    public string GetName()
    {
        return Name;
    }

    #endregion

    public static Item operator+(Item item1, Item item2)
    {
        item1.Amount += item2.Amount;

        return item1;
    }

    public static Item operator+(Item item, int amount)
    {
        item.Amount += amount;

        return item;
    }

    public static Item operator-(Item item1, int amount)
    {
        item1.Amount -= amount;

        return item1;
    }

    public static Item operator++(Item item)
    {
        item.Amount++;

        return item;
    }

    public static bool operator>=(Item item, int amount)
    {
        if (item.Amount >= amount)
        {
            return true;
        }

        return false;
    }

    public static bool operator<=(Item item, int amount)
    {
        if (item.Amount <= amount)
        {
            return true;
        }

        return false;
    }

    public static Item operator--(Item item)
    {
        item.Amount--;

        return item;
    }
}
