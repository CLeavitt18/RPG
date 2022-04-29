using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscRoller : MonoBehaviour
{
    [SerializeField] private GameObject[] Items;

    [SerializeField] private DropTable[] AmountTables;

    public CreatedItem RollMisc()
    {
        int Amount = Random.Range(GlobalValues.MinRoll + 1, GlobalValues.MaxRoll);

        Item Gold = Instantiate(Items[0]).GetComponent<Item>();
        Gold.Amount = Amount;

        CreatedItem Item = new CreatedItem(Gold, Amount);

        return Item;
    }
}
