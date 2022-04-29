using UnityEngine;

public class Item : MonoBehaviour
{
    public int Value;
    public int Amount;
    public int Weight;

    public GameObject _Item;

    public Color Rarity;

    public string Name;

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
}
