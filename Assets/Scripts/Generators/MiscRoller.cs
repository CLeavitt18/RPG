using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscRoller : MonoBehaviour
{
    [SerializeField] private GameObject[] Items;

    [SerializeField] private DropTable[] AmountTables;

    public Item RollMisc()
    {
        int Amount = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

        Item Gold = Instantiate(Items[0]).GetComponent<Item>();
        Gold.SetAmount(Amount);

        return Gold;
    }
}
